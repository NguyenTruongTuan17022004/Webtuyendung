using System.ComponentModel.DataAnnotations;

namespace HeThongTuyenDung.Models
{
    public class KyNang
    {
        [Key]
        public int MaKyNang { get; set; }
        
        [Required]
        [StringLength(100)]
        public string TenKyNang { get; set; } = string.Empty;
        
        [StringLength(255)]
        public string? MoTa { get; set; }
        
        [StringLength(50)]
        public string? LoaiKyNang { get; set; }
        
        [StringLength(255)]
        public string? BieuTuong { get; set; }
        
        public bool TrangThai { get; set; } = true;

        // Navigation properties
        public virtual ICollection<KyNangTuyenDung> KyNangTuyenDungs { get; set; } = new List<KyNangTuyenDung>();
        public virtual ICollection<KyNangUngVien> KyNangUngViens { get; set; } = new List<KyNangUngVien>();
    }
} 