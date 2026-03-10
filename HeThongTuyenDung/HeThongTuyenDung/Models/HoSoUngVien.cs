using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HeThongTuyenDung.Models
{
    public class HoSoUngVien
    {
        [Key]
        public int MaHoSo { get; set; }
        
        public int? MaNguoiDung { get; set; }
        
        [StringLength(200)]
        public string? ViTriMongMuon { get; set; }
        
        public string? GioiThieu { get; set; }
        
        public int? SoNamKinhNghiem { get; set; }
        
        [StringLength(200)]
        public string? CongViecHienTai { get; set; }
        
        [StringLength(200)]
        public string? CongTyHienTai { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal? MucLuongHienTai { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal? MucLuongMongMuon { get; set; }
        
        [StringLength(10)]
        public string DonViTien { get; set; } = "VND";
        
        [StringLength(255)]
        public string? DiaDiemMongMuon { get; set; }
        
        [StringLength(50)]
        public string? LoaiCongViecMongMuon { get; set; }
        
        public bool CoTheNuocNgoai { get; set; } = false;
        
        public bool DangTimViec { get; set; } = true;
        
        public bool CongKhaiHoSo { get; set; } = false;
        
        public int LuotXem { get; set; } = 0;
        
        public DateTime NgayTao { get; set; } = DateTime.Now;
        
        public DateTime NgayCapNhat { get; set; } = DateTime.Now;

        // Navigation properties
        public virtual NguoiDung? NguoiDung { get; set; }
        public virtual ICollection<KyNangUngVien> KyNangUngViens { get; set; } = new List<KyNangUngVien>();
        public virtual ICollection<HocVan> HocVans { get; set; } = new List<HocVan>();
        public virtual ICollection<KinhNghiem> KinhNghiems { get; set; } = new List<KinhNghiem>();
        public virtual ICollection<ChungChi> ChungChis { get; set; } = new List<ChungChi>();
        public virtual ICollection<DuAn> DuAns { get; set; } = new List<DuAn>();
        public virtual ICollection<DonUngTuyen> DonUngTuyens { get; set; } = new List<DonUngTuyen>();
        public virtual ICollection<UngVienDaLuu> UngVienDaLuus { get; set; } = new List<UngVienDaLuu>();
    }
} 