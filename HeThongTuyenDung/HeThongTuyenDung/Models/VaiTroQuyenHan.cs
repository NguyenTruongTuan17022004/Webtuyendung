namespace HeThongTuyenDung.Models
{
    public class VaiTroQuyenHan
    {
        public int MaVaiTro { get; set; }
        public int MaQuyen { get; set; }

        // Navigation properties
        public virtual VaiTro VaiTro { get; set; } = null!;
        public virtual QuyenHan QuyenHan { get; set; } = null!;
    }
} 