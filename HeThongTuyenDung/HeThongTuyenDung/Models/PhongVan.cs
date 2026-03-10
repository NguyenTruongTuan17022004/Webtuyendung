using System.ComponentModel.DataAnnotations;

namespace HeThongTuyenDung.Models
{
    public class PhongVan
    {
        [Key]
        public int MaPhongVan { get; set; }
        
        public int? MaDon { get; set; }
        
        [Required]
        public DateTime ThoiGian { get; set; }
        
        public int? ThoiLuong { get; set; }
        
        [StringLength(50)]
        public string? LoaiPhongVan { get; set; }
        
        [StringLength(255)]
        public string? DiaDiem { get; set; }
        
        public string? MoTa { get; set; }
        
        [StringLength(50)]
        public string TrangThai { get; set; } = "Đã lên lịch";
        
        public string? GhiChu { get; set; }
        
        public int? NguoiTao { get; set; }
        
        public DateTime NgayTao { get; set; } = DateTime.Now;
        
        public DateTime NgayCapNhat { get; set; } = DateTime.Now;

        // Navigation properties
        public virtual DonUngTuyen? DonUngTuyen { get; set; }
        public virtual NguoiDung? NguoiTaoUser { get; set; }
        public virtual ICollection<DanhGiaPhongVan> DanhGiaPhongVans { get; set; } = new List<DanhGiaPhongVan>();
    }
} 