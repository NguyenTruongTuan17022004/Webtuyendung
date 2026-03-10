# Hướng dẫn Test LTW

## Vấn đề hiện tại
Sau khi đăng tin tuyển dụng, tin vẫn không được update ra trang chủ.

## Các bước test và khắc phục

### 1. Truy cập trang Test
- Mở trình duyệt và truy cập: `https://localhost:7001/Test`
- Hoặc: `http://localhost:5001/Test`

### 2. Kiểm tra trạng thái hệ thống
- Xem thông tin kết nối database
- Kiểm tra số lượng dữ liệu hiện tại (vai trò, người dùng, công ty, tin tuyển dụng)

### 3. Tạo dữ liệu cơ bản (nếu cần)
- Nhấn nút **"Tạo dữ liệu cơ bản"** nếu chưa có dữ liệu
- Hệ thống sẽ tạo:
  - Vai trò (Admin, Nhà tuyển dụng, Ứng viên)
  - Danh mục công việc (Công nghệ thông tin, Kinh doanh, Marketing, v.v.)
  - Cấp bậc công việc (Thực tập sinh, Nhân viên, Trưởng nhóm, v.v.)
  - Loại công việc (Toàn thời gian, Bán thời gian, v.v.)
  - Kỹ năng (C#, ASP.NET Core, SQL Server, v.v.)

### 4. Test tạo tin tuyển dụng
- Nhấn nút **"Test tạo tin tuyển dụng"**
- Hệ thống sẽ tạo một tin tuyển dụng test với timestamp
- Kiểm tra kết quả trong popup

### 5. Kiểm tra dữ liệu
- Nhấn nút **"Kiểm tra dữ liệu"**
- Xem thống kê:
  - Tổng tin tuyển dụng
  - Tin đang hoạt động
  - Tin nổi bật
  - Tin mới (7 ngày qua)
- Xem danh sách tin tuyển dụng mới nhất

### 6. Kiểm tra trang chủ
- Truy cập trang chủ: `https://localhost:7001/`
- Kiểm tra xem tin tuyển dụng có hiển thị không
- Refresh trang nếu cần

### 7. Test tính năng mới: Real-time cập nhật
- **Làm mới bằng AJAX:** Nhấn nút "Làm mới" trên trang chủ
- **Auto refresh:** Hệ thống tự động cập nhật mỗi 30 giây
- **Thông báo:** Xem thông báo khi có tin mới
- **Hiệu ứng:** Xem animation cho tin tuyển dụng mới

### 8. Test đăng tin tuyển dụng thực tế
- Truy cập: `https://localhost:7001/Job/PostJob`
- Đăng tin tuyển dụng mới
- Kiểm tra xem tin có xuất hiện ngay lập tức trên trang chủ không
- Sử dụng nút "Làm mới" để cập nhật

### 9. Test trang Job Index
- Truy cập: `https://localhost:7001/Job`
- Kiểm tra xem tin tuyển dụng có hiển thị đúng layout không
- Sử dụng nút "Làm mới" để cập nhật real-time
- Kiểm tra auto refresh mỗi 30 giây
- Xem thông báo khi cập nhật thành công

### 10. Reset dữ liệu (nếu cần)
- Nhấn nút **"Reset dữ liệu"** để xóa tất cả tin tuyển dụng cũ
- Tạo lại dữ liệu mẫu sạch

## Các tính năng mới đã thêm

### 1. Real-time Updates
- **AJAX Refresh:** Cập nhật tin tuyển dụng mà không reload trang
- **Auto Refresh:** Tự động cập nhật mỗi 30 giây
- **Manual Refresh:** Nút "Làm mới" để cập nhật thủ công

### 2. Visual Indicators
- **Hot Badge:** Tin đăng trong 24 giờ qua có badge "Hot" màu đỏ
- **Featured Badge:** Tin nổi bật có badge "Nổi bật" màu vàng
- **New Badge:** Tin mới có badge "Mới" màu xanh
- **Real-time Badge:** Badge "Real-time" trên tiêu đề phần

### 3. Enhanced UX
- **Loading States:** Hiển thị loading khi đang cập nhật
- **Notifications:** Thông báo thành công/lỗi
- **Animations:** Hiệu ứng fade-in cho tin tuyển dụng mới
- **Hover Effects:** Hiệu ứng hover cho job cards

### 4. Improved Sorting
- **Priority Order:** Tin nổi bật hiển thị trước
- **Time-based:** Tin mới nhất hiển thị tiếp theo
- **Smart Filtering:** Chỉ hiển thị tin đang hoạt động

## Các vấn đề có thể gặp

### Vấn đề 1: Không có dữ liệu cơ bản
**Triệu chứng:** Lỗi khi tạo tin tuyển dụng
**Giải pháp:** Nhấn "Tạo dữ liệu cơ bản" trước

### Vấn đề 2: Cache trình duyệt
**Triệu chứng:** Tin tuyển dụng không hiển thị ngay
**Giải pháp:** 
- Sử dụng nút "Làm mới" trên trang chủ
- Refresh trang (Ctrl+F5)
- Xóa cache trình duyệt
- Mở tab ẩn danh

### Vấn đề 3: AJAX không hoạt động
**Triệu chứng:** Nút "Làm mới" không cập nhật được
**Giải pháp:**
- Kiểm tra console trong Developer Tools
- Đảm bảo JavaScript được bật
- Kiểm tra network tab để xem request

### Vấn đề 4: Database connection
**Triệu chứng:** Lỗi kết nối database
**Giải pháp:**
- Kiểm tra connection string trong `appsettings.json`
- Đảm bảo SQL Server đang chạy
- Kiểm tra quyền truy cập database

### Vấn đề 5: Trang chủ không load được tin tuyển dụng
**Triệu chứng:** Trang chủ trống hoặc lỗi
**Giải pháp:**
- Kiểm tra log trong console
- Đảm bảo tin tuyển dụng có `TrangThai = true`
- Kiểm tra relationship giữa các bảng

## Debug và Logging

### Kiểm tra log
- Mở Developer Tools (F12)
- Xem tab Console để kiểm tra lỗi JavaScript
- Kiểm tra tab Network để xem các request API
- Xem log của ứng dụng trong terminal

### Kiểm tra database
- Sử dụng SQL Server Management Studio
- Kiểm tra bảng `TinTuyenDungs`
- Đảm bảo các trường bắt buộc được điền đầy đủ
- Kiểm tra `TrangThai = true` cho tin đang hoạt động

### Kiểm tra API
- Test API: `GET /Home/GetLatestJobs`
- Kiểm tra response format
- Đảm bảo dữ liệu trả về đúng

## Liên kết hữu ích

- **Trang chủ:** `https://localhost:7001/`
- **Trang test:** `https://localhost:7001/Test`
- **Danh sách việc làm:** `https://localhost:7001/Job`
- **Đăng tin tuyển dụng:** `https://localhost:7001/Job/PostJob`
- **API Latest Jobs:** `https://localhost:7001/Home/GetLatestJobs`

## Lưu ý quan trọng

1. **ResponseCache:** Trang chủ đã được cấu hình không cache để đảm bảo dữ liệu luôn mới nhất
2. **Logging:** HomeController có logging chi tiết để debug
3. **Error Handling:** Có xử lý lỗi và fallback cho các trường hợp ngoại lệ
4. **Data Validation:** Đảm bảo dữ liệu được validate trước khi lưu
5. **Real-time Updates:** Hệ thống hỗ trợ cập nhật real-time qua AJAX
6. **Visual Feedback:** Có thông báo và hiệu ứng trực quan

## Kết quả mong đợi

Sau khi thực hiện các bước trên:
- Tin tuyển dụng sẽ xuất hiện ngay lập tức trên trang chủ
- Nút "Làm mới" sẽ cập nhật tin tuyển dụng mà không reload trang
- Auto refresh sẽ tự động cập nhật mỗi 30 giây
- Thống kê sẽ được cập nhật chính xác
- Không có lỗi trong console
- Dữ liệu được lưu đúng trong database
- Có thông báo và hiệu ứng trực quan khi cập nhật 