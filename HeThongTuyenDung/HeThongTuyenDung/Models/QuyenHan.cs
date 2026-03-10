using System.ComponentModel.DataAnnotations;

namespace HeThongTuyenDung.Models
{
    public class QuyenHan
    {
        [Key]
        public int MaQuyen { get; set; }
        
        [Required]
        [StringLength(100)]
        public string TenQuyen { get; set; } = string.Empty;
        
        [StringLength(255)]
        public string? MoTa { get; set; }

        // Navigation properties
        public virtual ICollection<VaiTroQuyenHan> VaiTroQuyenHans { get; set; } = new List<VaiTroQuyenHan>();
    }
} 