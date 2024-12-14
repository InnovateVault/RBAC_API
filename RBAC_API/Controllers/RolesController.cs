using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RBAC_API.DTOs;
using RBAC_API.Models;
using RBAC_API.Repositories;

namespace RBAC_API.Controllers
{
    [ApiController]
    [Route("api/roles")]
    public class RolesController : ControllerBase
    {
        private readonly IRoleRepository _roleRepository;

        public RolesController(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        [HttpPost]
        public IActionResult CreateRole([FromBody] RoleDTO roleDto)
        {
            var role = new Role
            {
                Name = roleDto.Name
            };

            _roleRepository.Add(role);

            return CreatedAtAction(nameof(GetRole), new { id = role.Id }, roleDto);
        }

        [HttpGet("{id}")]
        public IActionResult GetRole(int id)
        {
            var role = _roleRepository.GetById(id);
            if (role == null)
                return NotFound();

            return Ok(new RoleDTO
            {
                Id = role.Id,
                Name = role.Name
            });
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAllRoles()
        {
            var roles = _roleRepository.GetAll();
            var roleDtos = roles.Select(role => new RoleDTO
            {
                Id = role.Id,
                Name = role.Name
            });

            return Ok(roleDtos);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteRole(int id)
        {
            var role = _roleRepository.GetById(id);
            if (role == null)
                return NotFound();

            _roleRepository.Delete(id);

            return NoContent();
        }
    }
}
