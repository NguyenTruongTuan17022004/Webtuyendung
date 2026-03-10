using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HeThongTuyenDung.Models
{
    public class TinTuyenDung
    {
        [Key]
        public int MaTin { get; set; }

        [Required]
        [StringLength(200)]
        public string TieuDe { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string DuongDan { get; set; } = string.Empty;

        public int? MaDanhMuc { get; set; }

        public int? MaCapBac { get; set; }

        public int? MaLoai { get; set; }

        public string? MoTa { get; set; }

        public string? YeuCau { get; set; }

        public string? QuyenLoi { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? LuongTu { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? LuongDen { get; set; }

        public bool HienThiLuong { get; set; } = true;

        [StringLength(10)]
        public string DonViTien { get; set; } = "VND";

        public int? KinhNghiemTu { get; set; }

        public int? KinhNghiemDen { get; set; }

        public int SoLuong { get; set; } = 1;

        [StringLength(20)]
        public string? GioiTinh { get; set; }

        [StringLength(50)]
        public string? DoTuoi { get; set; }

        public string? DiaDiem { get; set; }

        public string? ThanhPho { get; set; }

        [StringLength(100)]
        public string QuocGia { get; set; } = "Việt Nam";

        public string? DiaChiLamViec { get; set; }

        public DateTime? HanNop { get; set; }

        public bool TrangThai { get; set; } = true;

        public bool NoiBat { get; set; } = false;

        public bool GapTuyen { get; set; } = false;

        public int LuotXem { get; set; } = 0;

        public int? NguoiTao { get; set; }

        public DateTime NgayTao { get; set; } = DateTime.Now;

        public DateTime NgayCapNhat { get; set; } = DateTime.Now;

        public int? MaNguoiDung { get; set; }

        public int? MaCongTy { get; set; }

        // Navigation properties
        public virtual CongTy? CongTy { get; set; }
        public virtual DanhMucCongViec? DanhMucCongViec { get; set; }
        public virtual CapBacCongViec? CapBacCongViec { get; set; }
        public virtual LoaiCongViec? LoaiCongViec { get; set; }
        public virtual NguoiDung? NguoiTaoUser { get; set; }
        public virtual NguoiDung? NguoiDung { get; set; }
        public virtual ICollection<KyNangTuyenDung> KyNangTuyenDungs { get; set; } = new List<KyNangTuyenDung>();
        public virtual ICollection<DonUngTuyen> DonUngTuyens { get; set; } = new List<DonUngTuyen>();
        public virtual ICollection<TinDaLuu> TinDaLuus { get; set; } = new List<TinDaLuu>();
    }
}
