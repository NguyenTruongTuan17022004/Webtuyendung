using System.ComponentModel.DataAnnotations;

namespace HeThongTuyenDung.Models
{
    public class NguoiDung
    {
        [Key]
        public int MaNguoiDung { get; set; }
        
        [Required]
        [StringLength(50)]
        public string TenDangNhap { get; set; } = string.Empty;
        
        [Required]
        [StringLength(255)]
        public string MatKhau { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string HoTen { get; set; } = string.Empty;
        
        [StringLength(20)]
        public string? SoDienThoai { get; set; }
        
        [StringLength(255)]
        public string? AnhDaiDien { get; set; }
        
        public int MaVaiTro { get; set; }
        
        public bool TrangThai { get; set; } = true;
        
        public DateTime? LanDangNhapCuoi { get; set; }
        
        public DateTime NgayTao { get; set; } = DateTime.Now;
        
        public DateTime NgayCapNhat { get; set; } = DateTime.Now;

        // Navigation properties
        public virtual VaiTro VaiTro { get; set; } = null!;
        public virtual HoSoUngVien? HoSoUngVien { get; set; }
        public virtual ICollection<CongTy> CongTys { get; set; } = new List<CongTy>();
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public virtual ICollection<TinTuyenDung> TinTuyenDungsTao { get; set; } = new List<TinTuyenDung>();
        public virtual ICollection<PhongVan> PhongVansTao { get; set; } = new List<PhongVan>();
        public virtual ICollection<DanhGiaPhongVan> DanhGiaPhongVans { get; set; } = new List<DanhGiaPhongVan>();
        public virtual ICollection<ThongBao> ThongBaos { get; set; } = new List<ThongBao>();
        public virtual ICollection<TinDaLuu> TinDaLuus { get; set; } = new List<TinDaLuu>();
        public virtual ICollection<UngVienDaLuu> UngVienDaLuus { get; set; } = new List<UngVienDaLuu>();
    }
} 