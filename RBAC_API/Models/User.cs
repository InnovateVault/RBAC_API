namespace RBAC_API.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public UserRole Role { get; set; }
        public UserStatus Status { get; set; } = UserStatus.Active;
    }
}
