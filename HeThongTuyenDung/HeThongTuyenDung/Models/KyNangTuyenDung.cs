using System.ComponentModel.DataAnnotations;

namespace HeThongTuyenDung.Models
{
    public class KyNangTuyenDung
    {
        public int MaTin { get; set; }
        public int MaKyNang { get; set; }
        
        [Range(1, 5)]
        public int CapDo { get; set; }
        
        public bool BatBuoc { get; set; } = true;

        // Navigation properties
        public virtual TinTuyenDung TinTuyenDung { get; set; } = null!;
        public virtual KyNang KyNang { get; set; } = null!;
    }
} 