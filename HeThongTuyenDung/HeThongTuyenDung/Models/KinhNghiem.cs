using System.ComponentModel.DataAnnotations;

namespace HeThongTuyenDung.Models
{
    public class KinhNghiem
    {
        [Key]
        public int MaKinhNghiem { get; set; }
        
        public int? MaHoSo { get; set; }
        
        [Required]
        [StringLength(200)]
        public string TenCongTy { get; set; } = string.Empty;
        
        [Required]
        [StringLength(200)]
        public string ChucVu { get; set; } = string.Empty;
        
        public DateTime? NgayBatDau { get; set; }
        
        public DateTime? NgayKetThuc { get; set; }
        
        public bool DangLamViec { get; set; } = false;
        
        public string? MoTa { get; set; }

        // Navigation properties
        public virtual HoSoUngVien? HoSoUngVien { get; set; }
    }
} 