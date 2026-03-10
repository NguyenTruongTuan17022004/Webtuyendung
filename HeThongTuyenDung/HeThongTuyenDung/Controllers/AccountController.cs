using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HeThongTuyenDung.Data;
using HeThongTuyenDung.Models;
using HeThongTuyenDung.Helpers;
using System.Security.Cryptography;
using System.Text;

namespace HeThongTuyenDung.Controllers
{
    public class AccountController : Controller
    {
        private readonly TuyenDungDbContext _context;

        public AccountController(TuyenDungDbContext context)
        {
            _context = context;
        }

        // GET: Account/Login
        public IActionResult Login(string? returnUrl = null)
        {
            // Nếu đã đăng nhập, chuyển hướng về trang chủ
            if (AuthorizationHelper.IsLoggedIn(HttpContext.Session))
            {
                var userRole = AuthorizationHelper.GetUserRole(HttpContext.Session);
                if (userRole == "Doanh nghiệp")
                {
                    return RedirectToAction("Index", "Job");
                }
                else
                {
                    return RedirectToAction("Index", "Job");
                }
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                // Kiểm tra vai trò đã được chọn chưa
                if (string.IsNullOrEmpty(model.VaiTro))
                {
                    ModelState.AddModelError(string.Empty, "Vui lòng chọn vai trò của bạn.");
                    return View(model);
                }

                var hashedPassword = HashPassword(model.MatKhau);
                var user = await _context.NguoiDungs
                    .Include(u => u.VaiTro)
                    .FirstOrDefaultAsync(u => u.TenDangNhap == model.TenDangNhap && u.MatKhau == hashedPassword);

                if (user != null && user.TrangThai)
                {
                    // Kiểm tra vai trò có khớp không
                    if (user.VaiTro.TenVaiTro != model.VaiTro)
                    {
                        ModelState.AddModelError(string.Empty, $"Tài khoản này thuộc vai trò '{user.VaiTro.TenVaiTro}', không phải '{model.VaiTro}'. Vui lòng chọn đúng vai trò.");
                        return View(model);
                    }

                    // Cập nhật lần đăng nhập cuối
                    user.LanDangNhapCuoi = DateTime.Now;
                    await _context.SaveChangesAsync();

                    // Lưu thông tin user vào session
                    HttpContext.Session.SetString("UserId", user.MaNguoiDung.ToString());
                    HttpContext.Session.SetString("UserName", user.HoTen);
                    HttpContext.Session.SetString("UserRole", user.VaiTro.TenVaiTro);
                    HttpContext.Session.SetString("UserEmail", user.Email);

                    if (model.GhiNhoDangNhap)
                    {
                        // Có thể thêm logic lưu cookie ở đây
                    }

                    // Chuyển hướng dựa trên vai trò
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }

                    // Chuyển hướng mặc định dựa trên vai trò
                    if (user.VaiTro.TenVaiTro == "Doanh nghiệp")
                    {
                        return RedirectToAction("Index", "Job"); // Có thể thay đổi thành dashboard doanh nghiệp
                    }
                    else
                    {
                        return RedirectToAction("Index", "Job");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Tên đăng nhập hoặc mật khẩu không đúng.");
                }
            }

            return View(model);
        }

        // GET: Account/Register
        public IActionResult Register(string? role = null)
        {
            // Nếu đã đăng nhập, chuyển hướng về trang chủ
            if (AuthorizationHelper.IsLoggedIn(HttpContext.Session))
            {
                return RedirectToAction("Index", "Job");
            }

            // Nếu có vai trò được chọn từ trang RoleInfo
            if (!string.IsNullOrEmpty(role))
            {
                ViewBag.SelectedRole = role;
            }

            return View();
        }

        // POST: Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra tên đăng nhập đã tồn tại chưa
                if (await _context.NguoiDungs.AnyAsync(u => u.TenDangNhap == model.TenDangNhap))
                {
                    ModelState.AddModelError("TenDangNhap", "Tên đăng nhập đã tồn tại.");
                    return View(model);
                }

                // Kiểm tra email đã tồn tại chưa
                if (await _context.NguoiDungs.AnyAsync(u => u.Email == model.Email))
                {
                    ModelState.AddModelError("Email", "Email đã được sử dụng.");
                    return View(model);
                }

                // Lấy vai trò được chọn
                var vaiTro = await _context.VaiTros.FirstOrDefaultAsync(v => v.TenVaiTro == model.VaiTro);
                if (vaiTro == null)
                {
                    // Tạo vai trò nếu chưa có
                    vaiTro = new VaiTro { TenVaiTro = model.VaiTro, MoTa = $"Người dùng với vai trò {model.VaiTro}" };
                    _context.VaiTros.Add(vaiTro);
                    await _context.SaveChangesAsync();
                }

                var user = new NguoiDung
                {
                    TenDangNhap = model.TenDangNhap,
                    MatKhau = HashPassword(model.MatKhau),
                    Email = model.Email,
                    HoTen = model.HoTen,
                    SoDienThoai = model.SoDienThoai,
                    MaVaiTro = vaiTro.MaVaiTro,
                    TrangThai = true,
                    NgayTao = DateTime.Now,
                    NgayCapNhat = DateTime.Now
                };

                _context.NguoiDungs.Add(user);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Đăng ký thành công với vai trò {model.VaiTro}! Vui lòng đăng nhập.";
                return RedirectToAction(nameof(Login));
            }

            return View(model);
        }

        // POST: Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Job");
        }

        // GET: Account/Profile
        [RequireLogin]
        public async Task<IActionResult> Profile()
        {
            var userId = AuthorizationHelper.GetUserIdAsInt(HttpContext.Session);
            if (!userId.HasValue)
            {
                return RedirectToAction(nameof(Login));
            }

            var user = await _context.NguoiDungs
                .Include(u => u.VaiTro)
                .FirstOrDefaultAsync(u => u.MaNguoiDung == userId.Value);

            if (user == null)
            {
                return RedirectToAction(nameof(Login));
            }

            return View(user);
        }

        // POST: Account/UpdateProfile
        [HttpPost]
        [RequireLogin]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(string hoTen, string soDienThoai)
        {
            var userId = AuthorizationHelper.GetUserIdAsInt(HttpContext.Session);
            if (!userId.HasValue)
            {
                return RedirectToAction(nameof(Login));
            }

            var user = await _context.NguoiDungs.FindAsync(userId.Value);
            if (user == null)
            {
                return NotFound();
            }

            if (string.IsNullOrWhiteSpace(hoTen))
            {
                TempData["Error"] = "Họ tên không được để trống.";
                return RedirectToAction(nameof(Profile));
            }

            user.HoTen = hoTen;
            user.SoDienThoai = soDienThoai;
            user.NgayCapNhat = DateTime.Now;

            // Also update session
            HttpContext.Session.SetString("UserFullName", hoTen);

            await _context.SaveChangesAsync();
            TempData["Success"] = "Cập nhật thông tin thành công!";

            return RedirectToAction(nameof(Profile));
        }

        // POST: Account/ChangePassword
        [HttpPost]
        [RequireLogin]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(string currentPassword, string newPassword, string confirmPassword)
        {
            var userId = AuthorizationHelper.GetUserIdAsInt(HttpContext.Session);
            if (!userId.HasValue)
            {
                return RedirectToAction(nameof(Login));
            }

            if (string.IsNullOrEmpty(currentPassword) || string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(confirmPassword))
            {
                TempData["Error"] = "Vui lòng nhập đầy đủ thông tin mật khẩu.";
                return RedirectToAction(nameof(Profile));
            }

            if (newPassword != confirmPassword)
            {
                TempData["Error"] = "Mật khẩu mới và xác nhận mật khẩu không khớp.";
                return RedirectToAction(nameof(Profile));
            }

            if (newPassword.Length < 6)
            {
                TempData["Error"] = "Mật khẩu mới phải có ít nhất 6 ký tự.";
                return RedirectToAction(nameof(Profile));
            }

            var user = await _context.NguoiDungs.FindAsync(userId.Value);
            if (user == null)
            {
                return NotFound();
            }

            var hashedCurrentPassword = HashPassword(currentPassword);
            if (user.MatKhau != hashedCurrentPassword)
            {
                TempData["Error"] = "Mật khẩu hiện tại không đúng.";
                return RedirectToAction(nameof(Profile));
            }

            user.MatKhau = HashPassword(newPassword);
            user.NgayCapNhat = DateTime.Now;

            await _context.SaveChangesAsync();
            TempData["Success"] = "Đổi mật khẩu thành công!";

            return RedirectToAction(nameof(Profile));
        }

        // GET: Account/AccessDenied
        public IActionResult AccessDenied()
        {
            return View();
        }

        // Helper method để hash password
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        // GET: Account/RoleInfo
        public IActionResult RoleInfo()
        {
            return View();
        }
    }
} 