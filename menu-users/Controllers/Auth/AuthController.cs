using System.Security.Claims;
using menu_users.Application.DTOs.Auth;
using menu_users.Application.Features.Auth.LoginUserUseCase;
using menu_users.Domain.Interfaces.Features.Auth;
using menu_users.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace menu_users.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILoginUserUseCase LoginUserUseCase;
        private readonly IRegisterUserUseCase RegisterUserUseCase;


        public AuthController(ILoginUserUseCase loginUserUseCase, IRegisterUserUseCase registerUserUseCase)
        {
            LoginUserUseCase = loginUserUseCase;
            RegisterUserUseCase = registerUserUseCase;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginUserRequest request)
        {
            var result = await LoginUserUseCase.Execute(request);

            if (!result.Success)
            {
                return BadRequest(new { message = result.Error });
            }

            return Ok(result.Data);
        }

        [HttpPost("signin")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
        {
            var result = await RegisterUserUseCase.Execute(request);

            if (!result.Success)
            {
                return BadRequest(new { message = result.Error });
            }

            return Ok(result.Data);
        }

        [HttpGet("me")]
        public IActionResult Me()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            if (userId is null)
                return Unauthorized("Token no válido");

            return Ok(new
            {
                Id = userId,
                Email = email,
                Role = role
            });
        }

    }
}
