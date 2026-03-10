using System.Diagnostics;
using HeThongTuyenDung.Models;
using HeThongTuyenDung.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HeThongTuyenDung.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly TuyenDungDbContext _context;

        public HomeController(ILogger<HomeController> logger, TuyenDungDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Index()
        {
            try
            {
                _logger.LogInformation("Loading homepage data...");

                // Lấy danh sách tin tuyển dụng nổi bật (có đánh dấu nổi bật)
                var featuredJobs = await _context.TinTuyenDungs
                    .Include(t => t.CongTy)
                    .Include(t => t.DanhMucCongViec)
                    .Include(t => t.CapBacCongViec)
                    .Include(t => t.LoaiCongViec)
                    .Where(t => t.TrangThai && t.NoiBat)
                    .OrderByDescending(t => t.NgayTao)
                    .Take(6)
                    .ToListAsync();

                _logger.LogInformation($"Found {featuredJobs.Count} featured jobs");

                // Lấy danh sách tin tuyển dụng mới nhất (tất cả tin đang hoạt động)
                // Ưu tiên tin mới nhất và tin nổi bật
                var latestJobs = await _context.TinTuyenDungs
                    .Include(t => t.CongTy)
                    .Include(t => t.DanhMucCongViec)
                    .Include(t => t.CapBacCongViec)
                    .Include(t => t.LoaiCongViec)
                    .Where(t => t.TrangThai)
                    .OrderByDescending(t => t.NoiBat) // Tin nổi bật lên đầu
                    .ThenByDescending(t => t.NgayTao) // Sau đó sắp xếp theo ngày tạo
                    .Take(9)
                    .ToListAsync();

                _logger.LogInformation($"Found {latestJobs.Count} latest jobs");

                // Log chi tiết từng tin tuyển dụng
                foreach (var job in latestJobs)
                {
                    _logger.LogInformation($"Job ID: {job.MaTin}, Title: {job.TieuDe}, Status: {job.TrangThai}, Company: {job.CongTy?.TenCongTy ?? "No Company"}, Created: {job.NgayTao}, Featured: {job.NoiBat}");
                }

                // Lấy thống kê
                var totalJobs = await _context.TinTuyenDungs.CountAsync(t => t.TrangThai);
                var totalCompanies = await _context.CongTys.CountAsync();
                var totalCandidates = await _context.NguoiDungs
                    .Include(n => n.VaiTro)
                    .CountAsync(n => n.VaiTro.TenVaiTro == "Ứng viên");

                // Lấy số tin tuyển dụng được đăng trong 7 ngày qua
                var recentJobsCount = await _context.TinTuyenDungs
                    .CountAsync(t => t.TrangThai && t.NgayTao >= DateTime.Now.AddDays(-7));

                _logger.LogInformation($"Statistics: Total jobs: {totalJobs}, Recent jobs: {recentJobsCount}");

                var viewModel = new HomeViewModel
                {
                    FeaturedJobs = featuredJobs,
                    LatestJobs = latestJobs,
                    TotalJobs = totalJobs,
                    TotalCompanies = totalCompanies,
                    TotalCandidates = totalCandidates,
                    RecentJobsCount = recentJobsCount
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading homepage data");
                
                // Return empty view model if there's an error
                var viewModel = new HomeViewModel
                {
                    FeaturedJobs = new List<TinTuyenDung>(),
                    LatestJobs = new List<TinTuyenDung>(),
                    TotalJobs = 0,
                    TotalCompanies = 0,
                    TotalCandidates = 0,
                    RecentJobsCount = 0
                };

                return View(viewModel);
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        // Trang danh sách công ty
        public async Task<IActionResult> Companies()
        {
            var companies = await _context.CongTys
                .Include(c => c.TinTuyenDungs)
                .OrderByDescending(c => c.NgayTao)
                .ToListAsync();

            return View(companies);
        }

        // Trang tin tức (sử dụng các tin tuyển dụng mới nhất như bài viết)
        public async Task<IActionResult> News()
        {
            var newsItems = await _context.TinTuyenDungs
                .Include(t => t.CongTy)
                .Where(t => t.TrangThai)
                .OrderByDescending(t => t.NgayTao)
                .Take(10)
                .ToListAsync();

            return View(newsItems);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // API để lấy việc làm mới nhất (cho real-time update)
        [HttpGet]
        public async Task<IActionResult> GetLatestJobs()
        {
            try
            {
                _logger.LogInformation("API GetLatestJobs called");
                
                // Kiểm tra tổng số tin tuyển dụng
                var totalJobs = await _context.TinTuyenDungs.CountAsync();
                var activeJobs = await _context.TinTuyenDungs.CountAsync(t => t.TrangThai == true);
                _logger.LogInformation($"Total jobs: {totalJobs}, Active jobs: {activeJobs}");
                
                var latestJobs = await _context.TinTuyenDungs
                    .Include(t => t.CongTy)
                    .Include(t => t.DanhMucCongViec)
                    .Include(t => t.CapBacCongViec)
                    .Include(t => t.LoaiCongViec)
                    .Where(t => t.TrangThai == true)
                    .OrderByDescending(t => t.NoiBat) // Tin nổi bật lên đầu
                    .ThenByDescending(t => t.NgayTao) // Sau đó sắp xếp theo ngày tạo
                    .Take(9) // Lấy 9 tin như trong Index()
                    .Select(t => new
                    {
                        maTin = t.MaTin,
                        tieuDe = t.TieuDe,
                        companyName = t.CongTy != null ? t.CongTy.TenCongTy : "Công ty không xác định",
                        diaDiem = t.DiaDiem,
                        luongTu = t.LuongTu,
                        luongDen = t.LuongDen,
                        donViTien = t.DonViTien,
                        hienThiLuong = t.HienThiLuong,
                        noiBat = t.NoiBat,
                        luotXem = t.LuotXem,
                        ngayTao = t.NgayTao,
                        categoryName = t.DanhMucCongViec != null ? t.DanhMucCongViec.TenDanhMuc : "Không xác định",
                        levelName = t.CapBacCongViec != null ? t.CapBacCongViec.TenCapBac : "Không xác định",
                        typeName = t.LoaiCongViec != null ? t.LoaiCongViec.TenLoai : "Không xác định",
                        urgentHiring = t.NoiBat,
                        jobType = t.LoaiCongViec != null ? t.LoaiCongViec.TenLoai : "Toàn thời gian"
                    })
                    .ToListAsync();

                _logger.LogInformation($"API GetLatestJobs returned {latestJobs.Count} jobs");
                
                // Log chi tiết từng job
                foreach (var job in latestJobs)
                {
                    _logger.LogInformation($"Job: ID={job.maTin}, Title={job.tieuDe}, Company={job.companyName}, Created={job.ngayTao}, Featured={job.noiBat}");
                }
                
                return Json(new { success = true, jobs = latestJobs });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy việc làm mới nhất");
                return Json(new { success = false, message = "Lỗi khi tải dữ liệu" });
            }
        }
    }
}
