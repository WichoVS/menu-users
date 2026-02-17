using menu_users.Application.DTOs.User;
using menu_users.Application.Features.Users.CreateUserUseCase;
using menu_users.Application.Features.Users.UpdatePasswordUseCase;
using menu_users.Application.Features.Users.UpdateUserUseCase;
using menu_users.Domain.Interfaces.Features.User;
using menu_users.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace menu_users.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ICreateUserUseCase _createUserUseCase;
        private readonly IGetAllUsersUseCase _getAllUsersUseCase;
        private readonly IGetUserByIdUseCase _getUserByIdUseCase;
        private readonly IRemoveUserUseCase _removeUserUseCase;
        private readonly IUpdateUserUseCase _updateUserUseCase;
        private readonly IGeneratePasswordUseCase _generatePasswordUseCase;
        private readonly IUpdatePasswordUseCase _updatePasswordUseCase;
        public UserController(
            ICreateUserUseCase createUserUseCase,
            IGetAllUsersUseCase getAllUsersUseCase,
            IGetUserByIdUseCase getUserByIdUseCase,
            IRemoveUserUseCase removeUserUseCase,
            IUpdateUserUseCase updateUserUseCase,
            IGeneratePasswordUseCase generatePasswordUseCase,
            IUpdatePasswordUseCase updatePasswordUseCase)
        {
            _createUserUseCase = createUserUseCase;
            _getAllUsersUseCase = getAllUsersUseCase;
            _getUserByIdUseCase = getUserByIdUseCase;
            _removeUserUseCase = removeUserUseCase;
            _updatePasswordUseCase = updatePasswordUseCase;
            _updateUserUseCase = updateUserUseCase;
            _generatePasswordUseCase = generatePasswordUseCase;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _getAllUsersUseCase.Execute();
            if (!result.Success)
            {
                return BadRequest(new { message = result.Error });
            }
            return Ok(result.Data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById([FromRoute] string id)
        {
            var result = await _getUserByIdUseCase.Execute(Guid.Parse(id));
            if (!result.Success)
            {
                return BadRequest(new { message = result.Error });
            }
            return Ok(result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
        {
            var result = await _createUserUseCase.Execute(request);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Error });
            }
            return Ok(result.Data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser([FromRoute] string id, [FromBody] UpdateUserRequest request)
        {
            var result = await _updateUserUseCase.Execute(Guid.Parse(id), request);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Error });
            }
            return Ok(result.Data);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] string id)
        {
            var result = await _removeUserUseCase.Execute(Guid.Parse(id));
            if (!result.Success)
            {
                return BadRequest(new { message = result.Error });
            }
            return Ok(result.Data);
        }

        [HttpPost("{id}/generate-password")]
        public async Task<IActionResult> GeneratePassword([FromRoute] string id)
        {
            var result = await _generatePasswordUseCase.Execute(Guid.Parse(id));
            if (!result.Success)
            {
                return BadRequest(new { message = result.Error });
            }
            return Ok(result.Data);
        }

        [HttpPut("{id}/password")]
        public async Task<IActionResult> UpdateUserPassword([FromRoute] string id, [FromBody] UpdatePasswordRequest request)
        {
            var result = await _updatePasswordUseCase.Execute(Guid.Parse(id), request.NewPassword);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Error });
            }
            return Ok(result.Data);
        }
    }

}
