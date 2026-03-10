using System.ComponentModel.DataAnnotations;

namespace HeThongTuyenDung.Models
{
    public class DanhMucCongViec
    {
        [Key]
        public int MaDanhMuc { get; set; }
        
        [Required]
        [StringLength(100)]
        public string TenDanhMuc { get; set; } = string.Empty;
        
        [StringLength(255)]
        public string? MoTa { get; set; }
        
        [StringLength(255)]
        public string? BieuTuong { get; set; }
        
        public int ThuTu { get; set; } = 0;
        
        public bool TrangThai { get; set; } = true;

        // Navigation properties
        public virtual ICollection<TinTuyenDung> TinTuyenDungs { get; set; } = new List<TinTuyenDung>();
    }
} 