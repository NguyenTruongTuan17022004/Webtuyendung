using System.ComponentModel.DataAnnotations;

namespace HeThongTuyenDung.Models
{
    public class DuAn
    {
        [Key]
        public int MaDuAn { get; set; }
        
        public int? MaHoSo { get; set; }
        
        [Required]
        [StringLength(200)]
        public string TenDuAn { get; set; } = string.Empty;
        
        [StringLength(200)]
        public string? VaiTro { get; set; }
        
        public DateTime? NgayBatDau { get; set; }
        
        public DateTime? NgayKetThuc { get; set; }
        
        public bool DangThucHien { get; set; } = false;
        
        public string? MoTa { get; set; }
        
        [StringLength(500)]
        public string? CongNgheSuDung { get; set; }
        
        [StringLength(255)]
        public string? LienKetDuAn { get; set; }
        
        public DateTime NgayTao { get; set; } = DateTime.Now;

        // Navigation properties
        public virtual HoSoUngVien? HoSoUngVien { get; set; }
    }
} 