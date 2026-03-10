using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HeThongTuyenDung.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CapBacCongViecs",
                columns: table => new
                {
                    MaCapBac = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenCapBac = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CapBacCongViecs", x => x.MaCapBac);
                });

            migrationBuilder.CreateTable(
                name: "DanhMucCongViecs",
                columns: table => new
                {
                    MaDanhMuc = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenDanhMuc = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    BieuTuong = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ThuTu = table.Column<int>(type: "int", nullable: false),
                    TrangThai = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DanhMucCongViecs", x => x.MaDanhMuc);
                });

            migrationBuilder.CreateTable(
                name: "KyNangs",
                columns: table => new
                {
                    MaKyNang = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenKyNang = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    LoaiKyNang = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BieuTuong = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TrangThai = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KyNangs", x => x.MaKyNang);
                });

            migrationBuilder.CreateTable(
                name: "LoaiCongViecs",
                columns: table => new
                {
                    MaLoai = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenLoai = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoaiCongViecs", x => x.MaLoai);
                });

            migrationBuilder.CreateTable(
                name: "QuyenHans",
                columns: table => new
                {
                    MaQuyen = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenQuyen = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuyenHans", x => x.MaQuyen);
                });

            migrationBuilder.CreateTable(
                name: "VaiTros",
                columns: table => new
                {
                    MaVaiTro = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenVaiTro = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VaiTros", x => x.MaVaiTro);
                });

            migrationBuilder.CreateTable(
                name: "NguoiDungs",
                columns: table => new
                {
                    MaNguoiDung = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenDangNhap = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MatKhau = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    HoTen = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SoDienThoai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    AnhDaiDien = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    MaVaiTro = table.Column<int>(type: "int", nullable: false),
                    TrangThai = table.Column<bool>(type: "bit", nullable: false),
                    LanDangNhapCuoi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayCapNhat = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NguoiDungs", x => x.MaNguoiDung);
                    table.ForeignKey(
                        name: "FK_NguoiDungs_VaiTros_MaVaiTro",
                        column: x => x.MaVaiTro,
                        principalTable: "VaiTros",
                        principalColumn: "MaVaiTro",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VaiTroQuyenHans",
                columns: table => new
                {
                    MaVaiTro = table.Column<int>(type: "int", nullable: false),
                    MaQuyen = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VaiTroQuyenHans", x => new { x.MaVaiTro, x.MaQuyen });
                    table.ForeignKey(
                        name: "FK_VaiTroQuyenHans_QuyenHans_MaQuyen",
                        column: x => x.MaQuyen,
                        principalTable: "QuyenHans",
                        principalColumn: "MaQuyen",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VaiTroQuyenHans_VaiTros_MaVaiTro",
                        column: x => x.MaVaiTro,
                        principalTable: "VaiTros",
                        principalColumn: "MaVaiTro",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CongTys",
                columns: table => new
                {
                    MaCongTy = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenCongTy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Logo = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    AnhBia = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MoTaNgan = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DiaChi = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ThanhPho = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    QuocGia = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DienThoai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Website = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    QuyMo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LinhVuc = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    MaSoThue = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DaXacThuc = table.Column<bool>(type: "bit", nullable: false),
                    TrangThai = table.Column<bool>(type: "bit", nullable: false),
                    MaNguoiDung = table.Column<int>(type: "int", nullable: true),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayCapNhat = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CongTys", x => x.MaCongTy);
                    table.ForeignKey(
                        name: "FK_CongTys_NguoiDungs_MaNguoiDung",
                        column: x => x.MaNguoiDung,
                        principalTable: "NguoiDungs",
                        principalColumn: "MaNguoiDung");
                });

            migrationBuilder.CreateTable(
                name: "HoSoUngViens",
                columns: table => new
                {
                    MaHoSo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaNguoiDung = table.Column<int>(type: "int", nullable: true),
                    ViTriMongMuon = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    GioiThieu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SoNamKinhNghiem = table.Column<int>(type: "int", nullable: true),
                    CongViecHienTai = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CongTyHienTai = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    MucLuongHienTai = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MucLuongMongMuon = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DonViTien = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    DiaDiemMongMuon = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    LoaiCongViecMongMuon = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CoTheNuocNgoai = table.Column<bool>(type: "bit", nullable: false),
                    DangTimViec = table.Column<bool>(type: "bit", nullable: false),
                    CongKhaiHoSo = table.Column<bool>(type: "bit", nullable: false),
                    LuotXem = table.Column<int>(type: "int", nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayCapNhat = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HoSoUngViens", x => x.MaHoSo);
                    table.ForeignKey(
                        name: "FK_HoSoUngViens_NguoiDungs_MaNguoiDung",
                        column: x => x.MaNguoiDung,
                        principalTable: "NguoiDungs",
                        principalColumn: "MaNguoiDung");
                });

            migrationBuilder.CreateTable(
                name: "ThongBaos",
                columns: table => new
                {
                    MaThongBao = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaNguoiDung = table.Column<int>(type: "int", nullable: true),
                    TieuDe = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    NoiDung = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LoaiThongBao = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MaLienQuan = table.Column<int>(type: "int", nullable: true),
                    DaXem = table.Column<bool>(type: "bit", nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThongBaos", x => x.MaThongBao);
                    table.ForeignKey(
                        name: "FK_ThongBaos_NguoiDungs_MaNguoiDung",
                        column: x => x.MaNguoiDung,
                        principalTable: "NguoiDungs",
                        principalColumn: "MaNguoiDung");
                });

            migrationBuilder.CreateTable(
                name: "AnhCongTys",
                columns: table => new
                {
                    MaAnh = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaCongTy = table.Column<int>(type: "int", nullable: true),
                    DuongDanAnh = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    MoTaAnh = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ThuTu = table.Column<int>(type: "int", nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnhCongTys", x => x.MaAnh);
                    table.ForeignKey(
                        name: "FK_AnhCongTys_CongTys_MaCongTy",
                        column: x => x.MaCongTy,
                        principalTable: "CongTys",
                        principalColumn: "MaCongTy");
                });

            migrationBuilder.CreateTable(
                name: "TinTuyenDungs",
                columns: table => new
                {
                    MaTin = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TieuDe = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DuongDan = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    MaDanhMuc = table.Column<int>(type: "int", nullable: true),
                    MaCapBac = table.Column<int>(type: "int", nullable: true),
                    MaLoai = table.Column<int>(type: "int", nullable: true),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YeuCau = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QuyenLoi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LuongTu = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    LuongDen = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    HienThiLuong = table.Column<bool>(type: "bit", nullable: false),
                    DonViTien = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    KinhNghiemTu = table.Column<int>(type: "int", nullable: true),
                    KinhNghiemDen = table.Column<int>(type: "int", nullable: true),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    GioiTinh = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    DoTuoi = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DiaDiem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ThanhPho = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QuocGia = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DiaChiLamViec = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HanNop = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TrangThai = table.Column<bool>(type: "bit", nullable: false),
                    NoiBat = table.Column<bool>(type: "bit", nullable: false),
                    GapTuyen = table.Column<bool>(type: "bit", nullable: false),
                    LuotXem = table.Column<int>(type: "int", nullable: false),
                    NguoiTao = table.Column<int>(type: "int", nullable: true),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayCapNhat = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MaNguoiDung = table.Column<int>(type: "int", nullable: true),
                    MaCongTy = table.Column<int>(type: "int", nullable: true),
                    CongTyMaCongTy = table.Column<int>(type: "int", nullable: true),
                    DanhMucCongViecMaDanhMuc = table.Column<int>(type: "int", nullable: true),
                    CapBacCongViecMaCapBac = table.Column<int>(type: "int", nullable: true),
                    LoaiCongViecMaLoai = table.Column<int>(type: "int", nullable: true),
                    NguoiTaoUserMaNguoiDung = table.Column<int>(type: "int", nullable: true),
                    NguoiDungMaNguoiDung = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TinTuyenDungs", x => x.MaTin);
                    table.ForeignKey(
                        name: "FK_TinTuyenDungs_CapBacCongViecs_CapBacCongViecMaCapBac",
                        column: x => x.CapBacCongViecMaCapBac,
                        principalTable: "CapBacCongViecs",
                        principalColumn: "MaCapBac");
                    table.ForeignKey(
                        name: "FK_TinTuyenDungs_CongTys_CongTyMaCongTy",
                        column: x => x.CongTyMaCongTy,
                        principalTable: "CongTys",
                        principalColumn: "MaCongTy");
                    table.ForeignKey(
                        name: "FK_TinTuyenDungs_DanhMucCongViecs_DanhMucCongViecMaDanhMuc",
                        column: x => x.DanhMucCongViecMaDanhMuc,
                        principalTable: "DanhMucCongViecs",
                        principalColumn: "MaDanhMuc");
                    table.ForeignKey(
                        name: "FK_TinTuyenDungs_LoaiCongViecs_LoaiCongViecMaLoai",
                        column: x => x.LoaiCongViecMaLoai,
                        principalTable: "LoaiCongViecs",
                        principalColumn: "MaLoai");
                    table.ForeignKey(
                        name: "FK_TinTuyenDungs_NguoiDungs_NguoiDungMaNguoiDung",
                        column: x => x.NguoiDungMaNguoiDung,
                        principalTable: "NguoiDungs",
                        principalColumn: "MaNguoiDung");
                    table.ForeignKey(
                        name: "FK_TinTuyenDungs_NguoiDungs_NguoiTaoUserMaNguoiDung",
                        column: x => x.NguoiTaoUserMaNguoiDung,
                        principalTable: "NguoiDungs",
                        principalColumn: "MaNguoiDung");
                });

            migrationBuilder.CreateTable(
                name: "ChungChis",
                columns: table => new
                {
                    MaChungChi = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaHoSo = table.Column<int>(type: "int", nullable: true),
                    TenChungChi = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DonViCap = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    NgayCap = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NgayHetHan = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HoSoUngVienMaHoSo = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChungChis", x => x.MaChungChi);
                    table.ForeignKey(
                        name: "FK_ChungChis_HoSoUngViens_HoSoUngVienMaHoSo",
                        column: x => x.HoSoUngVienMaHoSo,
                        principalTable: "HoSoUngViens",
                        principalColumn: "MaHoSo");
                });

            migrationBuilder.CreateTable(
                name: "DuAns",
                columns: table => new
                {
                    MaDuAn = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaHoSo = table.Column<int>(type: "int", nullable: true),
                    TenDuAn = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    VaiTro = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    NgayBatDau = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NgayKetThuc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DangThucHien = table.Column<bool>(type: "bit", nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CongNgheSuDung = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    LienKetDuAn = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HoSoUngVienMaHoSo = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DuAns", x => x.MaDuAn);
                    table.ForeignKey(
                        name: "FK_DuAns_HoSoUngViens_HoSoUngVienMaHoSo",
                        column: x => x.HoSoUngVienMaHoSo,
                        principalTable: "HoSoUngViens",
                        principalColumn: "MaHoSo");
                });

            migrationBuilder.CreateTable(
                name: "HocVans",
                columns: table => new
                {
                    MaHocVan = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaHoSo = table.Column<int>(type: "int", nullable: true),
                    TenTruong = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    BangCap = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ChuyenNganh = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    NgayBatDau = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NgayKetThuc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DangHoc = table.Column<bool>(type: "bit", nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HoSoUngVienMaHoSo = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HocVans", x => x.MaHocVan);
                    table.ForeignKey(
                        name: "FK_HocVans_HoSoUngViens_HoSoUngVienMaHoSo",
                        column: x => x.HoSoUngVienMaHoSo,
                        principalTable: "HoSoUngViens",
                        principalColumn: "MaHoSo");
                });

            migrationBuilder.CreateTable(
                name: "KinhNghiems",
                columns: table => new
                {
                    MaKinhNghiem = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaHoSo = table.Column<int>(type: "int", nullable: true),
                    TenCongTy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ChucVu = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    NgayBatDau = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NgayKetThuc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DangLamViec = table.Column<bool>(type: "bit", nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HoSoUngVienMaHoSo = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KinhNghiems", x => x.MaKinhNghiem);
                    table.ForeignKey(
                        name: "FK_KinhNghiems_HoSoUngViens_HoSoUngVienMaHoSo",
                        column: x => x.HoSoUngVienMaHoSo,
                        principalTable: "HoSoUngViens",
                        principalColumn: "MaHoSo");
                });

            migrationBuilder.CreateTable(
                name: "KyNangUngViens",
                columns: table => new
                {
                    MaHoSo = table.Column<int>(type: "int", nullable: false),
                    MaKyNang = table.Column<int>(type: "int", nullable: false),
                    CapDo = table.Column<int>(type: "int", nullable: false),
                    SoNamKinhNghiem = table.Column<int>(type: "int", nullable: true),
                    MoTa = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    HoSoUngVienMaHoSo = table.Column<int>(type: "int", nullable: false),
                    KyNangMaKyNang = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KyNangUngViens", x => new { x.MaHoSo, x.MaKyNang });
                    table.ForeignKey(
                        name: "FK_KyNangUngViens_HoSoUngViens_HoSoUngVienMaHoSo",
                        column: x => x.HoSoUngVienMaHoSo,
                        principalTable: "HoSoUngViens",
                        principalColumn: "MaHoSo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KyNangUngViens_KyNangs_KyNangMaKyNang",
                        column: x => x.KyNangMaKyNang,
                        principalTable: "KyNangs",
                        principalColumn: "MaKyNang",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UngVienDaLuus",
                columns: table => new
                {
                    MaNguoiDung = table.Column<int>(type: "int", nullable: false),
                    MaHoSo = table.Column<int>(type: "int", nullable: false),
                    NgayLuu = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NguoiDungMaNguoiDung = table.Column<int>(type: "int", nullable: false),
                    HoSoUngVienMaHoSo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UngVienDaLuus", x => new { x.MaNguoiDung, x.MaHoSo });
                    table.ForeignKey(
                        name: "FK_UngVienDaLuus_HoSoUngViens_HoSoUngVienMaHoSo",
                        column: x => x.HoSoUngVienMaHoSo,
                        principalTable: "HoSoUngViens",
                        principalColumn: "MaHoSo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UngVienDaLuus_NguoiDungs_NguoiDungMaNguoiDung",
                        column: x => x.NguoiDungMaNguoiDung,
                        principalTable: "NguoiDungs",
                        principalColumn: "MaNguoiDung",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DonUngTuyens",
                columns: table => new
                {
                    MaDon = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaTin = table.Column<int>(type: "int", nullable: true),
                    MaHoSo = table.Column<int>(type: "int", nullable: true),
                    MaNguoiDung = table.Column<int>(type: "int", nullable: true),
                    GhiChu = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ThuNgo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DuongDanCV = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    MucLuongMongMuon = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DonViTien = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    TrangThai = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NguonUngTuyen = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DaXem = table.Column<bool>(type: "bit", nullable: false),
                    NgayUngTuyen = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayCapNhat = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TinTuyenDungMaTin = table.Column<int>(type: "int", nullable: true),
                    HoSoUngVienMaHoSo = table.Column<int>(type: "int", nullable: true),
                    NguoiDungMaNguoiDung = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DonUngTuyens", x => x.MaDon);
                    table.ForeignKey(
                        name: "FK_DonUngTuyens_HoSoUngViens_HoSoUngVienMaHoSo",
                        column: x => x.HoSoUngVienMaHoSo,
                        principalTable: "HoSoUngViens",
                        principalColumn: "MaHoSo");
                    table.ForeignKey(
                        name: "FK_DonUngTuyens_NguoiDungs_NguoiDungMaNguoiDung",
                        column: x => x.NguoiDungMaNguoiDung,
                        principalTable: "NguoiDungs",
                        principalColumn: "MaNguoiDung");
                    table.ForeignKey(
                        name: "FK_DonUngTuyens_TinTuyenDungs_TinTuyenDungMaTin",
                        column: x => x.TinTuyenDungMaTin,
                        principalTable: "TinTuyenDungs",
                        principalColumn: "MaTin");
                });

            migrationBuilder.CreateTable(
                name: "KyNangTuyenDungs",
                columns: table => new
                {
                    MaTin = table.Column<int>(type: "int", nullable: false),
                    MaKyNang = table.Column<int>(type: "int", nullable: false),
                    CapDo = table.Column<int>(type: "int", nullable: false),
                    BatBuoc = table.Column<bool>(type: "bit", nullable: false),
                    TinTuyenDungMaTin = table.Column<int>(type: "int", nullable: false),
                    KyNangMaKyNang = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KyNangTuyenDungs", x => new { x.MaTin, x.MaKyNang });
                    table.ForeignKey(
                        name: "FK_KyNangTuyenDungs_KyNangs_KyNangMaKyNang",
                        column: x => x.KyNangMaKyNang,
                        principalTable: "KyNangs",
                        principalColumn: "MaKyNang",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KyNangTuyenDungs_TinTuyenDungs_TinTuyenDungMaTin",
                        column: x => x.TinTuyenDungMaTin,
                        principalTable: "TinTuyenDungs",
                        principalColumn: "MaTin",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TinDaLuus",
                columns: table => new
                {
                    MaNguoiDung = table.Column<int>(type: "int", nullable: false),
                    MaTin = table.Column<int>(type: "int", nullable: false),
                    NgayLuu = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NguoiDungMaNguoiDung = table.Column<int>(type: "int", nullable: false),
                    TinTuyenDungMaTin = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TinDaLuus", x => new { x.MaNguoiDung, x.MaTin });
                    table.ForeignKey(
                        name: "FK_TinDaLuus_NguoiDungs_NguoiDungMaNguoiDung",
                        column: x => x.NguoiDungMaNguoiDung,
                        principalTable: "NguoiDungs",
                        principalColumn: "MaNguoiDung",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TinDaLuus_TinTuyenDungs_TinTuyenDungMaTin",
                        column: x => x.TinTuyenDungMaTin,
                        principalTable: "TinTuyenDungs",
                        principalColumn: "MaTin",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PhongVans",
                columns: table => new
                {
                    MaPhongVan = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaDon = table.Column<int>(type: "int", nullable: true),
                    ThoiGian = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ThoiLuong = table.Column<int>(type: "int", nullable: true),
                    LoaiPhongVan = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DiaDiem = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrangThai = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    GhiChu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NguoiTao = table.Column<int>(type: "int", nullable: true),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayCapNhat = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DonUngTuyenMaDon = table.Column<int>(type: "int", nullable: true),
                    NguoiTaoUserMaNguoiDung = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhongVans", x => x.MaPhongVan);
                    table.ForeignKey(
                        name: "FK_PhongVans_DonUngTuyens_DonUngTuyenMaDon",
                        column: x => x.DonUngTuyenMaDon,
                        principalTable: "DonUngTuyens",
                        principalColumn: "MaDon");
                    table.ForeignKey(
                        name: "FK_PhongVans_NguoiDungs_NguoiTaoUserMaNguoiDung",
                        column: x => x.NguoiTaoUserMaNguoiDung,
                        principalTable: "NguoiDungs",
                        principalColumn: "MaNguoiDung");
                });

            migrationBuilder.CreateTable(
                name: "DanhGiaPhongVans",
                columns: table => new
                {
                    MaDanhGia = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaPhongVan = table.Column<int>(type: "int", nullable: true),
                    DiemDanhGia = table.Column<int>(type: "int", nullable: true),
                    DiemManh = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DiemYeu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GhiChu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeXuat = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NguoiDanhGia = table.Column<int>(type: "int", nullable: true),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PhongVanMaPhongVan = table.Column<int>(type: "int", nullable: true),
                    NguoiDanhGiaUserMaNguoiDung = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DanhGiaPhongVans", x => x.MaDanhGia);
                    table.ForeignKey(
                        name: "FK_DanhGiaPhongVans_NguoiDungs_NguoiDanhGiaUserMaNguoiDung",
                        column: x => x.NguoiDanhGiaUserMaNguoiDung,
                        principalTable: "NguoiDungs",
                        principalColumn: "MaNguoiDung");
                    table.ForeignKey(
                        name: "FK_DanhGiaPhongVans_PhongVans_PhongVanMaPhongVan",
                        column: x => x.PhongVanMaPhongVan,
                        principalTable: "PhongVans",
                        principalColumn: "MaPhongVan");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnhCongTys_MaCongTy",
                table: "AnhCongTys",
                column: "MaCongTy");

            migrationBuilder.CreateIndex(
                name: "IX_ChungChis_HoSoUngVienMaHoSo",
                table: "ChungChis",
                column: "HoSoUngVienMaHoSo");

            migrationBuilder.CreateIndex(
                name: "IX_CongTys_MaNguoiDung",
                table: "CongTys",
                column: "MaNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_DanhGiaPhongVans_NguoiDanhGiaUserMaNguoiDung",
                table: "DanhGiaPhongVans",
                column: "NguoiDanhGiaUserMaNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_DanhGiaPhongVans_PhongVanMaPhongVan",
                table: "DanhGiaPhongVans",
                column: "PhongVanMaPhongVan");

            migrationBuilder.CreateIndex(
                name: "IX_DonUngTuyens_HoSoUngVienMaHoSo",
                table: "DonUngTuyens",
                column: "HoSoUngVienMaHoSo");

            migrationBuilder.CreateIndex(
                name: "IX_DonUngTuyens_NguoiDungMaNguoiDung",
                table: "DonUngTuyens",
                column: "NguoiDungMaNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_DonUngTuyens_TinTuyenDungMaTin",
                table: "DonUngTuyens",
                column: "TinTuyenDungMaTin");

            migrationBuilder.CreateIndex(
                name: "IX_DuAns_HoSoUngVienMaHoSo",
                table: "DuAns",
                column: "HoSoUngVienMaHoSo");

            migrationBuilder.CreateIndex(
                name: "IX_HocVans_HoSoUngVienMaHoSo",
                table: "HocVans",
                column: "HoSoUngVienMaHoSo");

            migrationBuilder.CreateIndex(
                name: "IX_HoSoUngViens_MaNguoiDung",
                table: "HoSoUngViens",
                column: "MaNguoiDung",
                unique: true,
                filter: "[MaNguoiDung] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_KinhNghiems_HoSoUngVienMaHoSo",
                table: "KinhNghiems",
                column: "HoSoUngVienMaHoSo");

            migrationBuilder.CreateIndex(
                name: "IX_KyNangTuyenDungs_KyNangMaKyNang",
                table: "KyNangTuyenDungs",
                column: "KyNangMaKyNang");

            migrationBuilder.CreateIndex(
                name: "IX_KyNangTuyenDungs_TinTuyenDungMaTin",
                table: "KyNangTuyenDungs",
                column: "TinTuyenDungMaTin");

            migrationBuilder.CreateIndex(
                name: "IX_KyNangUngViens_HoSoUngVienMaHoSo",
                table: "KyNangUngViens",
                column: "HoSoUngVienMaHoSo");

            migrationBuilder.CreateIndex(
                name: "IX_KyNangUngViens_KyNangMaKyNang",
                table: "KyNangUngViens",
                column: "KyNangMaKyNang");

            migrationBuilder.CreateIndex(
                name: "IX_NguoiDungs_MaVaiTro",
                table: "NguoiDungs",
                column: "MaVaiTro");

            migrationBuilder.CreateIndex(
                name: "IX_PhongVans_DonUngTuyenMaDon",
                table: "PhongVans",
                column: "DonUngTuyenMaDon");

            migrationBuilder.CreateIndex(
                name: "IX_PhongVans_NguoiTaoUserMaNguoiDung",
                table: "PhongVans",
                column: "NguoiTaoUserMaNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_ThongBaos_MaNguoiDung",
                table: "ThongBaos",
                column: "MaNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_TinDaLuus_NguoiDungMaNguoiDung",
                table: "TinDaLuus",
                column: "NguoiDungMaNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_TinDaLuus_TinTuyenDungMaTin",
                table: "TinDaLuus",
                column: "TinTuyenDungMaTin");

            migrationBuilder.CreateIndex(
                name: "IX_TinTuyenDungs_CapBacCongViecMaCapBac",
                table: "TinTuyenDungs",
                column: "CapBacCongViecMaCapBac");

            migrationBuilder.CreateIndex(
                name: "IX_TinTuyenDungs_CongTyMaCongTy",
                table: "TinTuyenDungs",
                column: "CongTyMaCongTy");

            migrationBuilder.CreateIndex(
                name: "IX_TinTuyenDungs_DanhMucCongViecMaDanhMuc",
                table: "TinTuyenDungs",
                column: "DanhMucCongViecMaDanhMuc");

            migrationBuilder.CreateIndex(
                name: "IX_TinTuyenDungs_LoaiCongViecMaLoai",
                table: "TinTuyenDungs",
                column: "LoaiCongViecMaLoai");

            migrationBuilder.CreateIndex(
                name: "IX_TinTuyenDungs_NguoiDungMaNguoiDung",
                table: "TinTuyenDungs",
                column: "NguoiDungMaNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_TinTuyenDungs_NguoiTaoUserMaNguoiDung",
                table: "TinTuyenDungs",
                column: "NguoiTaoUserMaNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_UngVienDaLuus_HoSoUngVienMaHoSo",
                table: "UngVienDaLuus",
                column: "HoSoUngVienMaHoSo");

            migrationBuilder.CreateIndex(
                name: "IX_UngVienDaLuus_NguoiDungMaNguoiDung",
                table: "UngVienDaLuus",
                column: "NguoiDungMaNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_VaiTroQuyenHans_MaQuyen",
                table: "VaiTroQuyenHans",
                column: "MaQuyen");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnhCongTys");

            migrationBuilder.DropTable(
                name: "ChungChis");

            migrationBuilder.DropTable(
                name: "DanhGiaPhongVans");

            migrationBuilder.DropTable(
                name: "DuAns");

            migrationBuilder.DropTable(
                name: "HocVans");

            migrationBuilder.DropTable(
                name: "KinhNghiems");

            migrationBuilder.DropTable(
                name: "KyNangTuyenDungs");

            migrationBuilder.DropTable(
                name: "KyNangUngViens");

            migrationBuilder.DropTable(
                name: "ThongBaos");

            migrationBuilder.DropTable(
                name: "TinDaLuus");

            migrationBuilder.DropTable(
                name: "UngVienDaLuus");

            migrationBuilder.DropTable(
                name: "VaiTroQuyenHans");

            migrationBuilder.DropTable(
                name: "PhongVans");

            migrationBuilder.DropTable(
                name: "KyNangs");

            migrationBuilder.DropTable(
                name: "QuyenHans");

            migrationBuilder.DropTable(
                name: "DonUngTuyens");

            migrationBuilder.DropTable(
                name: "HoSoUngViens");

            migrationBuilder.DropTable(
                name: "TinTuyenDungs");

            migrationBuilder.DropTable(
                name: "CapBacCongViecs");

            migrationBuilder.DropTable(
                name: "CongTys");

            migrationBuilder.DropTable(
                name: "DanhMucCongViecs");

            migrationBuilder.DropTable(
                name: "LoaiCongViecs");

            migrationBuilder.DropTable(
                name: "NguoiDungs");

            migrationBuilder.DropTable(
                name: "VaiTros");
        }
    }
}
