using System.ComponentModel.DataAnnotations;

namespace HeThongTuyenDung.Models
{
    public class ApplicationFormModel
    {
        [Required(ErrorMessage = "Họ và tên là bắt buộc")]
        [StringLength(100, ErrorMessage = "Họ và tên không được quá 100 ký tự")]
        public string HoTen { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email là bắt buộc")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [StringLength(100, ErrorMessage = "Email không được quá 100 ký tự")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Số điện thoại là bắt buộc")]
        [StringLength(20, ErrorMessage = "Số điện thoại không được quá 20 ký tự")]
        public string SoDienThoai { get; set; } = string.Empty;

        [StringLength(255, ErrorMessage = "Địa chỉ không được quá 255 ký tự")]
        public string? DiaChi { get; set; }

        [StringLength(50, ErrorMessage = "Kinh nghiệm không được quá 50 ký tự")]
        public string? KinhNghiem { get; set; }

        [StringLength(50, ErrorMessage = "Học vấn không được quá 50 ký tự")]
        public string? HocVan { get; set; }

        [StringLength(500, ErrorMessage = "Mục tiêu nghề nghiệp không được quá 500 ký tự")]
        public string? MucTieu { get; set; }

        [StringLength(500, ErrorMessage = "Kỹ năng không được quá 500 ký tự")]
        public string? KyNang { get; set; }

        [StringLength(1000, ErrorMessage = "Kinh nghiệm làm việc không được quá 1000 ký tự")]
        public string? KinhNghiemLamViec { get; set; }

        [StringLength(500, ErrorMessage = "Dự án không được quá 500 ký tự")]
        public string? DuAn { get; set; }

        [Required(ErrorMessage = "Lý do ứng tuyển là bắt buộc")]
        [StringLength(1000, ErrorMessage = "Lý do ứng tuyển không được quá 1000 ký tự")]
        public string LyDoUngTuyen { get; set; } = string.Empty;

        [Range(0, double.MaxValue, ErrorMessage = "Mức lương mong muốn phải là số dương")]
        public decimal? LuongMongMuon { get; set; }

        public DateTime? ThoiGianBatDau { get; set; }

        [StringLength(500, ErrorMessage = "Thông tin bổ sung không được quá 500 ký tự")]
        public string? ThongTinBoSung { get; set; }

        public IFormFile? FileCV { get; set; }
    }
} 