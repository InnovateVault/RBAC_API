using Microsoft.AspNetCore.Mvc;
using RBAC_API.DTOs;
using RBAC_API.Models;
using RBAC_API.Services;
using System;

namespace RBAC_API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService; 

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // Register a new user
        [HttpPost("register")]
        public IActionResult Register([FromBody] CreateUserRequest request)
        {
            try
            {
                // Register user
                var user = _authService.Register(request.Username, request.Password, Enum.Parse<UserRole>(request.Role, true));

                // Return user details (excluding password)
                var userDto = new UserDTO
                {
                    Id = user.Id,
                    Username = user.Username,
                    Role = user.Role.ToString(),
                    Status = user.Status.ToString()
                };

                return CreatedAtAction(nameof(Register), new { id = user.Id }, userDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // User login (generate JWT token)
        [HttpPost("login")]
        public IActionResult Login([FromBody] CreateUserRequest request)
        {
            try
            {
                var token = _authService.Authenticate(request.Username, request.Password);

                // Return the JWT token
                return Ok(new { Token = token });
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("Invalid username or password.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

}
