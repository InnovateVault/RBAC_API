using RBAC_API.Models;

namespace RBAC_API.Services
{
    public interface IAuthService
    {
        string Authenticate(string username, string password);
        User Register(string username, string password, UserRole role);
        bool HasPermission(User user, PermissionType permissionType);
    }
}
