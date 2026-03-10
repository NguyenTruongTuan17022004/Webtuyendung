using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HeThongTuyenDung.Data;
using HeThongTuyenDung.Models;
using HeThongTuyenDung.Helpers;

namespace HeThongTuyenDung.Controllers
{
    public class DashboardController : Controller
    {
        private readonly TuyenDungDbContext _context;

        public DashboardController(TuyenDungDbContext context)
        {
            _context = context;
        }

        // Dashboard chung - chuyển hướng dựa trên vai trò
        [RequireLogin]
        public IActionResult Index()
        {
            var userRole = AuthorizationHelper.GetUserRole(HttpContext.Session);
            
            switch (userRole)
            {
                case "Ứng viên":
                    return RedirectToAction(nameof(CandidateDashboard));
                case "Doanh nghiệp":
                    return RedirectToAction(nameof(EmployerDashboard));
                case "Admin":
                    return RedirectToAction(nameof(AdminDashboard));
                default:
                    return RedirectToAction("Index", "Job");
            }
        }

        // Dashboard cho ứng viên
        [RequireCandidate]
        public async Task<IActionResult> CandidateDashboard()
        {
            var userId = AuthorizationHelper.GetUserIdAsInt(HttpContext.Session);
            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            // Lấy thống kê cho ứng viên
            var totalApplications = await _context.DonUngTuyens
                .Where(d => d.MaNguoiDung == userId.Value)
                .CountAsync();

            var pendingApplications = await _context.DonUngTuyens
                .Where(d => d.MaNguoiDung == userId.Value && d.TrangThai == "Chờ xử lý")
                .CountAsync();

            var acceptedApplications = await _context.DonUngTuyens
                .Where(d => d.MaNguoiDung == userId.Value && d.TrangThai == "Chấp nhận")
                .CountAsync();

            var rejectedApplications = await _context.DonUngTuyens
                .Where(d => d.MaNguoiDung == userId.Value && d.TrangThai == "Từ chối")
                .CountAsync();

            // Lấy đơn ứng tuyển gần đây
            var recentApplications = await _context.DonUngTuyens
                .Include(d => d.TinTuyenDung)
                .ThenInclude(t => t.CongTy)
                .Include(d => d.TinTuyenDung)
                .ThenInclude(t => t.DanhMucCongViec)
                .Where(d => d.MaNguoiDung == userId.Value)
                .OrderByDescending(d => d.NgayUngTuyen)
                .Take(10)
                .ToListAsync();

            // Tạo HomeViewModel
            var viewModel = new HomeViewModel
            {
                TotalApplications = totalApplications,
                PendingApplications = pendingApplications,
                AcceptedApplications = acceptedApplications,
                RejectedApplications = rejectedApplications,
                RecentApplications = recentApplications
            };

            return View(viewModel);
        }

        // Dashboard cho doanh nghiệp
        [RequireRole("Doanh nghiệp")]
        public async Task<IActionResult> EmployerDashboard()
        {
            var userId = AuthorizationHelper.GetUserIdAsInt(HttpContext.Session);
            
            // Lấy id công ty của user này
            var company = await _context.CongTys.FirstOrDefaultAsync(c => c.MaNguoiDung == userId);
            
            // Thống kê cho view
            var stats = new
            {
                TotalJobs = company != null ? await _context.TinTuyenDungs.CountAsync(t => t.MaCongTy == company.MaCongTy) : 0,
                ActiveJobs = company != null ? await _context.TinTuyenDungs.CountAsync(t => t.MaCongTy == company.MaCongTy && t.TrangThai == true && t.HanNopHoSo >= DateTime.Now) : 0,
                TotalApplications = company != null ? await _context.DonUngTuyens
                    .Include(d => d.TinTuyenDung)
                    .CountAsync(d => d.TinTuyenDung.MaCongTy == company.MaCongTy) : 0,
                NewApplications = company != null ? await _context.DonUngTuyens
                    .Include(d => d.TinTuyenDung)
                    .CountAsync(d => d.TinTuyenDung.MaCongTy == company.MaCongTy && d.TrangThai == "Chờ duyệt") : 0
            };

            // Lấy danh sách việc làm gần đây
            var recentJobs = company != null ? await _context.TinTuyenDungs
                .Where(t => t.MaCongTy == company.MaCongTy)
                .OrderByDescending(t => t.NgayTao)
                .Take(5)
                .ToListAsync() : new List<TinTuyenDung>();

            // Lấy danh sách ứng viên mới nhất
            var recentApplications = company != null ? await _context.DonUngTuyens
                .Include(d => d.NguoiDung)
                .Include(d => d.TinTuyenDung)
                .Where(d => d.TinTuyenDung.MaCongTy == company.MaCongTy)
                .OrderByDescending(d => d.NgayUngTuyen)
                .Take(5)
                .ToListAsync() : new List<DonUngTuyen>();

            ViewBag.Stats = stats;
            ViewBag.RecentJobs = recentJobs;
            ViewBag.RecentApplications = recentApplications;
            
            return View();
        }

        // GET: Dashboard/CompanyProfile
        [RequireRole("Doanh nghiệp")]
        public async Task<IActionResult> CompanyProfile()
        {
            var userId = AuthorizationHelper.GetUserIdAsInt(HttpContext.Session);
            var company = await _context.CongTys.FirstOrDefaultAsync(c => c.MaNguoiDung == userId);

            // Nếu công ty chưa tồn tại, tạo instance rỗng
            if (company == null)
            {
                company = new CongTy
                {
                    MaNguoiDung = userId,
                    TenCongTy = HttpContext.Session.GetString("UserFullName") ?? ""
                };
            }

            return View(company);
        }

        // POST: Dashboard/CompanyProfile
        [HttpPost]
        [RequireRole("Doanh nghiệp")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CompanyProfile(CongTy model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = AuthorizationHelper.GetUserIdAsInt(HttpContext.Session);
            var company = await _context.CongTys.FirstOrDefaultAsync(c => c.MaNguoiDung == userId);

            if (company == null)
            {
                // Thêm mới
                model.MaNguoiDung = userId;
                model.NgayTao = DateTime.Now;
                model.NgayCapNhat = DateTime.Now;
                _context.CongTys.Add(model);
            }
            else
            {
                // Cập nhật
                company.TenCongTy = model.TenCongTy;
                company.MoTa = model.MoTa;
                company.MoTaNgan = model.MoTaNgan;
                company.DiaChi = model.DiaChi;
                company.ThanhPho = model.ThanhPho;
                company.QuocGia = model.QuocGia ?? "Việt Nam";
                company.DienThoai = model.DienThoai;
                company.Email = model.Email;
                company.Website = model.Website;
                company.QuyMo = model.QuyMo;
                company.LinhVuc = model.LinhVuc;
                company.MaSoThue = model.MaSoThue;
                company.NgayCapNhat = DateTime.Now;

                _context.CongTys.Update(company);
            }

            await _context.SaveChangesAsync();
            TempData["Success"] = "Cập nhật hồ sơ công ty thành công!";

            return RedirectToAction(nameof(CompanyProfile));
        }

        // Dashboard cho admin
        [RequireAdmin]
        public async Task<IActionResult> AdminDashboard()
        {
            // Thống kê tổng quan hệ thống
            var totalUsers = await _context.NguoiDungs.CountAsync();
            var totalJobs = await _context.TinTuyenDungs.CountAsync();
            var totalApplications = await _context.DonUngTuyens.CountAsync();
            var totalCompanies = await _context.CongTys.CountAsync();

            // Thống kê theo vai trò
            var candidates = await _context.NguoiDungs
                .Include(u => u.VaiTro)
                .Where(u => u.VaiTro.TenVaiTro == "Ứng viên")
                .CountAsync();

            var employers = await _context.NguoiDungs
                .Include(u => u.VaiTro)
                .Where(u => u.VaiTro.TenVaiTro == "Doanh nghiệp")
                .CountAsync();

            // Tin tuyển dụng gần đây
            var recentJobs = await _context.TinTuyenDungs
                .Include(t => t.CongTy)
                .Include(t => t.NguoiDung)
                .OrderByDescending(t => t.NgayTao)
                .Take(10)
                .ToListAsync();

            // Đơn ứng tuyển gần đây
            var recentApplications = await _context.DonUngTuyens
                .Include(d => d.TinTuyenDung)
                .Include(d => d.NguoiDung)
                .OrderByDescending(d => d.NgayUngTuyen)
                .Take(10)
                .ToListAsync();

            ViewBag.TotalUsers = totalUsers;
            ViewBag.TotalJobs = totalJobs;
            ViewBag.TotalApplications = totalApplications;
            ViewBag.TotalCompanies = totalCompanies;
            ViewBag.Candidates = candidates;
            ViewBag.Employers = employers;
            ViewBag.RecentJobs = recentJobs;
            ViewBag.RecentApplications = recentApplications;

            return View();
        }

        // Quản lý ứng viên cho doanh nghiệp
        [RequireEmployer]
        public async Task<IActionResult> ManageApplications(int? jobId = null, string status = null, DateTime? dateFrom = null)
        {
            var userId = AuthorizationHelper.GetUserIdAsInt(HttpContext.Session);
            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            var query = _context.DonUngTuyens
                .Include(d => d.TinTuyenDung).ThenInclude(t => t.KyNangTuyenDungs)
                .Include(d => d.NguoiDung).ThenInclude(u => u.HoSoUngVien).ThenInclude(h => h.KyNangUngViens)
                .Where(d => d.TinTuyenDung.MaNguoiDung == userId.Value);

            if (jobId.HasValue)
            {
                query = query.Where(d => d.MaTin == jobId.Value);
            }

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(d => d.TrangThai == status);
            }

            if (dateFrom.HasValue)
            {
                query = query.Where(d => d.NgayUngTuyen >= dateFrom.Value);
            }

            var applications = await query.ToListAsync();

            // Tính Match Score
            var matchScores = new Dictionary<int, int>();
            foreach (var app in applications)
            {
                var candidateProfile = app.NguoiDung?.HoSoUngVien;
                if (candidateProfile != null && app.TinTuyenDung != null)
                {
                    matchScores[app.MaDon] = HeThongTuyenDung.Helpers.MatchCalculator.CalculateMatchScore(app.TinTuyenDung, candidateProfile);
                }
                else
                {
                    matchScores[app.MaDon] = 0;
                }
            }

            // Sắp xếp: Ưu tiên Match Score cao -> ngày ứng tuyển mới nhất
            applications = applications.OrderByDescending(d => matchScores.ContainsKey(d.MaDon) ? matchScores[d.MaDon] : 0)
                                       .ThenByDescending(d => d.NgayUngTuyen)
                                       .ToList();

            // Lấy danh sách tin tuyển dụng của doanh nghiệp
            var jobs = await _context.TinTuyenDungs
                .Where(t => t.MaNguoiDung == userId.Value)
                .ToListAsync();

            // Thống kê
            var totalApplications = await _context.DonUngTuyens
                .Include(d => d.TinTuyenDung)
                .Where(d => d.TinTuyenDung.MaNguoiDung == userId.Value)
                .CountAsync();

            var pendingApplications = await _context.DonUngTuyens
                .Include(d => d.TinTuyenDung)
                .Where(d => d.TinTuyenDung.MaNguoiDung == userId.Value && d.TrangThai == "Chờ xử lý")
                .CountAsync();

            var unreadApplications = await _context.DonUngTuyens
                .Include(d => d.TinTuyenDung)
                .Where(d => d.TinTuyenDung.MaNguoiDung == userId.Value && !d.DaXem)
                .CountAsync();

            ViewBag.Jobs = jobs;
            ViewBag.SelectedJobId = jobId;
            ViewBag.SelectedStatus = status;
            ViewBag.SelectedDateFrom = dateFrom?.ToString("yyyy-MM-dd");
            ViewBag.TotalApplications = totalApplications;
            ViewBag.PendingApplications = pendingApplications;
            ViewBag.UnreadApplications = unreadApplications;
            ViewBag.MatchScores = matchScores; // Truyền Dictionary qua ViewBag

            return View(applications);
        }

        // Cập nhật trạng thái đơn ứng tuyển
        [HttpPost]
        [RequireEmployer]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateApplicationStatus(int applicationId, string status)
        {
            var userId = AuthorizationHelper.GetUserIdAsInt(HttpContext.Session);
            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            var application = await _context.DonUngTuyens
                .Include(d => d.TinTuyenDung)
                .FirstOrDefaultAsync(d => d.MaDon == applicationId && d.TinTuyenDung.MaNguoiDung == userId.Value);

            if (application == null)
            {
                return NotFound();
            }

            application.TrangThai = status;
            application.NgayCapNhat = DateTime.Now;

            await _context.SaveChangesAsync();

            TempData["Success"] = $"Đã cập nhật trạng thái đơn ứng tuyển thành '{status}'";
            return RedirectToAction(nameof(ManageApplications));
        }

        // Lấy chi tiết đơn ứng tuyển
        [RequireEmployer]
        public async Task<IActionResult> GetApplicationDetails(int applicationId)
        {
            var userId = AuthorizationHelper.GetUserIdAsInt(HttpContext.Session);
            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            var application = await _context.DonUngTuyens
                .Include(d => d.TinTuyenDung)
                .ThenInclude(t => t.CongTy)
                .Include(d => d.TinTuyenDung)
                .ThenInclude(t => t.DanhMucCongViec)
                .Include(d => d.NguoiDung)
                .ThenInclude(u => u.HoSoUngVien)
                .ThenInclude(h => h.KinhNghiems)
                .Include(d => d.NguoiDung)
                .ThenInclude(u => u.HoSoUngVien)
                .ThenInclude(h => h.HocVans)
                .Include(d => d.NguoiDung)
                .ThenInclude(u => u.HoSoUngVien)
                .ThenInclude(h => h.KyNangUngViens)
                .ThenInclude(k => k.KyNang)
                .FirstOrDefaultAsync(d => d.MaDon == applicationId && d.TinTuyenDung.MaNguoiDung == userId.Value);

            if (application == null)
            {
                return NotFound();
            }

            // Đánh dấu đã xem
            if (!application.DaXem)
            {
                application.DaXem = true;
                await _context.SaveChangesAsync();
            }

            return PartialView("_ApplicationDetails", application);
        }

        // Đánh dấu đã xem đơn ứng tuyển
        [HttpPost]
        [RequireEmployer]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkAsRead(int applicationId)
        {
            var userId = AuthorizationHelper.GetUserIdAsInt(HttpContext.Session);
            if (!userId.HasValue)
            {
                return Json(new { success = false, message = "Vui lòng đăng nhập" });
            }

            var application = await _context.DonUngTuyens
                .Include(d => d.TinTuyenDung)
                .FirstOrDefaultAsync(d => d.MaDon == applicationId && d.TinTuyenDung.MaNguoiDung == userId.Value);

            if (application == null)
            {
                return Json(new { success = false, message = "Không tìm thấy đơn ứng tuyển" });
            }

            application.DaXem = true;
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Đã đánh dấu đã xem" });
        }

        // Đánh dấu tất cả đã xem
        [HttpPost]
        [RequireEmployer]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkAllAsRead()
        {
            var userId = AuthorizationHelper.GetUserIdAsInt(HttpContext.Session);
            if (!userId.HasValue)
            {
                return Json(new { success = false, message = "Vui lòng đăng nhập" });
            }

            var unreadApplications = await _context.DonUngTuyens
                .Include(d => d.TinTuyenDung)
                .Where(d => d.TinTuyenDung.MaNguoiDung == userId.Value && !d.DaXem)
                .ToListAsync();

            foreach (var application in unreadApplications)
            {
                application.DaXem = true;
            }

            await _context.SaveChangesAsync();

            return Json(new { success = true, message = $"Đã đánh dấu {unreadApplications.Count} đơn ứng tuyển đã xem" });
        }

        // Cập nhật trạng thái hàng loạt
        [HttpPost]
        [RequireEmployer]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BulkUpdateStatus(string applicationIds, string status)
        {
            var userId = AuthorizationHelper.GetUserIdAsInt(HttpContext.Session);
            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            if (string.IsNullOrEmpty(applicationIds))
            {
                TempData["Error"] = "Vui lòng chọn ít nhất một đơn ứng tuyển";
                return RedirectToAction(nameof(ManageApplications));
            }

            var applicationIdArray = applicationIds.Split(',').Select(int.Parse).ToArray();

            var applications = await _context.DonUngTuyens
                .Include(d => d.TinTuyenDung)
                .Where(d => applicationIdArray.Contains(d.MaDon) && d.TinTuyenDung.MaNguoiDung == userId.Value)
                .ToListAsync();

            foreach (var application in applications)
            {
                application.TrangThai = status;
                application.NgayCapNhat = DateTime.Now;
            }

            await _context.SaveChangesAsync();

            TempData["Success"] = $"Đã cập nhật trạng thái {applications.Count} đơn ứng tuyển thành '{status}'";
            return RedirectToAction(nameof(ManageApplications));
        }

        // Xuất danh sách ứng viên
        [RequireEmployer]
        public async Task<IActionResult> ExportApplications(int? jobId = null, string status = null)
        {
            var userId = AuthorizationHelper.GetUserIdAsInt(HttpContext.Session);
            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            var query = _context.DonUngTuyens
                .Include(d => d.TinTuyenDung)
                .Include(d => d.NguoiDung)
                .Where(d => d.TinTuyenDung.MaNguoiDung == userId.Value);

            if (jobId.HasValue)
            {
                query = query.Where(d => d.MaTin == jobId.Value);
            }

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(d => d.TrangThai == status);
            }

            var applications = await query
                .OrderByDescending(d => d.NgayUngTuyen)
                .ToListAsync();

            // Tạo nội dung CSV
            var csvContent = "Tên ứng viên,Email,Vị trí ứng tuyển,Công ty,Ngày ứng tuyển,Trạng thái\n";
            
            foreach (var app in applications)
            {
                csvContent += $"\"{app.NguoiDung.HoTen}\",\"{app.NguoiDung.Email}\",\"{app.TinTuyenDung.TieuDe}\",\"{app.TinTuyenDung.CongTy?.TenCongTy ?? "Không xác định"}\",\"{app.NgayUngTuyen:dd/MM/yyyy}\",\"{app.TrangThai}\"\n";
            }

            var fileName = $"applications_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
            
            return File(System.Text.Encoding.UTF8.GetBytes(csvContent), "text/csv", fileName);
        }

        // Gửi tin nhắn cho ứng viên
        [HttpPost]
        [RequireEmployer]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendMessage(int applicationId, string? message)
        {
            var userId = AuthorizationHelper.GetUserIdAsInt(HttpContext.Session);
            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            var application = await _context.DonUngTuyens
                .Include(d => d.TinTuyenDung)
                .Include(d => d.NguoiDung)
                .FirstOrDefaultAsync(d => d.MaDon == applicationId && d.TinTuyenDung.MaNguoiDung == userId.Value);

            if (application == null)
            {
                return NotFound();
            }

            // TODO: Implement message sending functionality
            // For now, just return success message
            TempData["Success"] = $"Đã gửi tin nhắn cho {application.NguoiDung.HoTen}";
            return RedirectToAction(nameof(ManageApplications));
        }

        // Lên lịch phỏng vấn
        [HttpPost]
        [RequireEmployer]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ScheduleInterview(int applicationId, DateTime interviewDate, string? location, string? notes)
        {
            var userId = AuthorizationHelper.GetUserIdAsInt(HttpContext.Session);
            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            var application = await _context.DonUngTuyens
                .Include(d => d.TinTuyenDung)
                .Include(d => d.NguoiDung)
                .FirstOrDefaultAsync(d => d.MaDon == applicationId && d.TinTuyenDung.MaNguoiDung == userId.Value);

            if (application == null)
            {
                return NotFound();
            }

            // Cập nhật trạng thái thành "Phỏng vấn"
            application.TrangThai = "Phỏng vấn";
            application.NgayCapNhat = DateTime.Now;

            // TODO: Create interview record in database
            // For now, just update the application status

            await _context.SaveChangesAsync();

            TempData["Success"] = $"Đã lên lịch phỏng vấn cho {application.NguoiDung.HoTen} vào {interviewDate:dd/MM/yyyy HH:mm}";
            return RedirectToAction(nameof(ManageApplications));
        }
    }
} 