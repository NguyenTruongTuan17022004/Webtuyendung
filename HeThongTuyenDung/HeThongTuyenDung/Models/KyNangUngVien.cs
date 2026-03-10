using System.ComponentModel.DataAnnotations;

namespace HeThongTuyenDung.Models
{
    public class KyNangUngVien
    {
        public int MaHoSo { get; set; }
        public int MaKyNang { get; set; }
        
        [Range(1, 5)]
        public int CapDo { get; set; }
        
        public int? SoNamKinhNghiem { get; set; }
        
        [StringLength(500)]
        public string? MoTa { get; set; }

        // Navigation properties
        public virtual HoSoUngVien HoSoUngVien { get; set; } = null!;
        public virtual KyNang KyNang { get; set; } = null!;
    }
} 