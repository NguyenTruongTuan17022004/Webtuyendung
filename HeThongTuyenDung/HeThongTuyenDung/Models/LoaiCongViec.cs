using System.ComponentModel.DataAnnotations;

namespace HeThongTuyenDung.Models
{
    public class LoaiCongViec
    {
        [Key]
        public int MaLoai { get; set; }
        
        [Required]
        [StringLength(100)]
        public string TenLoai { get; set; } = string.Empty;
        
        [StringLength(255)]
        public string? MoTa { get; set; }

        // Navigation properties
        public virtual ICollection<TinTuyenDung> TinTuyenDungs { get; set; } = new List<TinTuyenDung>();
    }
} 