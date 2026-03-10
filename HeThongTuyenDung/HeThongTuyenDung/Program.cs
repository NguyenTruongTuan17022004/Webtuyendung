using Microsoft.EntityFrameworkCore;
using HeThongTuyenDung.Data;
using HeThongTuyenDung.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add Session support
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Add Entity Framework
builder.Services.AddDbContext<TuyenDungDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Seed dữ liệu mẫu
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TuyenDungDbContext>();

    // Đảm bảo database được tạo
    db.Database.EnsureCreated();

    // Seed vai trò nếu chưa có
    if (!db.VaiTros.Any())
    {
        db.VaiTros.AddRange(
            new VaiTro { TenVaiTro = "Ứng viên", MoTa = "Người tìm việc làm" },
            new VaiTro { TenVaiTro = "Doanh nghiệp", MoTa = "Nhà tuyển dụng" },
            new VaiTro { TenVaiTro = "Admin", MoTa = "Quản trị viên hệ thống" }
        );
        db.SaveChanges();
    }

    // Seed công ty mẫu nếu chưa có
    if (!db.CongTys.Any())
    {
        db.CongTys.AddRange(
            new CongTy
            {
                TenCongTy = "Công ty ABC",
                MoTa = "Công ty công nghệ hàng đầu Việt Nam",
                DiaChi = "Hà Nội",
                Website = "https://abc.com",
                DienThoai = "024-12345678",
                Email = "contact@abc.com"
            },
            new CongTy
            {
                TenCongTy = "TechCorp Vietnam",
                MoTa = "Công ty công nghệ quốc tế",
                DiaChi = "TP. Hồ Chí Minh",
                Website = "https://techcorp.vn",
                DienThoai = "028-87654321",
                Email = "hr@techcorp.vn"
            },
            new CongTy
            {
                TenCongTy = "Digital Solutions",
                MoTa = "Giải pháp số hàng đầu",
                DiaChi = "Đà Nẵng",
                Website = "https://digitalsolutions.com",
                DienThoai = "0236-11223344",
                Email = "info@digitalsolutions.com"
            },
            new CongTy
            {
                TenCongTy = "Innovation Hub",
                MoTa = "Trung tâm đổi mới sáng tạo",
                DiaChi = "Hà Nội",
                Website = "https://innovationhub.vn",
                DienThoai = "024-99887766",
                Email = "careers@innovationhub.vn"
            }
        );
        db.SaveChanges();
    }

    // Seed danh mục công việc nếu chưa có
    if (!db.DanhMucCongViecs.Any())
    {
        db.DanhMucCongViecs.AddRange(
            new DanhMucCongViec { TenDanhMuc = "Công nghệ thông tin", MoTa = "Việc làm trong lĩnh vực IT", ThuTu = 1, TrangThai = true },
            new DanhMucCongViec { TenDanhMuc = "Tài chính - Kế toán", MoTa = "Việc làm tài chính, kế toán", ThuTu = 2, TrangThai = true },
            new DanhMucCongViec { TenDanhMuc = "Marketing - PR", MoTa = "Việc làm marketing, quảng cáo", ThuTu = 3, TrangThai = true },
            new DanhMucCongViec { TenDanhMuc = "Nhân sự", MoTa = "Việc làm quản lý nhân sự", ThuTu = 4, TrangThai = true },
            new DanhMucCongViec { TenDanhMuc = "Kinh doanh - Bán hàng", MoTa = "Việc làm kinh doanh, bán hàng", ThuTu = 5, TrangThai = true },
            new DanhMucCongViec { TenDanhMuc = "Giáo dục - Đào tạo", MoTa = "Việc làm giáo dục, giảng dạy", ThuTu = 6, TrangThai = true },
            new DanhMucCongViec { TenDanhMuc = "Y tế - Chăm sóc sức khỏe", MoTa = "Việc làm y tế, chăm sóc sức khỏe", ThuTu = 7, TrangThai = true },
            new DanhMucCongViec { TenDanhMuc = "Du lịch - Khách sạn", MoTa = "Việc làm du lịch, khách sạn", ThuTu = 8, TrangThai = true },
            new DanhMucCongViec { TenDanhMuc = "Xây dựng - Kiến trúc", MoTa = "Việc làm xây dựng, kiến trúc", ThuTu = 9, TrangThai = true },
            new DanhMucCongViec { TenDanhMuc = "Khác", MoTa = "Các việc làm khác", ThuTu = 10, TrangThai = true }
        );
        db.SaveChanges();
    }

    // Seed cấp bậc công việc nếu chưa có
    if (!db.CapBacCongViecs.Any())
    {
        db.CapBacCongViecs.AddRange(
            new CapBacCongViec { TenCapBac = "Thực tập sinh", MoTa = "Vị trí thực tập" },
            new CapBacCongViec { TenCapBac = "Nhân viên", MoTa = "Vị trí nhân viên" },
            new CapBacCongViec { TenCapBac = "Trưởng nhóm", MoTa = "Vị trí trưởng nhóm" },
            new CapBacCongViec { TenCapBac = "Quản lý", MoTa = "Vị trí quản lý" },
            new CapBacCongViec { TenCapBac = "Giám đốc", MoTa = "Vị trí giám đốc" },
            new CapBacCongViec { TenCapBac = "C-level", MoTa = "Vị trí cấp cao" }
        );
        db.SaveChanges();
    }

    // Seed loại công việc nếu chưa có
    if (!db.LoaiCongViecs.Any())
    {
        db.LoaiCongViecs.AddRange(
            new LoaiCongViec { TenLoai = "Toàn thời gian", MoTa = "Làm việc toàn thời gian" },
            new LoaiCongViec { TenLoai = "Bán thời gian", MoTa = "Làm việc bán thời gian" },
            new LoaiCongViec { TenLoai = "Thực tập", MoTa = "Vị trí thực tập" },
            new LoaiCongViec { TenLoai = "Hợp đồng", MoTa = "Làm việc theo hợp đồng" },
            new LoaiCongViec { TenLoai = "Từ xa", MoTa = "Làm việc từ xa" }
        );
        db.SaveChanges();
    }

    // Seed người dùng mẫu nếu chưa có
    if (!db.NguoiDungs.Any())
    {
        var adminRole = db.VaiTros.First(v => v.TenVaiTro == "Admin");
        var employerRole = db.VaiTros.First(v => v.TenVaiTro == "Doanh nghiệp");
        var candidateRole = db.VaiTros.First(v => v.TenVaiTro == "Ứng viên");

        db.NguoiDungs.AddRange(
            // Admin
            new NguoiDung
            {
                TenDangNhap = "admin",
                MatKhau = "123456", // Trong thực tế nên hash password
                Email = "admin@jobnow.com",
                HoTen = "Administrator",
                MaVaiTro = adminRole.MaVaiTro,
                TrangThai = true,
                NgayTao = DateTime.Now
            },
            // Employer
            new NguoiDung
            {
                TenDangNhap = "employer",
                MatKhau = "123456",
                Email = "hr@abc.com",
                HoTen = "HR Manager",
                MaVaiTro = employerRole.MaVaiTro,
                TrangThai = true,
                NgayTao = DateTime.Now
            },
            // Candidate
            new NguoiDung
            {
                TenDangNhap = "candidate",
                MatKhau = "123456",
                Email = "candidate@gmail.com",
                HoTen = "Nguyễn Văn A",
                MaVaiTro = candidateRole.MaVaiTro,
                TrangThai = true,
                NgayTao = DateTime.Now
            }
        );
        db.SaveChanges();
    }
    
    // Cập nhật người dùng cho công ty
    var employerUser = db.NguoiDungs.FirstOrDefault(u => u.TenDangNhap == "employer");
    if (employerUser != null)
    {
        // Gán công ty ABC cho user employer
        var companyABC = db.CongTys.FirstOrDefault(c => c.TenCongTy == "Công ty ABC");
        if (companyABC != null && companyABC.MaNguoiDung == null)
        {
            companyABC.MaNguoiDung = employerUser.MaNguoiDung;
            db.SaveChanges();
        }
    }

    // Seed kỹ năng nếu chưa có
    if (!db.KyNangs.Any())
    {
        db.KyNangs.AddRange(
            new KyNang { TenKyNang = "C#", MoTa = "Ngôn ngữ lập trình C#" },
            new KyNang { TenKyNang = "ASP.NET Core", MoTa = "Framework web ASP.NET Core" },
            new KyNang { TenKyNang = "Entity Framework", MoTa = "ORM Entity Framework" },
            new KyNang { TenKyNang = "SQL Server", MoTa = "Hệ quản trị cơ sở dữ liệu SQL Server" },
            new KyNang { TenKyNang = "JavaScript", MoTa = "Ngôn ngữ lập trình JavaScript" },
            new KyNang { TenKyNang = "React", MoTa = "Framework JavaScript React" },
            new KyNang { TenKyNang = "Vue.js", MoTa = "Framework JavaScript Vue.js" },
            new KyNang { TenKyNang = "HTML/CSS", MoTa = "HTML và CSS" },
            new KyNang { TenKyNang = "Git", MoTa = "Hệ thống quản lý phiên bản Git" },
            new KyNang { TenKyNang = "Docker", MoTa = "Containerization với Docker" },
            new KyNang { TenKyNang = "Azure", MoTa = "Microsoft Azure Cloud" },
            new KyNang { TenKyNang = "AWS", MoTa = "Amazon Web Services" },
            new KyNang { TenKyNang = "Agile/Scrum", MoTa = "Phương pháp phát triển Agile/Scrum" },
            new KyNang { TenKyNang = "UI/UX Design", MoTa = "Thiết kế giao diện người dùng" },
            new KyNang { TenKyNang = "Figma", MoTa = "Công cụ thiết kế Figma" },
            new KyNang { TenKyNang = "Adobe Photoshop", MoTa = "Phần mềm chỉnh sửa ảnh Photoshop" },
            new KyNang { TenKyNang = "Excel", MoTa = "Microsoft Excel" },
            new KyNang { TenKyNang = "PowerPoint", MoTa = "Microsoft PowerPoint" },
            new KyNang { TenKyNang = "Word", MoTa = "Microsoft Word" },
            new KyNang { TenKyNang = "Tiếng Anh", MoTa = "Kỹ năng tiếng Anh" },
            new KyNang { TenKyNang = "Giao tiếp", MoTa = "Kỹ năng giao tiếp" },
            new KyNang { TenKyNang = "Làm việc nhóm", MoTa = "Kỹ năng làm việc nhóm" },
            new KyNang { TenKyNang = "Quản lý thời gian", MoTa = "Kỹ năng quản lý thời gian" },
            new KyNang { TenKyNang = "Giải quyết vấn đề", MoTa = "Kỹ năng giải quyết vấn đề" }
        );
        db.SaveChanges();
    }

    // Seed tin tuyển dụng mẫu nếu chưa có
    if (!db.TinTuyenDungs.Any())
    {
        var congTyABC = db.CongTys.First(c => c.TenCongTy == "Công ty ABC");
        var congTyTechCorp = db.CongTys.First(c => c.TenCongTy == "TechCorp Vietnam");
        var congTyDigital = db.CongTys.First(c => c.TenCongTy == "Digital Solutions");
        var congTyInnovation = db.CongTys.First(c => c.TenCongTy == "Innovation Hub");
        
        var danhMucIT = db.DanhMucCongViecs.First(d => d.TenDanhMuc == "Công nghệ thông tin");
        var capBacNhanVien = db.CapBacCongViecs.First(c => c.TenCapBac == "Nhân viên");
        var loaiToanThoiGian = db.LoaiCongViecs.First(l => l.TenLoai == "Toàn thời gian");
        
        db.TinTuyenDungs.AddRange(
            new TinTuyenDung
            {
                TieuDe = "Lập trình viên .NET",
                DuongDan = "lap-trinh-vien-dotnet",
                MaCongTy = congTyABC.MaCongTy,
                MaNguoiDung = employerUser?.MaNguoiDung, // Gán cho user mẫu
                MaDanhMuc = danhMucIT.MaDanhMuc,
                MaCapBac = capBacNhanVien.MaCapBac,
                MaLoai = loaiToanThoiGian.MaLoai,
                MoTa = "Phát triển phần mềm trên nền tảng .NET Core, ASP.NET Core",
                YeuCau = "Có kinh nghiệm 2+ năm với .NET, SQL Server, Entity Framework",
                LuongTu = 15000000,
                LuongDen = 25000000,
                HienThiLuong = true,
                DonViTien = "VND",
                SoLuong = 3,
                DiaDiem = "Hà Nội",
                ThanhPho = "Hà Nội",
                NgayTao = DateTime.Now,
                TrangThai = true,
                NoiBat = true
            },
            new TinTuyenDung
            {
                TieuDe = "Frontend Developer",
                DuongDan = "frontend-developer",
                MaCongTy = congTyTechCorp.MaCongTy,
                MaDanhMuc = danhMucIT.MaDanhMuc,
                MaCapBac = capBacNhanVien.MaCapBac,
                MaLoai = loaiToanThoiGian.MaLoai,
                MoTa = "Phát triển giao diện người dùng với React, Vue.js",
                YeuCau = "Có kinh nghiệm HTML, CSS, JavaScript, React/Vue.js",
                LuongTu = 12000000,
                LuongDen = 20000000,
                HienThiLuong = true,
                DonViTien = "VND",
                SoLuong = 2,
                DiaDiem = "TP. Hồ Chí Minh",
                ThanhPho = "TP. Hồ Chí Minh",
                NgayTao = DateTime.Now,
                TrangThai = true,
                NoiBat = true
            },
            new TinTuyenDung
            {
                TieuDe = "UI/UX Designer",
                DuongDan = "ui-ux-designer",
                MaCongTy = congTyDigital.MaCongTy,
                MaDanhMuc = danhMucIT.MaDanhMuc,
                MaCapBac = capBacNhanVien.MaCapBac,
                MaLoai = loaiToanThoiGian.MaLoai,
                MoTa = "Thiết kế giao diện người dùng và trải nghiệm người dùng",
                YeuCau = "Có kinh nghiệm Figma, Adobe XD, Photoshop",
                LuongTu = 10000000,
                LuongDen = 18000000,
                HienThiLuong = true,
                DonViTien = "VND",
                SoLuong = 1,
                DiaDiem = "Hà Nội",
                ThanhPho = "Hà Nội",
                NgayTao = DateTime.Now,
                TrangThai = true,
                NoiBat = true
            },
            new TinTuyenDung
            {
                TieuDe = "Backend Developer",
                DuongDan = "backend-developer",
                MaCongTy = congTyInnovation.MaCongTy,
                MaDanhMuc = danhMucIT.MaDanhMuc,
                MaCapBac = capBacNhanVien.MaCapBac,
                MaLoai = loaiToanThoiGian.MaLoai,
                MoTa = "Phát triển backend với Node.js, Python, Java",
                YeuCau = "Có kinh nghiệm RESTful API, Database design",
                LuongTu = 13000000,
                LuongDen = 22000000,
                HienThiLuong = true,
                DonViTien = "VND",
                SoLuong = 2,
                DiaDiem = "Đà Nẵng",
                ThanhPho = "Đà Nẵng",
                NgayTao = DateTime.Now.AddDays(-1),
                TrangThai = true,
                NoiBat = false
            },
            new TinTuyenDung
            {
                TieuDe = "DevOps Engineer",
                DuongDan = "devops-engineer",
                MaCongTy = congTyABC.MaCongTy,
                MaNguoiDung = employerUser?.MaNguoiDung,
                MaDanhMuc = danhMucIT.MaDanhMuc,
                MaCapBac = capBacNhanVien.MaCapBac,
                MaLoai = loaiToanThoiGian.MaLoai,
                MoTa = "Quản lý infrastructure và deployment",
                YeuCau = "Có kinh nghiệm Docker, Kubernetes, CI/CD",
                LuongTu = 18000000,
                LuongDen = 30000000,
                HienThiLuong = true,
                DonViTien = "VND",
                SoLuong = 1,
                DiaDiem = "TP. Hồ Chí Minh",
                ThanhPho = "TP. Hồ Chí Minh",
                NgayTao = DateTime.Now.AddDays(-2),
                TrangThai = true,
                NoiBat = false
            },
            new TinTuyenDung
            {
                TieuDe = "Mobile Developer",
                DuongDan = "mobile-developer",
                MaCongTy = congTyTechCorp.MaCongTy,
                MaDanhMuc = danhMucIT.MaDanhMuc,
                MaCapBac = capBacNhanVien.MaCapBac,
                MaLoai = loaiToanThoiGian.MaLoai,
                MoTa = "Phát triển ứng dụng mobile với React Native, Flutter",
                YeuCau = "Có kinh nghiệm React Native hoặc Flutter",
                LuongTu = 14000000,
                LuongDen = 24000000,
                HienThiLuong = true,
                DonViTien = "VND",
                SoLuong = 2,
                DiaDiem = "Hà Nội",
                ThanhPho = "Hà Nội",
                NgayTao = DateTime.Now.AddDays(-3),
                TrangThai = true,
                NoiBat = false
            },
            new TinTuyenDung
            {
                TieuDe = "Full Stack Developer",
                DuongDan = "full-stack-developer",
                MaCongTy = congTyDigital.MaCongTy,
                MaDanhMuc = danhMucIT.MaDanhMuc,
                MaCapBac = capBacNhanVien.MaCapBac,
                MaLoai = loaiToanThoiGian.MaLoai,
                MoTa = "Phát triển full stack với MERN stack hoặc MEAN stack",
                YeuCau = "Có kinh nghiệm MongoDB, Express, React, Node.js",
                LuongTu = 16000000,
                LuongDen = 28000000,
                HienThiLuong = true,
                DonViTien = "VND",
                SoLuong = 1,
                DiaDiem = "TP. Hồ Chí Minh",
                ThanhPho = "TP. Hồ Chí Minh",
                NgayTao = DateTime.Now.AddDays(-4),
                TrangThai = true,
                NoiBat = false
            },
            new TinTuyenDung
            {
                TieuDe = "Data Scientist",
                DuongDan = "data-scientist",
                MaCongTy = congTyInnovation.MaCongTy,
                MaDanhMuc = danhMucIT.MaDanhMuc,
                MaCapBac = capBacNhanVien.MaCapBac,
                MaLoai = loaiToanThoiGian.MaLoai,
                MoTa = "Phân tích dữ liệu và xây dựng mô hình machine learning",
                YeuCau = "Có kinh nghiệm Python, R, SQL, Machine Learning",
                LuongTu = 20000000,
                LuongDen = 35000000,
                HienThiLuong = true,
                DonViTien = "VND",
                SoLuong = 1,
                DiaDiem = "Hà Nội",
                ThanhPho = "Hà Nội",
                NgayTao = DateTime.Now.AddDays(-5),
                TrangThai = true,
                NoiBat = false
            }
        );
        db.SaveChanges();
    }
}

app.Run();