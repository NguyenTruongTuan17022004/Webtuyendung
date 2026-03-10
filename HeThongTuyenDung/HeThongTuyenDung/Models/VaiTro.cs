using System.ComponentModel.DataAnnotations;

namespace HeThongTuyenDung.Models
{
    public class VaiTro
    {
        [Key]
        public int MaVaiTro { get; set; }
        
        [Required]
        [StringLength(50)]
        public string TenVaiTro { get; set; } = string.Empty;
        
        [StringLength(255)]
        public string? MoTa { get; set; }

        // Navigation properties
        public virtual ICollection<NguoiDung> NguoiDungs { get; set; } = new List<NguoiDung>();
        public virtual ICollection<VaiTroQuyenHan> VaiTroQuyenHans { get; set; } = new List<VaiTroQuyenHan>();
    }
} 