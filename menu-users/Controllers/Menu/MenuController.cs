using menu_users.Application.DTOs.Menu;
using menu_users.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace menu_users.Controllers.Menu
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly IMenuService _menuService;
        private readonly IMenuToUserService _menuToUserService;

        public MenuController(IMenuService menuService, IMenuToUserService menuToUserService)
        {
            _menuService = menuService;
            _menuToUserService = menuToUserService;
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetMenuByUserIdAsync([FromRoute] string userId)
        {
            var result = await _menuToUserService.GetMenusByUserIdAsync(Guid.Parse(userId));
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
            var result = await _menuService.GetAllMenusAsync();
            if (!result.Success)
            {
                return BadRequest(new { message = result.Error });
            }

            return Ok(result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMenuAsync([FromBody] CreateMenu request)
        {
            var result = await _menuService.CreateMenuAsync(request);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Error });
            }

            return Ok(result.Data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMenuAsync([FromRoute] int id, [FromBody] MenuUpdate request)
        {
            var result = await _menuService.UpdateMenuAsync(id, request);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Error });

            }

            return Ok(result.Data);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMenuAsync([FromRoute] int id)
        {
            var result = await _menuService.DeleteMenuAsync(id);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Error });

            }

            return Ok(result.Data);
        }

        [HttpPut("{userId}/set-default-menus")]
        public async Task<IActionResult> SetDefaultMenusByHierarchyAsync([FromRoute] Guid userId)
        {
            var result = await _menuToUserService.SetDefaultMenusByHierarchyAsync(userId);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Error });
            }

            return Ok(result.Data);
        }

        [HttpPost("{userId}/add-menu/{menuId}")]
        public async Task<IActionResult> AddMenuToUserAsync([FromRoute] Guid userId, [FromRoute] int menuId)
        {
            var result = await _menuToUserService.AddMenuToUserAsync(userId, menuId);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Error });
            }

            return Ok(result.Data);
        }

        [HttpDelete("{userId}/remove-menu/{menuId}")]
        public async Task<IActionResult> RemoveMenuFromUserAsync([FromRoute] Guid userId, [FromRoute] int menuId)
        {
            var result = await _menuToUserService.RemoveMenuFromUserAsync(userId, menuId);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Error });
            }
            return Ok(result.Data);
        }
    }
}
