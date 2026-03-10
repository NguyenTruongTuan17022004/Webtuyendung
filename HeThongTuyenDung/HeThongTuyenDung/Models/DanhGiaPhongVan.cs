using System.ComponentModel.DataAnnotations;

namespace HeThongTuyenDung.Models
{
    public class DanhGiaPhongVan
    {
        [Key]
        public int MaDanhGia { get; set; }
        
        public int? MaPhongVan { get; set; }
        
        [Range(1, 5)]
        public int? DiemDanhGia { get; set; }
        
        public string? DiemManh { get; set; }
        
        public string? DiemYeu { get; set; }
        
        public string? GhiChu { get; set; }
        
        [StringLength(50)]
        public string? DeXuat { get; set; }
        
        public int? NguoiDanhGia { get; set; }
        
        public DateTime NgayTao { get; set; } = DateTime.Now;

        // Navigation properties
        public virtual PhongVan? PhongVan { get; set; }
        public virtual NguoiDung? NguoiDanhGiaUser { get; set; }
    }
} 