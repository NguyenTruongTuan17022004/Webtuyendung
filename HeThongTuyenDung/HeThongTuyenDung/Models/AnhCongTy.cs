using System.ComponentModel.DataAnnotations;

namespace HeThongTuyenDung.Models
{
    public class AnhCongTy
    {
        [Key]
        public int MaAnh { get; set; }
        
        public int? MaCongTy { get; set; }
        
        [Required]
        [StringLength(255)]
        public string DuongDanAnh { get; set; } = string.Empty;
        
        [StringLength(255)]
        public string? MoTaAnh { get; set; }
        
        public int ThuTu { get; set; } = 0;
        
        public DateTime NgayTao { get; set; } = DateTime.Now;

        // Navigation properties
        public virtual CongTy? CongTy { get; set; }
    }
} 