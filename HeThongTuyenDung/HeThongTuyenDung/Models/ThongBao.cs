using System.ComponentModel.DataAnnotations;

namespace HeThongTuyenDung.Models
{
    public class ThongBao
    {
        [Key]
        public int MaThongBao { get; set; }
        
        public int? MaNguoiDung { get; set; }
        
        [StringLength(200)]
        public string? TieuDe { get; set; }
        
        public string? NoiDung { get; set; }
        
        [StringLength(50)]
        public string? LoaiThongBao { get; set; }
        
        public int? MaLienQuan { get; set; }
        
        public bool DaXem { get; set; } = false;
        
        public DateTime NgayTao { get; set; } = DateTime.Now;

        // Navigation properties
        public virtual NguoiDung? NguoiDung { get; set; }
    }
} 