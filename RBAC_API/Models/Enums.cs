namespace RBAC_API.Models
{
    public enum UserRole
    {
        Admin = 1,
        Manager = 2,
        Employee = 3,
        Guest = 4
    }

    public enum PermissionType
    {
        Create = 1,
        Read = 2,
        Update = 3,
        Delete = 4
    }

    public enum UserStatus
    {
        Active = 1,
        Inactive = 2,
        Suspended = 3
    }
}
