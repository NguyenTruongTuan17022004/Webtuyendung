using System.ComponentModel.DataAnnotations;

namespace HeThongTuyenDung.Models
{
    public class CongTy
    {
        [Key]
        public int MaCongTy { get; set; }
        
        [Required]
        [StringLength(200)]
        public string TenCongTy { get; set; } = string.Empty;
        
        [StringLength(255)]
        public string? Logo { get; set; }
        
        [StringLength(255)]
        public string? AnhBia { get; set; }
        
        public string? MoTa { get; set; }
        
        [StringLength(500)]
        public string? MoTaNgan { get; set; }
        
        [StringLength(255)]
        public string? DiaChi { get; set; }
        
        [StringLength(100)]
        public string? ThanhPho { get; set; }
        
        [StringLength(100)]
        public string QuocGia { get; set; } = "Việt Nam";
        
        [StringLength(20)]
        public string? DienThoai { get; set; }
        
        [StringLength(100)]
        [EmailAddress]
        public string? Email { get; set; }
        
        [StringLength(200)]
        public string? Website { get; set; }
        
        [StringLength(50)]
        public string? QuyMo { get; set; }
        
        [StringLength(100)]
        public string? LinhVuc { get; set; }
        
        [StringLength(50)]
        public string? MaSoThue { get; set; }
        
        public bool DaXacThuc { get; set; } = false;
        
        public bool TrangThai { get; set; } = true;
        
        public int? MaNguoiDung { get; set; }
        
        public DateTime NgayTao { get; set; } = DateTime.Now;
        
        public DateTime NgayCapNhat { get; set; } = DateTime.Now;

        // Navigation properties
        public virtual NguoiDung? NguoiDung { get; set; }
        public virtual ICollection<AnhCongTy> AnhCongTys { get; set; } = new List<AnhCongTy>();
        public virtual ICollection<TinTuyenDung> TinTuyenDungs { get; set; } = new List<TinTuyenDung>();
    }
} 