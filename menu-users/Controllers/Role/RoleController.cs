using menu_users.Application.DTOs.Role;
using menu_users.Application.Features.Roles.CreateRoleUseCase;
using menu_users.Application.Features.Roles.UpdateRoleUseCase;
using menu_users.Domain.Interfaces.Features.Roles;
using menu_users.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace menu_users.Controllers.Role
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly ICreateRoleUseCase _createRoleUseCase;
        private readonly IDeleteRoleUseCase _deleteRoleUseCase;
        private readonly IGetAllRolesUseCase _getAllRolesUseCase;
        private readonly IGetRoleByIdUseCase _getRoleByIdUseCase;
        private readonly IUpdateRoleUseCase _updateRoleUseCase;
        public RoleController(
            ICreateRoleUseCase createRoleUseCase,
            IDeleteRoleUseCase deleteRoleUseCase,
            IGetAllRolesUseCase getAllRolesUseCase,
            IGetRoleByIdUseCase getRoleByIdUseCase,
            IUpdateRoleUseCase updateRoleUseCase
        )
        {
            _createRoleUseCase = createRoleUseCase;
            _deleteRoleUseCase = deleteRoleUseCase;
            _getAllRolesUseCase = getAllRolesUseCase;
            _getRoleByIdUseCase = getRoleByIdUseCase;
            _updateRoleUseCase = updateRoleUseCase;
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleRequest request)
        {
            var result = await _createRoleUseCase.Execute(request);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Error });
            }
            return Ok(result.Data);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var result = await _deleteRoleUseCase.Execute(id);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Error });
            }
            return Ok(result.Data);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRoles()
        {
            var result = await _getAllRolesUseCase.Execute();
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result.Data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoleById(int id)
        {
            var result = await _getRoleByIdUseCase.Execute(id);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Error });
            }
            return Ok(result.Data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRole(int id, [FromBody] UpdateRoleRequest request)
        {
            var result = await _updateRoleUseCase.Execute(id, request);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Error });
            }
            return Ok(result.Data);
        }
    }
}
