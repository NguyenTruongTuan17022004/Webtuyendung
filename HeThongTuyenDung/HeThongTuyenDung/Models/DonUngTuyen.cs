using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HeThongTuyenDung.Models
{
    public class DonUngTuyen
    {
        [Key]
        public int MaDon { get; set; }
        
        public int? MaTin { get; set; }
        
        public int? MaHoSo { get; set; }
        
        public int? MaNguoiDung { get; set; }
        
        [StringLength(100)]
        public string? HoTen { get; set; }
        
        [StringLength(100)]
        [EmailAddress]
        public string? Email { get; set; }
        
        [StringLength(20)]
        public string? SoDienThoai { get; set; }
        
        [StringLength(255)]
        public string? DiaChi { get; set; }
        
        [StringLength(50)]
        public string? KinhNghiem { get; set; }
        
        [StringLength(50)]
        public string? HocVan { get; set; }
        
        [StringLength(500)]
        public string? MucTieu { get; set; }
        
        [StringLength(500)]
        public string? KyNang { get; set; }
        
        [StringLength(1000)]
        public string? KinhNghiemLamViec { get; set; }
        
        [StringLength(500)]
        public string? DuAn { get; set; }
        
        [StringLength(1000)]
        public string? LyDoUngTuyen { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal? LuongMongMuon { get; set; }
        
        public DateTime? ThoiGianBatDau { get; set; }
        
        [StringLength(500)]
        public string? ThongTinBoSung { get; set; }
        
        [StringLength(500)]
        public string? GhiChu { get; set; }
        
        public string? ThuNgo { get; set; }
        
        [StringLength(255)]
        public string? DuongDanCV { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal? MucLuongMongMuon { get; set; }
        
        [StringLength(10)]
        public string DonViTien { get; set; } = "VND";
        
        [StringLength(50)]
        public string TrangThai { get; set; } = "Chờ duyệt";
        
        [StringLength(50)]
        public string? NguonUngTuyen { get; set; }
        
        public bool DaXem { get; set; } = false;
        
        public DateTime NgayUngTuyen { get; set; } = DateTime.Now;
        
        public DateTime NgayCapNhat { get; set; } = DateTime.Now;

        // Navigation properties
        [ForeignKey("MaTin")]
        public virtual TinTuyenDung? TinTuyenDung { get; set; }
        
        [ForeignKey("MaHoSo")]
        public virtual HoSoUngVien? HoSoUngVien { get; set; }
        
        [ForeignKey("MaNguoiDung")]
        public virtual NguoiDung? NguoiDung { get; set; }
        
        public virtual ICollection<PhongVan> PhongVans { get; set; } = new List<PhongVan>();
    }
} 