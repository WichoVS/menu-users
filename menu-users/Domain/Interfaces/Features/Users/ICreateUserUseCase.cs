using System;
using menu_users.Application.DTOs.ApiResponse;
using menu_users.Application.DTOs.User;
using menu_users.Application.Features.Users.CreateUserUseCase;

namespace menu_users.Domain.Interfaces.Features.User;

public interface ICreateUserUseCase
{
    Task<ApiResponse<UserDTO>> Execute(CreateUserRequest request);
}
