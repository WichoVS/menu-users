using System;
using menu_users.Application.DTOs.ApiResponse;
using menu_users.Application.DTOs.User;
using menu_users.Application.Features.Users.UpdateUserUseCase;

namespace menu_users.Domain.Interfaces.Features.User;

public interface IUpdateUserUseCase
{
    Task<ApiResponse<UserDTO>> Execute(Guid id, UpdateUserRequest request);
}
