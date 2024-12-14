using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RBAC_API.DTOs;
using RBAC_API.Models;
using RBAC_API.Repositories;

namespace RBAC_API.Controllers
{
    [ApiController]
    [Route("api/permissions")]
    [Authorize(Roles = "Admin")] 
    public class PermissionsController : ControllerBase
    {
        private readonly IPermissionRepository _permissionRepository;

        public PermissionsController(IPermissionRepository permissionRepository)
        {
            _permissionRepository = permissionRepository;
        }

        [HttpPost]
        public IActionResult CreatePermission([FromBody] PermissionDTO permissionDto)
        {
            var permission = new Permission
            {
                Type = Enum.Parse<PermissionType>(permissionDto.Type, true),
            };

            _permissionRepository.Add(permission);

            return CreatedAtAction(nameof(GetPermission), new { id = permission.Id }, permissionDto);
        }

        [HttpGet("{id}")]
        public IActionResult GetPermission(int id)
        {
            var permission = _permissionRepository.GetById(id);
            if (permission == null)
                return NotFound();

            return Ok(new PermissionDTO
            {
                Id = permission.Id,
                Type = permission.Type.ToString(),
            });
        }

        [HttpGet]
        public IActionResult GetAllPermissions()
        {
            var permissions = _permissionRepository.GetAll();
            var permissionDtos = permissions.Select(permission => new PermissionDTO
            {
                Id = permission.Id,
                Type = permission.Type.ToString(),
            });

            return Ok(permissionDtos);
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePermission(int id)
        {
            var permission = _permissionRepository.GetById(id);
            if (permission == null)
                return NotFound();

            _permissionRepository.Delete(id);

            return NoContent();
        }
    }
}
