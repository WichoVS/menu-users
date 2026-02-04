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

        public MenuController(IMenuService menuService)
        {
            _menuService = menuService;
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetMenuByUserIdAsync([FromRoute] string userId)
        {
            var result = await _menuService.GetMenuByUserIdAsync(userId);
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        // En este hay que verificar luego que el usuario sea jerarquía de superUser
        [HttpGet]
        public async Task<IActionResult> GetAllMenusAsync()
        {
            var result = await _menuService.GetAllMenusAsync();
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMenuAsync([FromBody] CreateMenu request)
        {
            var result = await _menuService.CreateMenuAsync(request);
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMenuAsync([FromRoute] int id, [FromBody] MenuUpdate request)
        {
            var result = await _menuService.UpdateMenuAsync(id, request);
            if (!result.Success)
            {
                return BadRequest(result);

            }

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMenuAsync([FromRoute] int id)
        {
            var result = await _menuService.DeleteMenuAsync(id);
            if (!result.Success)
            {
                return BadRequest(result);

            }

            return Ok(result);
        }
    }
}
