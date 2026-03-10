using System.ComponentModel.DataAnnotations;

namespace HeThongTuyenDung.Models
{
    public class HocVan
    {
        [Key]
        public int MaHocVan { get; set; }
        
        public int? MaHoSo { get; set; }
        
        [Required]
        [StringLength(200)]
        public string TenTruong { get; set; } = string.Empty;
        
        [StringLength(200)]
        public string? BangCap { get; set; }
        
        [StringLength(200)]
        public string? ChuyenNganh { get; set; }
        
        public DateTime? NgayBatDau { get; set; }
        
        public DateTime? NgayKetThuc { get; set; }
        
        public bool DangHoc { get; set; } = false;
        
        public string? MoTa { get; set; }

        // Navigation properties
        public virtual HoSoUngVien? HoSoUngVien { get; set; }
    }
} 