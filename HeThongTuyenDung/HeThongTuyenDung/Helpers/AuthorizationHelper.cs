using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HeThongTuyenDung.Helpers
{
    public static class AuthorizationHelper
    {
        public static bool IsLoggedIn(ISession session)
        {
            return !string.IsNullOrEmpty(session.GetString("UserId"));
        }

        public static string? GetUserRole(ISession session)
        {
            return session.GetString("UserRole");
        }

        public static bool IsCandidate(ISession session)
        {
            return GetUserRole(session) == "Ứng viên";
        }

        public static bool IsEmployer(ISession session)
        {
            return GetUserRole(session) == "Doanh nghiệp";
        }

        public static bool IsAdmin(ISession session)
        {
            return GetUserRole(session) == "Admin";
        }

        public static bool CanPostJob(ISession session)
        {
            var role = GetUserRole(session);
            return role == "Doanh nghiệp" || role == "Admin";
        }

        public static bool CanApplyJob(ISession session)
        {
            return IsCandidate(session);
        }

        public static bool CanManageApplications(ISession session)
        {
            return IsEmployer(session) || IsAdmin(session);
        }

        public static bool CanViewProfile(ISession session, int targetUserId)
        {
            if (!IsLoggedIn(session)) return false;
            
            var currentUserId = GetUserId(session);
            if (string.IsNullOrEmpty(currentUserId)) return false;
            
            // Người dùng có thể xem profile của chính mình
            if (int.Parse(currentUserId) == targetUserId) return true;
            
            // Admin có thể xem tất cả profile
            if (IsAdmin(session)) return true;
            
            // Doanh nghiệp có thể xem profile ứng viên đã ứng tuyển vào công ty của họ
            if (IsEmployer(session)) return true;
            
            return false;
        }

        public static bool CanEditProfile(ISession session, int targetUserId)
        {
            if (!IsLoggedIn(session)) return false;
            
            var currentUserId = GetUserId(session);
            if (string.IsNullOrEmpty(currentUserId)) return false;
            
            // Chỉ có thể edit profile của chính mình
            if (int.Parse(currentUserId) == targetUserId) return true;
            
            // Admin có thể edit tất cả profile
            if (IsAdmin(session)) return true;
            
            return false;
        }

        public static string? GetUserId(ISession session)
        {
            return session.GetString("UserId");
        }

        public static string? GetUserName(ISession session)
        {
            return session.GetString("UserName");
        }

        public static int? GetUserIdAsInt(ISession session)
        {
            var userId = GetUserId(session);
            if (int.TryParse(userId, out int result))
                return result;
            return null;
        }
    }

    // Attribute để yêu cầu đăng nhập
    public class RequireLoginAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var session = context.HttpContext.Session;
            
            if (!AuthorizationHelper.IsLoggedIn(session))
            {
                context.Result = new RedirectToActionResult("Login", "Account", new { returnUrl = context.HttpContext.Request.Path });
                return;
            }
            
            base.OnActionExecuting(context);
        }
    }

    // Attribute để yêu cầu vai trò cụ thể
    public class RequireRoleAttribute : ActionFilterAttribute
    {
        private readonly string[] _allowedRoles;

        public RequireRoleAttribute(params string[] roles)
        {
            _allowedRoles = roles;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var session = context.HttpContext.Session;
            
            if (!AuthorizationHelper.IsLoggedIn(session))
            {
                context.Result = new RedirectToActionResult("Login", "Account", new { returnUrl = context.HttpContext.Request.Path });
                return;
            }

            var userRole = AuthorizationHelper.GetUserRole(session);
            if (string.IsNullOrEmpty(userRole) || !_allowedRoles.Contains(userRole))
            {
                context.Result = new RedirectToActionResult("AccessDenied", "Account", null);
                return;
            }
            
            base.OnActionExecuting(context);
        }
    }

    // Attribute cho ứng viên
    public class RequireCandidateAttribute : RequireRoleAttribute
    {
        public RequireCandidateAttribute() : base("Ứng viên") { }
    }

    // Attribute cho doanh nghiệp
    public class RequireEmployerAttribute : RequireRoleAttribute
    {
        public RequireEmployerAttribute() : base("Doanh nghiệp") { }
    }

    // Attribute cho admin
    public class RequireAdminAttribute : RequireRoleAttribute
    {
        public RequireAdminAttribute() : base("Admin") { }
    }
} 