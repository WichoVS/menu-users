using System;
using menu_users.Application.DTOs.ApiResponse;
using menu_users.Application.DTOs.Auth;
using menu_users.Application.DTOs.User;

namespace menu_users.Domain.Interfaces.Services;

public interface IUserService
{
    Task<ApiResponse<IEnumerable<UserDTO>>> GetAllUsersAsync();
    Task<ApiResponse<UserDTO>> GetUserByIdAsync(string id);
    Task<ApiResponse<UserDTO>> CreateUserAsync(SignInRequest request);
    Task<ApiResponse<UserDTO>> UpdateUserAsync(string id, UpdateUserRequest request);
    Task<ApiResponse<bool>> DeleteUserAsync(string id);

}
