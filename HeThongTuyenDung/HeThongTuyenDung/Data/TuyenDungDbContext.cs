using Microsoft.EntityFrameworkCore;
using HeThongTuyenDung.Models;

namespace HeThongTuyenDung.Data
{
    public class TuyenDungDbContext : DbContext
    {
        public TuyenDungDbContext(DbContextOptions<TuyenDungDbContext> options)
            : base(options)
        {
        }

        // DbSets for all entities
        public DbSet<VaiTro> VaiTros { get; set; }
        public DbSet<QuyenHan> QuyenHans { get; set; }
        public DbSet<VaiTroQuyenHan> VaiTroQuyenHans { get; set; }
        public DbSet<NguoiDung> NguoiDungs { get; set; }
        public DbSet<CongTy> CongTys { get; set; }
        public DbSet<AnhCongTy> AnhCongTys { get; set; }
        public DbSet<CapBacCongViec> CapBacCongViecs { get; set; }
        public DbSet<LoaiCongViec> LoaiCongViecs { get; set; }
        public DbSet<DanhMucCongViec> DanhMucCongViecs { get; set; }
        public DbSet<KyNang> KyNangs { get; set; }
        public DbSet<TinTuyenDung> TinTuyenDungs { get; set; }
        public DbSet<KyNangTuyenDung> KyNangTuyenDungs { get; set; }
        public DbSet<HoSoUngVien> HoSoUngViens { get; set; }
        public DbSet<KyNangUngVien> KyNangUngViens { get; set; }
        public DbSet<HocVan> HocVans { get; set; }
        public DbSet<KinhNghiem> KinhNghiems { get; set; }
        public DbSet<ChungChi> ChungChis { get; set; }
        public DbSet<DuAn> DuAns { get; set; }
        public DbSet<DonUngTuyen> DonUngTuyens { get; set; }
        public DbSet<PhongVan> PhongVans { get; set; }
        public DbSet<DanhGiaPhongVan> DanhGiaPhongVans { get; set; }
        public DbSet<ThongBao> ThongBaos { get; set; }
        public DbSet<TinDaLuu> TinDaLuus { get; set; }
        public DbSet<UngVienDaLuu> UngVienDaLuus { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure composite keys
            modelBuilder.Entity<VaiTroQuyenHan>()
                .HasKey(vq => new { vq.MaVaiTro, vq.MaQuyen });

            modelBuilder.Entity<KyNangTuyenDung>()
                .HasKey(kt => new { kt.MaTin, kt.MaKyNang });

            modelBuilder.Entity<KyNangUngVien>()
                .HasKey(ku => new { ku.MaHoSo, ku.MaKyNang });

            modelBuilder.Entity<TinDaLuu>()
                .HasKey(td => new { td.MaNguoiDung, td.MaTin });

            modelBuilder.Entity<UngVienDaLuu>()
                .HasKey(ud => new { ud.MaNguoiDung, ud.MaHoSo });

            // Add unique constraint for DuongDan in TinTuyenDung
            modelBuilder.Entity<TinTuyenDung>()
                .HasIndex(t => t.DuongDan)
                .IsUnique();

            // Configure basic relationships only - let EF Core handle the rest automatically
            modelBuilder.Entity<VaiTroQuyenHan>()
                .HasOne(vq => vq.VaiTro)
                .WithMany(v => v.VaiTroQuyenHans)
                .HasForeignKey(vq => vq.MaVaiTro);

            modelBuilder.Entity<VaiTroQuyenHan>()
                .HasOne(vq => vq.QuyenHan)
                .WithMany(q => q.VaiTroQuyenHans)
                .HasForeignKey(vq => vq.MaQuyen);

            modelBuilder.Entity<NguoiDung>()
                .HasOne(n => n.VaiTro)
                .WithMany(v => v.NguoiDungs)
                .HasForeignKey(n => n.MaVaiTro);

            modelBuilder.Entity<CongTy>()
                .HasOne(c => c.NguoiDung)
                .WithMany(n => n.CongTys)
                .HasForeignKey(c => c.MaNguoiDung);

            modelBuilder.Entity<AnhCongTy>()
                .HasOne(a => a.CongTy)
                .WithMany(c => c.AnhCongTys)
                .HasForeignKey(a => a.MaCongTy);

            modelBuilder.Entity<HoSoUngVien>()
                .HasOne(h => h.NguoiDung)
                .WithOne(n => n.HoSoUngVien)
                .HasForeignKey<HoSoUngVien>(h => h.MaNguoiDung);

            modelBuilder.Entity<ThongBao>()
                .HasOne(t => t.NguoiDung)
                .WithMany(n => n.ThongBaos)
                .HasForeignKey(t => t.MaNguoiDung);
        }
    }
} 