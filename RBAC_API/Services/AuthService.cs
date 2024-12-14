using Microsoft.EntityFrameworkCore;
using RBAC_API.Data;
using RBAC_API.Helpers;
using RBAC_API.Models;
using System.Security.Cryptography;
using System.Text;

namespace RBAC_API.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;

        public AuthService(ApplicationDbContext context)
        {
            _context = context;
        }

        public string Authenticate(string username, string password)
        {
            var user = _context.Users.Include(u => u.Roles).SingleOrDefault(u => u.Username == username);

            if (user == null || !VerifyPassword(password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid username or password.");

            var jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET") ?? throw new InvalidOperationException("JWT_SECRET not set.");
            var jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? throw new InvalidOperationException("JWT_ISSUER not set.");
            var jwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? throw new InvalidOperationException("JWT_AUDIENCE not set.");

            return JwtHelper.GenerateJwtToken(user.Username, string.Join(",", user.Role), jwtSecret, jwtIssuer, jwtAudience);
        }

        public User Register(string username, string password, UserRole role)
        {
            if (_context.Users.Any(u => u.Username == username))
                throw new InvalidOperationException("A user with the given username already exists.");

            var passwordHash = HashPassword(password);

            var user = new User
            {
                Username = username,
                PasswordHash = passwordHash,
                Role = role,
                Status = UserStatus.Active
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return user;
        }

        public bool HasPermission(User user, PermissionType permissionType)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var userPermissions = _context.Roles
                .Include(r => r.Permissions)
                .Where(r => user.Roles.Any(ur => ur.Id == r.Id))
                .SelectMany(r => r.Permissions)
                .ToList();

            return userPermissions.Any(p => p.Type == permissionType);
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        private bool VerifyPassword(string password, string hash)
        {
            return HashPassword(password) == hash;
        }
    }
}
