using menu_users.Application.DTOs.Menu;
using menu_users.Application.Features.Menus.CreateMenuUseCase;
using menu_users.Application.Features.Menus.UpdateMenuUseCase;
using menu_users.Domain.Interfaces.Features.Menus;
using menu_users.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace menu_users.Controllers.Menu
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly IGetMenusByUserIdUseCase _getMenusByUserIdUseCase;
        private readonly IGetAllMenusUseCase _getAllMenusUseCase;
        private readonly ICreateMenuUseCase _createMenuUseCase;
        private readonly IUpdateMenuUseCase _updateMenuUseCase;
        private readonly IDeleteMenuUseCase _deleteMenuUseCase;
        private readonly IAssignDefaultMenusToUserUseCase _assignDefaultMenusToUserUseCase;
        private readonly IAssignMenuToUserUseCase _assignMenuToUserUseCase;
        private readonly IRemoveMenuToUserUseCase _removeMenuToUserUseCase;

        public MenuController(
            IGetMenusByUserIdUseCase getMenusByUserIdUseCase,
            IGetAllMenusUseCase getAllMenusUseCase,
            ICreateMenuUseCase createMenuUseCase,
            IUpdateMenuUseCase updateMenuUseCase,
            IDeleteMenuUseCase deleteMenuUseCase,
            IAssignDefaultMenusToUserUseCase assignDefaultMenusToUserUseCase,
            IAssignMenuToUserUseCase assignMenuToUserUseCase,
            IRemoveMenuToUserUseCase removeMenuToUserUseCase)
        {
            _getMenusByUserIdUseCase = getMenusByUserIdUseCase;
            _getAllMenusUseCase = getAllMenusUseCase;
            _createMenuUseCase = createMenuUseCase;
            _updateMenuUseCase = updateMenuUseCase;
            _deleteMenuUseCase = deleteMenuUseCase;
            _assignDefaultMenusToUserUseCase = assignDefaultMenusToUserUseCase;
            _assignMenuToUserUseCase = assignMenuToUserUseCase;
            _removeMenuToUserUseCase = removeMenuToUserUseCase;
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetMenuByUserIdAsync([FromRoute] string userId)
        {
            var result = await _getMenusByUserIdUseCase.Execute(userId);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Error });
            }

            return Ok(result.Data);
        }

        // En este hay que verificar luego que el usuario sea jerarquía de superUser
        [HttpGet]
        public async Task<IActionResult> GetAllMenusAsync()
        {
            var result = await _getAllMenusUseCase.Execute();
            if (!result.Success)
            {
                return BadRequest(new { message = result.Error });
            }

            return Ok(result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMenuAsync([FromBody] CreateMenuRequest request)
        {
            var result = await _createMenuUseCase.Execute(request);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Error });
            }

            return Ok(result.Data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMenuAsync([FromRoute] int id, [FromBody] UpdateMenuRequest request)
        {
            var result = await _updateMenuUseCase.Execute(id, request);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Error });

            }

            return Ok(result.Data);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMenuAsync([FromRoute] int id)
        {
            var result = await _deleteMenuUseCase.Execute(id);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Error });

            }

            return Ok(result.Data);
        }

        [HttpPut("{userId}/set-default-menus")]
        public async Task<IActionResult> SetDefaultMenusByHierarchyAsync([FromRoute] Guid userId)
        {
            var result = await _assignDefaultMenusToUserUseCase.Execute(userId);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Error });
            }

            return Ok(result.Data);
        }

        [HttpPost("{userId}/add-menu/{menuId}")]
        public async Task<IActionResult> AddMenuToUserAsync([FromRoute] Guid userId, [FromRoute] int menuId)
        {
            var result = await _assignMenuToUserUseCase.Execute(userId, menuId);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Error });
            }

            return Ok(result.Data);
        }

        [HttpDelete("{userId}/remove-menu/{menuId}")]
        public async Task<IActionResult> RemoveMenuFromUserAsync([FromRoute] Guid userId, [FromRoute] int menuId)
        {
            var result = await _removeMenuToUserUseCase.Execute(userId, menuId);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Error });
            }
            return Ok(result.Data);
        }
    }
}
