using System.ComponentModel.DataAnnotations;

namespace HeThongTuyenDung.Models
{
    public class CapBacCongViec
    {
        [Key]
        public int MaCapBac { get; set; }
        
        [Required]
        [StringLength(100)]
        public string TenCapBac { get; set; } = string.Empty;
        
        [StringLength(255)]
        public string? MoTa { get; set; }

        // Navigation properties
        public virtual ICollection<TinTuyenDung> TinTuyenDungs { get; set; } = new List<TinTuyenDung>();
    }
} 