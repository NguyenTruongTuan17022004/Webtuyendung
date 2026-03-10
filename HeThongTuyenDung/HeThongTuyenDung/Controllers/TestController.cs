using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HeThongTuyenDung.Data;
using HeThongTuyenDung.Models;
using HeThongTuyenDung.Helpers;

namespace HeThongTuyenDung.Controllers
{
    public class TestController : Controller
    {
        private readonly TuyenDungDbContext _context;

        public TestController(TuyenDungDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                // Test connection by getting count of VaiTro
                var vaiTroCount = await _context.VaiTros.CountAsync();
                var nguoiDungCount = await _context.NguoiDungs.CountAsync();
                var congTyCount = await _context.CongTys.CountAsync();
                var tinTuyenDungCount = await _context.TinTuyenDungs.CountAsync();

                ViewBag.VaiTroCount = vaiTroCount;
                ViewBag.NguoiDungCount = nguoiDungCount;
                ViewBag.CongTyCount = congTyCount;
                ViewBag.TinTuyenDungCount = tinTuyenDungCount;
                ViewBag.ConnectionStatus = "Kết nối thành công!";

                return View();
            }
            catch (Exception ex)
            {
                ViewBag.ConnectionStatus = $"Lỗi kết nối: {ex.Message}";
                return View();
            }
        }

        // Action để tạo tin tuyển dụng mẫu
        public async Task<IActionResult> CreateSampleJob()
        {
            try
            {
                // Kiểm tra xem có danh mục công việc nào không
                var danhMuc = await _context.DanhMucCongViecs.FirstOrDefaultAsync();
                if (danhMuc == null)
                {
                    // Tạo danh mục mẫu
                    danhMuc = new DanhMucCongViec { TenDanhMuc = "Công nghệ thông tin" };
                    _context.DanhMucCongViecs.Add(danhMuc);
                    await _context.SaveChangesAsync();
                }

                // Kiểm tra xem có cấp bậc công việc nào không
                var capBac = await _context.CapBacCongViecs.FirstOrDefaultAsync();
                if (capBac == null)
                {
                    // Tạo cấp bậc mẫu
                    capBac = new CapBacCongViec { TenCapBac = "Nhân viên" };
                    _context.CapBacCongViecs.Add(capBac);
                    await _context.SaveChangesAsync();
                }

                // Kiểm tra xem có loại công việc nào không
                var loaiCongViec = await _context.LoaiCongViecs.FirstOrDefaultAsync();
                if (loaiCongViec == null)
                {
                    // Tạo loại công việc mẫu
                    loaiCongViec = new LoaiCongViec { TenLoai = "Toàn thời gian" };
                    _context.LoaiCongViecs.Add(loaiCongViec);
                    await _context.SaveChangesAsync();
                }

                // Tạo hoặc lấy công ty mẫu
                var congTy = await _context.CongTys.FirstOrDefaultAsync();
                if (congTy == null)
                {
                    // Tạo công ty mẫu
                    congTy = new CongTy
                    {
                        TenCongTy = "Công ty Công nghệ ABC",
                        MaSoThue = "0123456789",
                        DiaChi = "123 Đường ABC, Quận 1, TP.HCM",
                        DienThoai = "028-1234-5678",
                        Email = "info@abc.com",
                        Website = "www.abc.com",
                        MoTa = "Công ty công nghệ hàng đầu Việt Nam",
                        TrangThai = true,
                        NgayTao = DateTime.Now,
                        MaNguoiDung = 1 // Giả sử có user ID = 1
                    };
                    _context.CongTys.Add(congTy);
                    await _context.SaveChangesAsync();
                }

                // Tạo tin tuyển dụng mẫu
                var sampleJob = new TinTuyenDung
                {
                    TieuDe = "Lập trình viên .NET - Tin tuyển dụng mẫu",
                    DuongDan = "lap-trinh-vien-net",
                    MaDanhMuc = danhMuc.MaDanhMuc,
                    MaCapBac = capBac.MaCapBac,
                    MaLoai = loaiCongViec.MaLoai,
                    MoTa = "Chúng tôi đang tìm kiếm một lập trình viên .NET có kinh nghiệm để tham gia vào dự án phát triển phần mềm.",
                    YeuCau = "- Kinh nghiệm 2+ năm với .NET\n- Thành thạo C#, ASP.NET Core\n- Biết SQL Server\n- Có khả năng làm việc nhóm",
                    QuyenLoi = "- Lương cạnh tranh\n- Bảo hiểm đầy đủ\n- Môi trường làm việc trẻ trung\n- Cơ hội thăng tiến",
                    LuongTu = 15000000,
                    LuongDen = 25000000,
                    HienThiLuong = true,
                    DonViTien = "VND",
                    SoLuong = 2,
                    DiaDiem = "Hà Nội",
                    ThanhPho = "Hà Nội",
                    DiaChiLamViec = "Tầng 5, Tòa nhà ABC, 123 Đường XYZ, Quận 1",
                    TrangThai = true,
                    NoiBat = true,
                    GapTuyen = false,
                    LuotXem = 0,
                    NgayTao = DateTime.Now,
                    NgayCapNhat = DateTime.Now,
                    MaNguoiDung = 1, // Giả sử có user ID = 1
                    MaCongTy = congTy.MaCongTy // Gán công ty
                };

                _context.TinTuyenDungs.Add(sampleJob);
                await _context.SaveChangesAsync();

                TempData["Success"] = $"Đã tạo tin tuyển dụng mẫu thành công! ID: {sampleJob.MaTin}, Công ty: {congTy.TenCongTy}";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Lỗi: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        // Action để kiểm tra dữ liệu
        public async Task<IActionResult> CheckData()
        {
            var data = new
            {
                TotalJobs = await _context.TinTuyenDungs.CountAsync(),
                ActiveJobs = await _context.TinTuyenDungs.CountAsync(j => j.TrangThai),
                FeaturedJobs = await _context.TinTuyenDungs.CountAsync(j => j.TrangThai && j.NoiBat),
                RecentJobs = await _context.TinTuyenDungs.CountAsync(j => j.TrangThai && j.NgayTao >= DateTime.Now.AddDays(-7)),
                TotalCompanies = await _context.CongTys.CountAsync(),
                TotalCategories = await _context.DanhMucCongViecs.CountAsync(),
                LatestJobs = await _context.TinTuyenDungs
                    .Include(t => t.CongTy)
                    .Include(t => t.DanhMucCongViec)
                    .Where(t => t.TrangThai)
                    .OrderByDescending(t => t.NgayTao)
                    .Take(5)
                    .Select(t => new
                    {
                        t.MaTin,
                        t.TieuDe,
                        t.NgayTao,
                        t.TrangThai,
                        t.NoiBat,
                        CompanyName = t.CongTy != null ? t.CongTy.TenCongTy : "Không có",
                        CategoryName = t.DanhMucCongViec != null ? t.DanhMucCongViec.TenDanhMuc : "Không có"
                    })
                    .ToListAsync()
            };

            return Json(data);
        }

        // Action để xóa dữ liệu cũ và tạo lại
        public async Task<IActionResult> ResetData()
        {
            try
            {
                // Xóa tất cả tin tuyển dụng cũ
                var oldJobs = await _context.TinTuyenDungs.ToListAsync();
                _context.TinTuyenDungs.RemoveRange(oldJobs);
                await _context.SaveChangesAsync();

                // Tạo lại dữ liệu mẫu
                await CreateSampleJob();

                TempData["Success"] = "Đã xóa dữ liệu cũ và tạo lại dữ liệu mẫu thành công!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Lỗi: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        // Action để tạo dữ liệu cơ bản
        public async Task<IActionResult> CreateBasicData()
        {
            try
            {
                // Tạo vai trò cơ bản
                if (!await _context.VaiTros.AnyAsync())
                {
                    var vaiTros = new List<VaiTro>
                    {
                        new VaiTro { TenVaiTro = "Admin", MoTa = "Quản trị viên hệ thống" },
                        new VaiTro { TenVaiTro = "Nhà tuyển dụng", MoTa = "Nhà tuyển dụng" },
                        new VaiTro { TenVaiTro = "Ứng viên", MoTa = "Ứng viên tìm việc" }
                    };
                    _context.VaiTros.AddRange(vaiTros);
                    await _context.SaveChangesAsync();
                }

                // Tạo danh mục công việc cơ bản
                if (!await _context.DanhMucCongViecs.AnyAsync())
                {
                    var danhMucs = new List<DanhMucCongViec>
                    {
                        new DanhMucCongViec { TenDanhMuc = "Công nghệ thông tin" },
                        new DanhMucCongViec { TenDanhMuc = "Kinh doanh" },
                        new DanhMucCongViec { TenDanhMuc = "Marketing" },
                        new DanhMucCongViec { TenDanhMuc = "Tài chính" },
                        new DanhMucCongViec { TenDanhMuc = "Giáo dục" }
                    };
                    _context.DanhMucCongViecs.AddRange(danhMucs);
                    await _context.SaveChangesAsync();
                }

                // Tạo cấp bậc công việc cơ bản
                if (!await _context.CapBacCongViecs.AnyAsync())
                {
                    var capBacs = new List<CapBacCongViec>
                    {
                        new CapBacCongViec { TenCapBac = "Thực tập sinh" },
                        new CapBacCongViec { TenCapBac = "Nhân viên" },
                        new CapBacCongViec { TenCapBac = "Trưởng nhóm" },
                        new CapBacCongViec { TenCapBac = "Quản lý" },
                        new CapBacCongViec { TenCapBac = "Giám đốc" }
                    };
                    _context.CapBacCongViecs.AddRange(capBacs);
                    await _context.SaveChangesAsync();
                }

                // Tạo loại công việc cơ bản
                if (!await _context.LoaiCongViecs.AnyAsync())
                {
                    var loaiCongViecs = new List<LoaiCongViec>
                    {
                        new LoaiCongViec { TenLoai = "Toàn thời gian" },
                        new LoaiCongViec { TenLoai = "Bán thời gian" },
                        new LoaiCongViec { TenLoai = "Thực tập" },
                        new LoaiCongViec { TenLoai = "Làm việc từ xa" }
                    };
                    _context.LoaiCongViecs.AddRange(loaiCongViecs);
                    await _context.SaveChangesAsync();
                }

                // Tạo kỹ năng cơ bản
                if (!await _context.KyNangs.AnyAsync())
                {
                    var kyNangs = new List<KyNang>
                    {
                        new KyNang { TenKyNang = "C#" },
                        new KyNang { TenKyNang = "ASP.NET Core" },
                        new KyNang { TenKyNang = "SQL Server" },
                        new KyNang { TenKyNang = "JavaScript" },
                        new KyNang { TenKyNang = "React" },
                        new KyNang { TenKyNang = "Angular" },
                        new KyNang { TenKyNang = "Python" },
                        new KyNang { TenKyNang = "Java" }
                    };
                    _context.KyNangs.AddRange(kyNangs);
                    await _context.SaveChangesAsync();
                }

                TempData["Success"] = "Đã tạo dữ liệu cơ bản thành công!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Lỗi: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        // Action để test tạo tin tuyển dụng và kiểm tra ngay lập tức
        public async Task<IActionResult> TestJobCreation()
        {
            try
            {
                // Tạo tin tuyển dụng test
                var danhMuc = await _context.DanhMucCongViecs.FirstOrDefaultAsync();
                var capBac = await _context.CapBacCongViecs.FirstOrDefaultAsync();
                var loaiCongViec = await _context.LoaiCongViecs.FirstOrDefaultAsync();
                var congTy = await _context.CongTys.FirstOrDefaultAsync();

                if (danhMuc == null || capBac == null || loaiCongViec == null)
                {
                    TempData["Error"] = "Thiếu dữ liệu cơ bản. Vui lòng tạo dữ liệu cơ bản trước.";
                    return RedirectToAction("Index");
                }

                // Tạo công ty nếu chưa có
                if (congTy == null)
                {
                    congTy = new CongTy
                    {
                        TenCongTy = "Công ty Test",
                        MaSoThue = "123456789",
                        DiaChi = "123 Test Street",
                        DienThoai = "0123456789",
                        Email = "test@company.com",
                        Website = "www.test.com",
                        MoTa = "Công ty test",
                        TrangThai = true,
                        NgayTao = DateTime.Now,
                        MaNguoiDung = 1
                    };
                    _context.CongTys.Add(congTy);
                    await _context.SaveChangesAsync();
                }

                // Tạo tin tuyển dụng test
                var testJob = new TinTuyenDung
                {
                    TieuDe = $"Tin tuyển dụng TEST - {DateTime.Now:HH:mm:ss}",
                    DuongDan = $"test-job-{DateTime.Now:HHmmss}",
                    MaDanhMuc = danhMuc.MaDanhMuc,
                    MaCapBac = capBac.MaCapBac,
                    MaLoai = loaiCongViec.MaLoai,
                    MoTa = "Đây là tin tuyển dụng test để kiểm tra hiển thị trên trang chủ.",
                    YeuCau = "Yêu cầu test",
                    QuyenLoi = "Quyền lợi test",
                    LuongTu = 10000000,
                    LuongDen = 20000000,
                    HienThiLuong = true,
                    DonViTien = "VND",
                    SoLuong = 1,
                    DiaDiem = "Hà Nội",
                    ThanhPho = "Hà Nội",
                    DiaChiLamViec = "123 Test Street",
                    TrangThai = true,
                    NoiBat = true,
                    GapTuyen = false,
                    LuotXem = 0,
                    NgayTao = DateTime.Now,
                    NgayCapNhat = DateTime.Now,
                    MaNguoiDung = 1,
                    MaCongTy = congTy.MaCongTy
                };

                _context.TinTuyenDungs.Add(testJob);
                await _context.SaveChangesAsync();

                // Kiểm tra ngay lập tức
                var totalJobs = await _context.TinTuyenDungs.CountAsync(t => t.TrangThai);
                var latestJobs = await _context.TinTuyenDungs
                    .Include(t => t.CongTy)
                    .Where(t => t.TrangThai)
                    .OrderByDescending(t => t.NgayTao)
                    .Take(3)
                    .ToListAsync();

                var result = new
                {
                    Success = true,
                    Message = $"Đã tạo tin tuyển dụng test thành công! ID: {testJob.MaTin}",
                    TotalJobs = totalJobs,
                    LatestJobs = latestJobs.Select(j => new
                    {
                        j.MaTin,
                        j.TieuDe,
                        j.NgayTao,
                        j.TrangThai,
                        j.NoiBat,
                        CompanyName = j.CongTy?.TenCongTy ?? "Không có"
                    }).ToList()
                };

                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = $"Lỗi: {ex.Message}" });
            }
        }

        // Test API GetLatestJobs
        [HttpGet]
        public async Task<IActionResult> TestGetLatestJobs()
        {
            try
            {
                // Gọi API GetLatestJobs
                var response = await _context.TinTuyenDungs
                    .Include(t => t.CongTy)
                    .Include(t => t.DanhMucCongViec)
                    .Include(t => t.CapBacCongViec)
                    .Include(t => t.LoaiCongViec)
                    .Where(t => t.TrangThai == true)
                    .OrderByDescending(t => t.NoiBat)
                    .ThenByDescending(t => t.NgayTao)
                    .Take(9)
                    .ToListAsync();

                var result = new
                {
                    TotalJobs = await _context.TinTuyenDungs.CountAsync(),
                    ActiveJobs = await _context.TinTuyenDungs.CountAsync(t => t.TrangThai == true),
                    LatestJobs = response.Select(t => new
                    {
                        ID = t.MaTin,
                        Title = t.TieuDe,
                        Company = t.CongTy?.TenCongTy ?? "Không có công ty",
                        Status = t.TrangThai,
                        Created = t.NgayTao,
                        Featured = t.NoiBat,
                        Category = t.DanhMucCongViec?.TenDanhMuc ?? "Không xác định"
                    }).ToList()
                };

                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message, stackTrace = ex.StackTrace });
            }
        }

        // Test tạo tin tuyển dụng
        [HttpPost]
        public async Task<IActionResult> TestCreateJob()
        {
            try
            {
                // Tạo tin tuyển dụng test
                var testJob = new TinTuyenDung
                {
                    TieuDe = "Test Job - " + DateTime.Now.ToString("HH:mm:ss"),
                    MaDanhMuc = 1, // Giả sử có danh mục với ID = 1
                    MaCapBac = 1,  // Giả sử có cấp bậc với ID = 1
                    MaLoai = 1,    // Giả sử có loại với ID = 1
                    DiaDiem = "Hà Nội",
                    ThanhPho = "Hà Nội",
                    SoLuong = 1,
                    HienThiLuong = true,
                    LuongTu = 10000000,
                    LuongDen = 15000000,
                    DonViTien = "VND",
                    MoTa = "Mô tả công việc test",
                    YeuCau = "Yêu cầu test",
                    QuyenLoi = "Quyền lợi test",
                    NoiBat = false,
                    GapTuyen = false,
                    TrangThai = true,
                    NgayTao = DateTime.Now,
                    NgayCapNhat = DateTime.Now,
                    LuotXem = 0,
                    MaNguoiDung = 1, // Giả sử có user với ID = 1
                    MaCongTy = 1,    // Giả sử có công ty với ID = 1
                    QuocGia = "Việt Nam",
                    DuongDan = "test-job-" + DateTime.Now.Ticks
                };

                _context.TinTuyenDungs.Add(testJob);
                await _context.SaveChangesAsync();

                return Json(new { 
                    success = true, 
                    message = "Tạo tin tuyển dụng test thành công", 
                    jobId = testJob.MaTin,
                    jobTitle = testJob.TieuDe,
                    createdAt = testJob.NgayTao
                });
            }
            catch (Exception ex)
            {
                return Json(new { 
                    success = false, 
                    error = ex.Message, 
                    stackTrace = ex.StackTrace 
                });
            }
        }

        // Test endpoint để kiểm tra tin tuyển dụng trong database
        [HttpGet]
        public async Task<IActionResult> CheckJobs()
        {
            try
            {
                var allJobs = await _context.TinTuyenDungs
                    .Include(t => t.CongTy)
                    .Include(t => t.DanhMucCongViec)
                    .OrderByDescending(t => t.NgayTao)
                    .ToListAsync();

                var activeJobs = allJobs.Where(t => t.TrangThai).ToList();
                var inactiveJobs = allJobs.Where(t => !t.TrangThai).ToList();

                var result = new
                {
                    totalJobs = allJobs.Count,
                    activeJobs = activeJobs.Count,
                    inactiveJobs = inactiveJobs.Count,
                    latestJobs = activeJobs.Take(5).Select(j => new
                    {
                        id = j.MaTin,
                        title = j.TieuDe,
                        company = j.CongTy?.TenCongTy ?? "Không có công ty",
                        status = j.TrangThai,
                        created = j.NgayTao,
                        featured = j.NoiBat,
                        category = j.DanhMucCongViec?.TenDanhMuc
                    }).ToList()
                };

                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        // Test endpoint để tạo tin tuyển dụng test
        [HttpPost]
        public async Task<IActionResult> CreateTestJob()
        {
            try
            {
                // Lấy user ID đầu tiên (giả sử là employer)
                var firstUser = await _context.NguoiDungs.FirstOrDefaultAsync();
                if (firstUser == null)
                {
                    return Json(new { success = false, message = "Không có user nào trong database" });
                }

                // Lấy danh mục đầu tiên
                var firstCategory = await _context.DanhMucCongViecs.FirstOrDefaultAsync();
                if (firstCategory == null)
                {
                    return Json(new { success = false, message = "Không có danh mục nào trong database" });
                }

                var testJob = new TinTuyenDung
                {
                    TieuDe = $"Test Job - {DateTime.Now:yyyy-MM-dd HH:mm:ss}",
                    MoTa = "Đây là tin tuyển dụng test",
                    DiaDiem = "Hà Nội",
                    SoLuong = 1,
                    MaNguoiDung = firstUser.MaNguoiDung,
                    MaDanhMuc = firstCategory.MaDanhMuc,
                    NgayTao = DateTime.Now,
                    NgayCapNhat = DateTime.Now,
                    TrangThai = true,
                    LuotXem = 0,
                    HienThiLuong = false,
                    QuocGia = "Việt Nam",
                    DonViTien = "VND"
                };

                _context.TinTuyenDungs.Add(testJob);
                await _context.SaveChangesAsync();

                return Json(new { 
                    success = true, 
                    message = "Đã tạo tin test thành công", 
                    jobId = testJob.MaTin,
                    title = testJob.TieuDe
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // Test kiểm tra vai trò người dùng
        public IActionResult CheckUserRole()
        {
            var isLoggedIn = AuthorizationHelper.IsLoggedIn(HttpContext.Session);
            var userRole = AuthorizationHelper.GetUserRole(HttpContext.Session);
            var userId = AuthorizationHelper.GetUserIdAsInt(HttpContext.Session);
            
            var result = new
            {
                IsLoggedIn = isLoggedIn,
                UserRole = userRole,
                UserId = userId,
                CanPostJob = AuthorizationHelper.CanPostJob(HttpContext.Session),
                IsEmployer = AuthorizationHelper.IsEmployer(HttpContext.Session),
                IsCandidate = AuthorizationHelper.IsCandidate(HttpContext.Session),
                IsAdmin = AuthorizationHelper.IsAdmin(HttpContext.Session)
            };
            
            return Json(result);
        }
    }
} 