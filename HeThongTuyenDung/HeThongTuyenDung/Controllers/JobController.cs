using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HeThongTuyenDung.Data;
using HeThongTuyenDung.Models;
using HeThongTuyenDung.Helpers;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace HeThongTuyenDung.Controllers
{
    public class JobController : Controller
    {
        private readonly TuyenDungDbContext _context;
        private readonly ILogger<JobController> _logger;

        public JobController(TuyenDungDbContext context, ILogger<JobController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Trang chủ tuyển dụng
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Loading Job Index page...");
            
            var jobs = await _context.TinTuyenDungs
                .Include(t => t.CongTy)
                .Include(t => t.DanhMucCongViec)
                .Include(t => t.CapBacCongViec)
                .Include(t => t.LoaiCongViec)
                .Where(t => t.TrangThai)
                .OrderByDescending(t => t.NgayTao)
                .Take(50) // Tăng từ 12 lên 50 để hiển thị nhiều tin hơn
                .ToListAsync();

            _logger.LogInformation($"Found {jobs.Count} active jobs");
            
            // Log thông tin các tin tuyển dụng để debug
            foreach (var job in jobs.Take(5)) // Chỉ log 5 tin đầu tiên
            {
                _logger.LogInformation($"Job ID: {job.MaTin}, Title: {job.TieuDe}, Status: {job.TrangThai}, Created: {job.NgayTao}, Featured: {job.NoiBat}");
            }

            return View(jobs);
        }

        // Trang chi tiết công việc
        public async Task<IActionResult> Details(int id)
        {
            var job = await _context.TinTuyenDungs
                .Include(t => t.CongTy)
                .Include(t => t.DanhMucCongViec)
                .Include(t => t.CapBacCongViec)
                .Include(t => t.LoaiCongViec)
                .Include(t => t.KyNangTuyenDungs)
                .ThenInclude(kt => kt.KyNang)
                .FirstOrDefaultAsync(t => t.MaTin == id);

            if (job == null)
            {
                return NotFound();
            }

            // Tăng lượt xem
            job.LuotXem++;
            await _context.SaveChangesAsync();

            // Kiểm tra xem người dùng đã lưu việc làm này chưa
            var userId = AuthorizationHelper.GetUserId(HttpContext.Session);
            if (!string.IsNullOrEmpty(userId))
            {
                var savedJob = await _context.TinDaLuus
                    .FirstOrDefaultAsync(t => t.MaTin == id && t.MaNguoiDung == int.Parse(userId));
                ViewBag.IsSaved = savedJob != null;
            }

            return View(job);
        }

        // Trang tìm kiếm công việc
        public async Task<IActionResult> Search(string keyword, string location, int? category)
        {
            var query = _context.TinTuyenDungs
                .Include(t => t.CongTy)
                .Include(t => t.DanhMucCongViec)
                .Include(t => t.CapBacCongViec)
                .Include(t => t.KyNangTuyenDungs)
                .Where(t => t.TrangThai);

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(t => t.TieuDe.Contains(keyword) || t.MoTa.Contains(keyword));
            }

            if (!string.IsNullOrEmpty(location))
            {
                query = query.Where(t => t.DiaDiem.Contains(location) || t.ThanhPho.Contains(location));
            }

            if (category.HasValue)
            {
                query = query.Where(t => t.MaDanhMuc == category);
            }

            var jobs = await query.ToListAsync();

            // Lấy HoSoUngVien của user hiện tại (nếu là Ứng Viên)
            var userId = HttpContext.Session.GetInt32("UserId");
            var userRole = HttpContext.Session.GetString("UserRole");

            // Dictionary lưu Match Score
            var matchScores = new Dictionary<int, int>();

            if (userId.HasValue && userRole == "Candidate")
            {
                var candidate = await _context.HoSoUngViens
                    .Include(h => h.KyNangUngViens)
                    .FirstOrDefaultAsync(h => h.MaNguoiDung == userId);

                if (candidate != null)
                {
                    foreach (var job in jobs)
                    {
                        matchScores[job.MaTin] = HeThongTuyenDung.Helpers.MatchCalculator.CalculateMatchScore(job, candidate);
                    }

                    // Sắp xếp theo độ phù hợp giảm dần, rồi đến ngày tạo
                    jobs = jobs.OrderByDescending(j => matchScores.ContainsKey(j.MaTin) ? matchScores[j.MaTin] : 0)
                               .ThenByDescending(j => j.NgayTao)
                               .ToList();
                }
                else
                {
                     // Mặc định sắp xếp theo ngày tạo nếu chưa có hồ sơ
                     jobs = jobs.OrderByDescending(j => j.NgayTao).ToList();
                }
            }
            else
            {
                // Người dùng chưa đăng nhập hoặc không phải ứng viên
                jobs = jobs.OrderByDescending(j => j.NgayTao).ToList();
            }

            ViewBag.Keyword = keyword;
            ViewBag.Location = location;
            ViewBag.Category = category;
            ViewBag.Categories = await _context.DanhMucCongViecs.ToListAsync();
            ViewBag.MatchScores = matchScores; // Truyền Dictionary qua ViewBag

            return View(jobs);
        }

        // Trang đăng ký ứng tuyển
        [RequireCandidate]
        public async Task<IActionResult> Apply(int id)
        {
            var job = await _context.TinTuyenDungs
                .Include(t => t.CongTy)
                .FirstOrDefaultAsync(t => t.MaTin == id);

            if (job == null)
            {
                return NotFound();
            }

            // Kiểm tra xem đã ứng tuyển chưa
            var userId = AuthorizationHelper.GetUserIdAsInt(HttpContext.Session);
            var existingApplication = await _context.DonUngTuyens
                .FirstOrDefaultAsync(d => d.MaTin == id && d.MaNguoiDung == userId);

            if (existingApplication != null)
            {
                TempData["Warning"] = "Bạn đã ứng tuyển cho vị trí này rồi!";
                return RedirectToAction(nameof(Details), new { id });
            }

            return View(job);
        }

        [HttpPost]
        [RequireCandidate]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Apply(int id, [FromForm] ApplicationFormModel formData)
        {
            _logger.LogInformation($"=== BẮT ĐẦU XỬ LÝ ĐƠN ỨNG TUYỂN ===");
            _logger.LogInformation($"Job ID: {id}");
            
            var userId = AuthorizationHelper.GetUserIdAsInt(HttpContext.Session);
            if (!userId.HasValue)
            {
                _logger.LogWarning("User chưa đăng nhập");
                return RedirectToAction("Login", "Account");
            }

            _logger.LogInformation($"User ID: {userId.Value}");

            // Kiểm tra job có tồn tại không
            var job = await _context.TinTuyenDungs
                .Include(t => t.CongTy)
                .FirstOrDefaultAsync(t => t.MaTin == id);

            if (job == null)
            {
                _logger.LogWarning($"Job ID {id} không tồn tại");
                return NotFound();
            }

            // Kiểm tra xem đã ứng tuyển chưa
            var existingApplication = await _context.DonUngTuyens
                .FirstOrDefaultAsync(d => d.MaTin == id && d.MaNguoiDung == userId);

            if (existingApplication != null)
            {
                _logger.LogWarning($"User {userId.Value} đã ứng tuyển job {id} rồi");
                TempData["Warning"] = "Bạn đã ứng tuyển cho vị trí này rồi!";
                return RedirectToAction(nameof(Details), new { id });
            }

            // Kiểm tra hạn nộp
            if (job.HanNop.HasValue && job.HanNop.Value < DateTime.Now)
            {
                _logger.LogWarning($"Job {id} đã hết hạn nộp đơn");
                TempData["Error"] = "Đã hết hạn nộp đơn ứng tuyển cho vị trí này!";
                return RedirectToAction(nameof(Details), new { id });
            }

            // Validate file upload
            string? cvFileName = null;
            if (formData.FileCV != null)
            {
                var allowedExtensions = new[] { ".pdf", ".doc", ".docx" };
                var fileExtension = Path.GetExtension(formData.FileCV.FileName).ToLowerInvariant();
                
                if (!allowedExtensions.Contains(fileExtension))
                {
                    _logger.LogWarning($"File không đúng định dạng: {formData.FileCV.FileName}");
                    ModelState.AddModelError("FileCV", "Chỉ chấp nhận file PDF, DOC, DOCX");
                }
                else if (formData.FileCV.Length > 5 * 1024 * 1024) // 5MB
                {
                    _logger.LogWarning($"File quá lớn: {formData.FileCV.Length} bytes");
                    ModelState.AddModelError("FileCV", "File không được lớn hơn 5MB");
                }
                else
                {
                    // Tạo tên file unique
                    cvFileName = $"CV_{userId.Value}_{DateTime.Now:yyyyMMdd_HHmmss}_{Path.GetFileName(formData.FileCV.FileName)}";
                    var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "cv");
                    
                    // Tạo thư mục nếu chưa có
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }
                    
                    var filePath = Path.Combine(uploadPath, cvFileName);
                    
                    try
                    {
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await formData.FileCV.CopyToAsync(stream);
                        }
                        _logger.LogInformation($"Đã lưu file CV: {cvFileName}");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Lỗi khi lưu file CV: {ex.Message}");
                        ModelState.AddModelError("FileCV", "Lỗi khi upload file CV");
                    }
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _logger.LogInformation("Bắt đầu lưu đơn ứng tuyển...");
                    
                    // Tạo đơn ứng tuyển mới
                    var application = new DonUngTuyen
                    {
                        MaTin = id,
                        MaNguoiDung = userId.Value,
                        NgayUngTuyen = DateTime.Now,
                        TrangThai = "Chờ xử lý",
                        DuongDanCV = cvFileName,
                        DaXem = false,
                        
                        // Thông tin từ form
                        HoTen = formData.HoTen,
                        Email = formData.Email,
                        SoDienThoai = formData.SoDienThoai,
                        DiaChi = formData.DiaChi,
                        KinhNghiem = formData.KinhNghiem,
                        HocVan = formData.HocVan,
                        MucTieu = formData.MucTieu,
                        KyNang = formData.KyNang,
                        KinhNghiemLamViec = formData.KinhNghiemLamViec,
                        DuAn = formData.DuAn,
                        LyDoUngTuyen = formData.LyDoUngTuyen,
                        LuongMongMuon = formData.LuongMongMuon,
                        ThoiGianBatDau = formData.ThoiGianBatDau,
                        ThongTinBoSung = formData.ThongTinBoSung,
                        
                        // Thông tin khác
                        MucLuongMongMuon = formData.LuongMongMuon,
                        DonViTien = "VND",
                        NgayCapNhat = DateTime.Now
                    };

                    // Log thông tin đơn ứng tuyển
                    _logger.LogInformation($"Thông tin đơn ứng tuyển:");
                    _logger.LogInformation($"- Họ tên: {application.HoTen}");
                    _logger.LogInformation($"- Email: {application.Email}");
                    _logger.LogInformation($"- Số điện thoại: {application.SoDienThoai}");
                    _logger.LogInformation($"- Kinh nghiệm: {application.KinhNghiem}");
                    _logger.LogInformation($"- Học vấn: {application.HocVan}");
                    _logger.LogInformation($"- File CV: {cvFileName}");

                    _context.DonUngTuyens.Add(application);
                    await _context.SaveChangesAsync();

                    _logger.LogInformation($"Đã lưu đơn ứng tuyển thành công! ID: {application.MaDon}");

                    // Tăng lượt xem cho job
                    job.LuotXem++;
                    await _context.SaveChangesAsync();

                    // Tạo thông báo cho nhà tuyển dụng
                    var notification = new ThongBao
                    {
                        MaNguoiDung = job.MaNguoiDung,
                        TieuDe = "Có đơn ứng tuyển mới",
                        NoiDung = $"Có đơn ứng tuyển mới cho vị trí '{job.TieuDe}' từ ứng viên {formData.HoTen}",
                        LoaiThongBao = "Đơn ứng tuyển",
                        DaXem = false,
                        NgayTao = DateTime.Now,
                        MaLienQuan = job.MaTin
                    };

                    _context.ThongBaos.Add(notification);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Đơn ứng tuyển đã được gửi thành công! Chúng tôi sẽ liên hệ với bạn sớm nhất.";
                    _logger.LogInformation("=== ĐƠN ỨNG TUYỂN THÀNH CÔNG ===");
                    return RedirectToAction(nameof(Details), new { id });
                }
                catch (Exception ex)
                {
                    _logger.LogError($"LỖI KHI LƯU ĐƠN ỨNG TUYỂN: {ex.Message}");
                    _logger.LogError($"Stack trace: {ex.StackTrace}");
                    
                    if (ex.InnerException != null)
                    {
                        _logger.LogError($"INNER EXCEPTION: {ex.InnerException.Message}");
                    }
                    
                    ModelState.AddModelError("", $"Lỗi khi gửi đơn ứng tuyển: {ex.Message}");
                }
            }
            else
            {
                _logger.LogWarning("MODEL STATE KHÔNG HỢP LỆ:");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    _logger.LogWarning($"- {error.ErrorMessage}");
                }
            }

            // Nếu có lỗi, trả về view với thông tin job
            return View(job);
        }

        // Lưu việc làm
        [HttpPost]
        [RequireLogin]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveJob(int jobId)
        {
            var userId = AuthorizationHelper.GetUserIdAsInt(HttpContext.Session);
            if (!userId.HasValue)
            {
                return Json(new { success = false, message = "Vui lòng đăng nhập để lưu việc làm" });
            }

            var existingSave = await _context.TinDaLuus
                .FirstOrDefaultAsync(t => t.MaTin == jobId && t.MaNguoiDung == userId.Value);

            if (existingSave != null)
            {
                // Nếu đã lưu rồi thì xóa
                _context.TinDaLuus.Remove(existingSave);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Đã bỏ lưu việc làm", saved = false });
            }
            else
            {
                // Lưu việc làm mới
                var savedJob = new TinDaLuu
                {
                    MaTin = jobId,
                    MaNguoiDung = userId.Value,
                    NgayLuu = DateTime.Now
                };

                _context.TinDaLuus.Add(savedJob);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Đã lưu việc làm thành công", saved = true });
            }
        }

        // Trang việc làm đã lưu
        [RequireLogin]
        public async Task<IActionResult> SavedJobs()
        {
            var userId = AuthorizationHelper.GetUserIdAsInt(HttpContext.Session);
            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            var savedJobs = await _context.TinDaLuus
                .Include(t => t.TinTuyenDung)
                .ThenInclude(t => t.CongTy)
                .Include(t => t.TinTuyenDung)
                .ThenInclude(t => t.DanhMucCongViec)
                .Where(t => t.MaNguoiDung == userId.Value)
                .OrderByDescending(t => t.NgayLuu)
                .ToListAsync();

            return View(savedJobs);
        }

        // Trang đơn ứng tuyển của tôi
        [RequireCandidate]
        public async Task<IActionResult> MyApplications()
        {
            var userId = AuthorizationHelper.GetUserIdAsInt(HttpContext.Session);
            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            var applications = await _context.DonUngTuyens
                .Include(d => d.TinTuyenDung)
                .ThenInclude(t => t.CongTy)
                .Include(d => d.TinTuyenDung)
                .ThenInclude(t => t.DanhMucCongViec)
                .Where(d => d.MaNguoiDung == userId.Value)
                .OrderByDescending(d => d.NgayUngTuyen)
                .ToListAsync();

            return View(applications);
        }

        // Trang đăng tin tuyển dụng
        [RequireEmployer]
        public async Task<IActionResult> PostJob()
        {
            ViewBag.DanhMucCongViecs = await _context.DanhMucCongViecs.ToListAsync();
            ViewBag.CapBacCongViecs = await _context.CapBacCongViecs.ToListAsync();
            ViewBag.LoaiCongViecs = await _context.LoaiCongViecs.ToListAsync();
            ViewBag.KyNangs = await _context.KyNangs.ToListAsync();

            return View();
        }

        [HttpPost]
        [RequireEmployer]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostJob([FromForm] TinTuyenDung job, List<int> skillIds)
        {
            _logger.LogInformation("=== BẮT ĐẦU ĐĂNG TIN TUYỂN DỤNG ===");
            
            var userId = AuthorizationHelper.GetUserIdAsInt(HttpContext.Session);
            _logger.LogInformation($"User ID: {userId}");
            
            if (!userId.HasValue)
            {
                _logger.LogWarning("User chưa đăng nhập");
                return RedirectToAction("Login", "Account");
            }

            // Tạo DuongDan ngay từ đầu, trước khi validation
            if (string.IsNullOrEmpty(job.DuongDan) && !string.IsNullOrEmpty(job.TieuDe))
            {
                // Tạo DuongDan từ tiêu đề
                var duongDan = job.TieuDe.ToLower()
                    .Replace(" ", "-")
                    .Replace("--", "-")
                    .Replace("---", "-")
                    .Replace("à", "a").Replace("á", "a").Replace("ả", "a").Replace("ã", "a").Replace("ạ", "a")
                    .Replace("ă", "a").Replace("ằ", "a").Replace("ắ", "a").Replace("ẳ", "a").Replace("ẵ", "a").Replace("ặ", "a")
                    .Replace("â", "a").Replace("ầ", "a").Replace("ấ", "a").Replace("ẩ", "a").Replace("ẫ", "a").Replace("ậ", "a")
                    .Replace("è", "e").Replace("é", "e").Replace("ẻ", "e").Replace("ẽ", "e").Replace("ẹ", "e")
                    .Replace("ê", "e").Replace("ề", "e").Replace("ế", "e").Replace("ể", "e").Replace("ễ", "e").Replace("ệ", "e")
                    .Replace("ì", "i").Replace("í", "i").Replace("ỉ", "i").Replace("ĩ", "i").Replace("ị", "i")
                    .Replace("ò", "o").Replace("ó", "o").Replace("ỏ", "o").Replace("õ", "o").Replace("ọ", "o")
                    .Replace("ô", "o").Replace("ồ", "o").Replace("ố", "o").Replace("ổ", "o").Replace("ỗ", "o").Replace("ộ", "o")
                    .Replace("ơ", "o").Replace("ờ", "o").Replace("ớ", "o").Replace("ở", "o").Replace("ỡ", "o").Replace("ợ", "o")
                    .Replace("ù", "u").Replace("ú", "u").Replace("ủ", "u").Replace("ũ", "u").Replace("ụ", "u")
                    .Replace("ư", "u").Replace("ừ", "u").Replace("ứ", "u").Replace("ử", "u").Replace("ữ", "u").Replace("ự", "u")
                    .Replace("ỳ", "y").Replace("ý", "y").Replace("ỷ", "y").Replace("ỹ", "y").Replace("ỵ", "y")
                    .Replace("đ", "d")
                    .Replace(".", "").Replace(",", "").Replace(";", "").Replace(":", "")
                    .Replace("!", "").Replace("?", "").Replace("(", "").Replace(")", "")
                    .Replace("[", "").Replace("]", "").Replace("{", "").Replace("}", "")
                    .Replace("\"", "").Replace("'", "").Replace("`", "")
                    .Replace("&", "va").Replace("@", "at").Replace("#", "").Replace("$", "")
                    .Replace("%", "").Replace("^", "").Replace("*", "").Replace("+", "")
                    .Replace("=", "").Replace("|", "").Replace("\\", "").Replace("/", "")
                    .Replace("<", "").Replace(">", "").Replace("~", "");
                
                // Loại bỏ các ký tự không hợp lệ và giới hạn độ dài
                duongDan = System.Text.RegularExpressions.Regex.Replace(duongDan, @"[^a-z0-9\-]", "");
                duongDan = System.Text.RegularExpressions.Regex.Replace(duongDan, @"-+", "-");
                duongDan = duongDan.Trim('-');
                
                // Giới hạn độ dài tối đa 255 ký tự
                if (duongDan.Length > 255)
                {
                    duongDan = duongDan.Substring(0, 255);
                    duongDan = duongDan.TrimEnd('-');
                }
                
                // Nếu vẫn rỗng, tạo một giá trị mặc định
                if (string.IsNullOrEmpty(duongDan))
                {
                    duongDan = $"tin-tuyen-dung-{DateTime.Now:yyyyMMdd-HHmmss}";
                }
                
                // Kiểm tra xem DuongDan đã tồn tại chưa
                var existingJob = await _context.TinTuyenDungs
                    .FirstOrDefaultAsync(t => t.DuongDan == duongDan);
                
                if (existingJob != null)
                {
                    // Nếu đã tồn tại, thêm timestamp để tạo unique
                    duongDan = $"{duongDan}-{DateTime.Now:yyyyMMdd-HHmmss}";
                    _logger.LogInformation($"DuongDan đã tồn tại, tạo mới: {duongDan}");
                }
                
                job.DuongDan = duongDan;
                _logger.LogInformation($"Đã tạo DuongDan: {job.DuongDan}");
            }
            else if (string.IsNullOrEmpty(job.DuongDan))
            {
                // Nếu không có tiêu đề, tạo DuongDan mặc định
                job.DuongDan = $"tin-tuyen-dung-{DateTime.Now:yyyyMMdd-HHmmss}";
                _logger.LogInformation($"Đã tạo DuongDan mặc định: {job.DuongDan}");
            }

            _logger.LogInformation($"ModelState.IsValid: {ModelState.IsValid}");
            _logger.LogInformation($"DuongDan trước validation: {job.DuongDan}");
            
            // Clear DuongDan validation errors since we've set the value
            if (ModelState.ContainsKey("DuongDan"))
            {
                ModelState.Remove("DuongDan");
            }
            
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("MODEL STATE KHÔNG HỢP LỆ:");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    _logger.LogWarning($"- {error.ErrorMessage}");
                }
                
                ViewBag.DanhMucCongViecs = await _context.DanhMucCongViecs.ToListAsync();
                ViewBag.CapBacCongViecs = await _context.CapBacCongViecs.ToListAsync();
                ViewBag.LoaiCongViecs = await _context.LoaiCongViecs.ToListAsync();
                ViewBag.KyNangs = await _context.KyNangs.ToListAsync();

                return View(job);
            }

            try
            {
                _logger.LogInformation("Bắt đầu lưu tin tuyển dụng...");
                
                // Lấy thông tin công ty của người dùng
                var userCompany = await _context.CongTys
                    .FirstOrDefaultAsync(c => c.MaNguoiDung == userId.Value);
                
                _logger.LogInformation($"Công ty của user: {userCompany?.TenCongTy ?? "Không có công ty"}");

                // Thiết lập thông tin cơ bản cho tin tuyển dụng
                job.MaNguoiDung = userId.Value;
                job.MaCongTy = userCompany?.MaCongTy;
                job.NgayTao = DateTime.Now;
                job.NgayCapNhat = DateTime.Now;
                job.TrangThai = true;
                job.LuotXem = 0;

                if (string.IsNullOrEmpty(job.QuocGia))
                {
                    job.QuocGia = "Việt Nam";
                }

                if (string.IsNullOrEmpty(job.DonViTien))
                {
                    job.DonViTien = "VND";
                }

                _logger.LogInformation($"Thông tin tin tuyển dụng trước khi lưu:");
                _logger.LogInformation($"- Tiêu đề: {job.TieuDe}");
                _logger.LogInformation($"- Đường dẫn: {job.DuongDan}");
                _logger.LogInformation($"- Danh mục: {job.MaDanhMuc}");
                _logger.LogInformation($"- Địa điểm: {job.DiaDiem}");
                _logger.LogInformation($"- Trạng thái: {job.TrangThai}");
                _logger.LogInformation($"- Ngày tạo: {job.NgayTao}");

                _context.TinTuyenDungs.Add(job);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Đã lưu tin tuyển dụng thành công! ID: {job.MaTin}");

                // Log thông tin tin tuyển dụng vừa tạo
                Console.WriteLine($"=== TIN TUYỂN DỤNG MỚI ĐƯỢC TẠO ===");
                Console.WriteLine($"ID: {job.MaTin}");
                Console.WriteLine($"Tiêu đề: {job.TieuDe}");
                Console.WriteLine($"Công ty: {userCompany?.TenCongTy ?? "Không có công ty"}");
                Console.WriteLine($"Trạng thái: {job.TrangThai}");
                Console.WriteLine($"Ngày tạo: {job.NgayTao}");
                Console.WriteLine($"Nổi bật: {job.NoiBat}");
                Console.WriteLine($"=====================================");

                // Thêm kỹ năng yêu cầu
                if (skillIds != null && skillIds.Any())
                {
                    _logger.LogInformation($"Thêm {skillIds.Count} kỹ năng yêu cầu");
                    foreach (var skillId in skillIds)
                    {
                        var kyNangTuyenDung = new KyNangTuyenDung
                        {
                            MaTin = job.MaTin,
                            MaKyNang = skillId
                        };
                        _context.KyNangTuyenDungs.Add(kyNangTuyenDung);
                    }
                    await _context.SaveChangesAsync();
                }

                TempData["Success"] = $"Đăng tin tuyển dụng thành công! Tin tuyển dụng '{job.TieuDe}' đã được hiển thị trên trang chủ. ID: {job.MaTin}";
                _logger.LogInformation("=== ĐĂNG TIN THÀNH CÔNG ===");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError($"LỖI KHI ĐĂNG TIN: {ex.Message}");
                _logger.LogError($"Stack trace: {ex.StackTrace}");
                
                // Log inner exception details
                if (ex.InnerException != null)
                {
                    _logger.LogError($"INNER EXCEPTION: {ex.InnerException.Message}");
                    _logger.LogError($"INNER EXCEPTION STACK TRACE: {ex.InnerException.StackTrace}");
                }
                
                // Log model state for debugging
                _logger.LogError("MODEL STATE ERRORS:");
                foreach (var modelState in ModelState)
                {
                    foreach (var error in modelState.Value.Errors)
                    {
                        _logger.LogError($"- {modelState.Key}: {error.ErrorMessage}");
                    }
                }
                
                // Log job object properties for debugging
                _logger.LogError("JOB OBJECT PROPERTIES:");
                _logger.LogError($"- MaTin: {job.MaTin}");
                _logger.LogError($"- TieuDe: {job.TieuDe}");
                _logger.LogError($"- DuongDan: {job.DuongDan}");
                _logger.LogError($"- MaNguoiDung: {job.MaNguoiDung}");
                _logger.LogError($"- MaCongTy: {job.MaCongTy}");
                _logger.LogError($"- MaDanhMuc: {job.MaDanhMuc}");
                _logger.LogError($"- MaCapBac: {job.MaCapBac}");
                _logger.LogError($"- MaLoai: {job.MaLoai}");
                _logger.LogError($"- DiaDiem: {job.DiaDiem}");
                _logger.LogError($"- QuocGia: {job.QuocGia}");
                _logger.LogError($"- NgayTao: {job.NgayTao}");
                _logger.LogError($"- TrangThai: {job.TrangThai}");
                
                Console.WriteLine($"LỖI KHI ĐĂNG TIN: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"INNER EXCEPTION: {ex.InnerException.Message}");
                }
                
                ModelState.AddModelError("", $"Lỗi khi lưu tin tuyển dụng: {ex.Message}");
                if (ex.InnerException != null)
                {
                    ModelState.AddModelError("", $"Chi tiết lỗi: {ex.InnerException.Message}");
                }
            }

            ViewBag.DanhMucCongViecs = await _context.DanhMucCongViecs.ToListAsync();
            ViewBag.CapBacCongViecs = await _context.CapBacCongViecs.ToListAsync();
            ViewBag.LoaiCongViecs = await _context.LoaiCongViecs.ToListAsync();
            ViewBag.KyNangs = await _context.KyNangs.ToListAsync();

            return View(job);
        }

        // Hủy đơn ứng tuyển
        [HttpPost]
        [RequireCandidate]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelApplication(int applicationId)
        {
            _logger.LogInformation($"=== HỦY ĐƠN ỨNG TUYỂN ===");
            _logger.LogInformation($"Application ID: {applicationId}");
            
            var userId = AuthorizationHelper.GetUserIdAsInt(HttpContext.Session);
            if (!userId.HasValue)
            {
                _logger.LogWarning("User chưa đăng nhập");
                return Json(new { success = false, message = "Vui lòng đăng nhập để thực hiện thao tác này" });
            }

            try
            {
                var application = await _context.DonUngTuyens
                    .Include(d => d.TinTuyenDung)
                    .FirstOrDefaultAsync(d => d.MaDon == applicationId && d.MaNguoiDung == userId.Value);

                if (application == null)
                {
                    _logger.LogWarning($"Không tìm thấy đơn ứng tuyển {applicationId} của user {userId.Value}");
                    return Json(new { success = false, message = "Không tìm thấy đơn ứng tuyển" });
                }

                // Kiểm tra trạng thái đơn ứng tuyển
                if (application.TrangThai != "Chờ xử lý")
                {
                    _logger.LogWarning($"Không thể hủy đơn ứng tuyển {applicationId} với trạng thái {application.TrangThai}");
                    return Json(new { success = false, message = "Chỉ có thể hủy đơn ứng tuyển đang chờ xử lý" });
                }

                _logger.LogInformation($"Hủy đơn ứng tuyển: {application.TinTuyenDung?.TieuDe ?? "Không xác định"}");

                _context.DonUngTuyens.Remove(application);
                await _context.SaveChangesAsync();

                _logger.LogInformation("=== HỦY ĐƠN ỨNG TUYỂN THÀNH CÔNG ===");
                return Json(new { success = true, message = "Đã hủy đơn ứng tuyển thành công!" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"LỖI KHI HỦY ĐƠN ỨNG TUYỂN: {ex.Message}");
                _logger.LogError($"Stack trace: {ex.StackTrace}");
                
                if (ex.InnerException != null)
                {
                    _logger.LogError($"INNER EXCEPTION: {ex.InnerException.Message}");
                }
                
                return Json(new { success = false, message = "Có lỗi xảy ra khi hủy đơn ứng tuyển" });
            }
        }

        // API endpoint để lấy danh sách việc làm mới nhất (cho AJAX refresh)
        [HttpGet]
        public async Task<IActionResult> GetLatestJobs()
        {
            try
            {
                var jobs = await _context.TinTuyenDungs
                    .Include(t => t.CongTy)
                    .Include(t => t.DanhMucCongViec)
                    .Include(t => t.CapBacCongViec)
                    .Include(t => t.LoaiCongViec)
                    .Where(t => t.TrangThai)
                    .OrderByDescending(t => t.NgayTao)
                    .Take(50)
                    .Select(t => new
                    {
                        maTin = t.MaTin,
                        tieuDe = t.TieuDe,
                        companyName = t.CongTy != null ? t.CongTy.TenCongTy : "Công ty không xác định",
                        diaDiem = t.DiaDiem,
                        levelName = t.CapBacCongViec != null ? t.CapBacCongViec.TenCapBac : "Không xác định",
                        categoryName = t.DanhMucCongViec != null ? t.DanhMucCongViec.TenDanhMuc : "Khác",
                        typeName = t.LoaiCongViec != null ? t.LoaiCongViec.TenLoai : null,
                        luongTu = t.LuongTu,
                        luongDen = t.LuongDen,
                        donViTien = t.DonViTien,
                        hienThiLuong = t.HienThiLuong,
                        noiBat = t.NoiBat,
                        gapTuyen = t.GapTuyen,
                        luotXem = t.LuotXem,
                        ngayTao = t.NgayTao
                    })
                    .ToListAsync();

                _logger.LogInformation($"API GetLatestJobs: Found {jobs.Count} jobs");
                
                return Json(new { success = true, jobs = jobs });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetLatestJobs: {ex.Message}");
                return Json(new { success = false, message = "Lỗi khi tải danh sách việc làm" });
            }
        }
    }
} 