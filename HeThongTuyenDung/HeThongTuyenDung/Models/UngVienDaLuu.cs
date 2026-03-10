using System.ComponentModel.DataAnnotations;

namespace HeThongTuyenDung.Models
{
    public class UngVienDaLuu
    {
        public int MaNguoiDung { get; set; }
        public int MaHoSo { get; set; }
        
        public DateTime NgayLuu { get; set; } = DateTime.Now;

        // Navigation properties
        public virtual NguoiDung NguoiDung { get; set; } = null!;
        public virtual HoSoUngVien HoSoUngVien { get; set; } = null!;
    }
} 