using System.ComponentModel.DataAnnotations;

namespace HeThongTuyenDung.Models
{
    public class TinDaLuu
    {
        public int MaNguoiDung { get; set; }
        public int MaTin { get; set; }
        
        public DateTime NgayLuu { get; set; } = DateTime.Now;

        // Navigation properties
        public virtual NguoiDung NguoiDung { get; set; } = null!;
        public virtual TinTuyenDung TinTuyenDung { get; set; } = null!;
    }
} 