# Hệ thống Phân quyền - LTW

## Tổng quan

Hệ thống phân quyền của LTW được thiết kế để phân biệt rõ ràng giữa các vai trò người dùng và đảm bảo mỗi vai trò chỉ có thể truy cập các chức năng phù hợp.

## Các vai trò trong hệ thống

### 1. Ứng viên (Candidate)
**Quyền hạn:**
- Xem danh sách tin tuyển dụng
- Tìm kiếm việc làm
- Ứng tuyển vào các vị trí
- Lưu việc làm yêu thích
- Xem trạng thái đơn ứng tuyển
- Cập nhật hồ sơ cá nhân
- Xem dashboard cá nhân

**Chức năng chính:**
- Dashboard hiển thị thống kê đơn ứng tuyển
- Quản lý hồ sơ ứng tuyển
- Lưu và quản lý việc làm yêu thích

### 2. Doanh nghiệp (Employer)
**Quyền hạn:**
- Đăng tin tuyển dụng
- Quản lý tin tuyển dụng đã đăng
- Xem và quản lý đơn ứng tuyển
- Cập nhật trạng thái đơn ứng tuyển
- Xem thống kê tuyển dụng
- Cập nhật thông tin công ty

**Chức năng chính:**
- Dashboard hiển thị thống kê tuyển dụng
- Quản lý ứng viên ứng tuyển
- Đăng và chỉnh sửa tin tuyển dụng

### 3. Admin
**Quyền hạn:**
- Tất cả quyền của Ứng viên và Doanh nghiệp
- Quản lý toàn bộ hệ thống
- Xem thống kê tổng quan
- Quản lý người dùng
- Quản lý tin tuyển dụng

## Cách sử dụng hệ thống phân quyền

### 1. Đăng ký tài khoản
Khi đăng ký, người dùng phải chọn vai trò:
- **Ứng viên**: Dành cho người tìm việc
- **Doanh nghiệp**: Dành cho nhà tuyển dụng

### 2. Đăng nhập
Khi đăng nhập, người dùng phải chọn đúng vai trò đã đăng ký để có thể truy cập hệ thống.

### 3. Truy cập Dashboard
Sau khi đăng nhập, người dùng sẽ được chuyển hướng đến dashboard phù hợp với vai trò của mình.

## Cấu trúc code

### AuthorizationHelper.cs
File chứa các helper method và attribute để kiểm tra quyền:

```csharp
// Kiểm tra đăng nhập
[RequireLogin]

// Kiểm tra vai trò cụ thể
[RequireCandidate]    // Chỉ ứng viên
[RequireEmployer]     // Chỉ doanh nghiệp
[RequireAdmin]        // Chỉ admin

// Kiểm tra nhiều vai trò
[RequireRole("Ứng viên", "Admin")]
```

### Các method helper:
```csharp
AuthorizationHelper.IsLoggedIn(session)
AuthorizationHelper.IsCandidate(session)
AuthorizationHelper.IsEmployer(session)
AuthorizationHelper.CanPostJob(session)
AuthorizationHelper.CanApplyJob(session)
```

## Ví dụ sử dụng

### 1. Bảo vệ Action trong Controller
```csharp
[RequireCandidate]
public async Task<IActionResult> Apply(int id)
{
    // Chỉ ứng viên mới có thể truy cập
}

[RequireEmployer]
public async Task<IActionResult> PostJob()
{
    // Chỉ doanh nghiệp mới có thể truy cập
}
```

### 2. Kiểm tra quyền trong View
```csharp
@if (AuthorizationHelper.IsEmployer(Context.Session))
{
    <a asp-action="PostJob" class="btn btn-success">Đăng tin tuyển dụng</a>
}
```

### 3. Kiểm tra quyền trong Controller
```csharp
if (!AuthorizationHelper.CanPostJob(HttpContext.Session))
{
    return RedirectToAction("AccessDenied", "Account");
}
```

## Bảo mật

### 1. Session Management
- Thông tin người dùng được lưu trong session
- Session được kiểm tra ở mọi request cần phân quyền
- Tự động chuyển hướng về trang đăng nhập nếu chưa đăng nhập

### 2. Access Control
- Mỗi action được bảo vệ bằng attribute phù hợp
- Kiểm tra vai trò trước khi cho phép truy cập
- Trang AccessDenied hiển thị khi không có quyền

### 3. Data Protection
- Người dùng chỉ có thể truy cập dữ liệu của mình
- Doanh nghiệp chỉ có thể quản lý tin tuyển dụng của mình
- Ứng viên chỉ có thể xem đơn ứng tuyển của mình

## Mở rộng hệ thống

### Thêm vai trò mới
1. Thêm vai trò vào database
2. Tạo attribute mới trong AuthorizationHelper
3. Cập nhật logic phân quyền
4. Tạo dashboard cho vai trò mới

### Thêm quyền mới
1. Thêm method kiểm tra quyền trong AuthorizationHelper
2. Sử dụng method trong controller hoặc view
3. Cập nhật UI để hiển thị chức năng mới

## Troubleshooting

### Lỗi thường gặp:
1. **"Truy cập bị từ chối"**: Kiểm tra vai trò đăng nhập có đúng không
2. **Không hiển thị menu**: Kiểm tra logic hiển thị menu trong layout
3. **Không lưu được session**: Kiểm tra cấu hình session trong Program.cs

### Debug:
- Kiểm tra session: `Context.Session.GetString("UserRole")`
- Kiểm tra đăng nhập: `AuthorizationHelper.IsLoggedIn(Context.Session)`
- Xem log để tìm lỗi chi tiết

## Kết luận

Hệ thống phân quyền được thiết kế để đảm bảo an toàn và dễ sử dụng. Mỗi vai trò có quyền hạn rõ ràng và được bảo vệ bằng nhiều lớp kiểm tra. Việc mở rộng hệ thống cũng được thiết kế để dễ dàng thực hiện. 