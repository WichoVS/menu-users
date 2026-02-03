using menu_users.Application.DTOs.Role;
using menu_users.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace menu_users.Controllers.Role
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] RoleCreateRequest request)
        {
            var result = await _roleService.CreateRoleAsync(request);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Error });
            }
            return Ok(result.Data);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var result = await _roleService.DeleteRoleAsync(id);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Error });
            }
            return Ok(result.Data);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRoles()
        {
            var result = await _roleService.GetAllRolesAsync();
            if (!result.Success)
            {
                return BadRequest(new { message = result.Error });
            }
            return Ok(result.Data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoleById(int id)
        {
            var result = await _roleService.GetRoleByIdAsync(id);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Error });
            }
            return Ok(result.Data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRole(int id, [FromBody] RoleUpdateRequest request)
        {
            var result = await _roleService.UpdateRoleAsync(id, request);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Error });
            }
            return Ok(result.Data);
        }
    }
}
