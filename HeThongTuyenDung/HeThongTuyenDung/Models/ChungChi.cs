using System.ComponentModel.DataAnnotations;

namespace HeThongTuyenDung.Models
{
    public class ChungChi
    {
        [Key]
        public int MaChungChi { get; set; }
        
        public int? MaHoSo { get; set; }
        
        [Required]
        [StringLength(200)]
        public string TenChungChi { get; set; } = string.Empty;
        
        [StringLength(200)]
        public string? DonViCap { get; set; }
        
        public DateTime? NgayCap { get; set; }
        
        public DateTime? NgayHetHan { get; set; }

        // Navigation properties
        public virtual HoSoUngVien? HoSoUngVien { get; set; }
    }
} 