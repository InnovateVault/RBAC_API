using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RBAC_API.DTOs;
using RBAC_API.Models;
using RBAC_API.Repositories;

namespace RBAC_API.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost]
        [AllowAnonymous] // Allows anyone to register a new user
        public IActionResult CreateUser([FromBody] CreateUserRequest request)
        {
            var user = new User
            {
                Username = request.Username,
                PasswordHash = HashPassword(request.Password),
                Role = Enum.Parse<UserRole>(request.Role, true),
                Status = UserStatus.Active
            };

            _userRepository.Add(user);

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, new UserDTO
            {
                Id = user.Id,
                Username = user.Username,
                Role = user.Role.ToString(),
                Status = user.Status.ToString()
            });
        }

        [HttpGet("{id}")]
        [Authorize] // Any authenticated user can access this
        public IActionResult GetUser(int id)
        {
            var user = _userRepository.GetById(id);
            if (user == null)
                return NotFound();

            return Ok(new UserDTO
            {
                Id = user.Id,
                Username = user.Username,
                Role = user.Role.ToString(),
                Status = user.Status.ToString()
            });
        }

        [HttpGet]
        [Authorize(Roles = "Admin")] // Only Admins can access this
        public IActionResult GetAllUsers()
        {
            var users = _userRepository.GetAll();
            var userDtos = users.Select(user => new UserDTO
            {
                Id = user.Id,
                Username = user.Username,
                Role = user.Role.ToString(),
                Status = user.Status.ToString()
            });

            return Ok(userDtos);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")] // Only Admins can update user details
        public IActionResult UpdateUser(int id, [FromBody] CreateUserRequest request)
        {
            var user = _userRepository.GetById(id);
            if (user == null)
                return NotFound();

            user.Username = request.Username;
            user.Role = Enum.Parse<UserRole>(request.Role, true);

            _userRepository.Update(user);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")] // Only Admins can delete users
        public IActionResult DeleteUser(int id)
        {
            var user = _userRepository.GetById(id);
            if (user == null)
                return NotFound();

            _userRepository.Delete(id);

            return NoContent();
        }

        private string HashPassword(string password)
        {
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            var hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }
}
