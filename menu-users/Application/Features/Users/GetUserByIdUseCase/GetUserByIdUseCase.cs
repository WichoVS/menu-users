using System;
using menu_users.Application.DTOs.ApiResponse;
using menu_users.Application.DTOs.User;
using menu_users.Domain.Interfaces.Features.User;
using menu_users.Domain.Interfaces.Repositories;

namespace menu_users.Application.Features.Users.GetUserByIdUseCase;

public class GetUserByIdUseCase : IGetUserByIdUseCase
{
    private readonly IUserRepository _userRepository;
    public GetUserByIdUseCase(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<ApiResponse<UserDTO>> Execute(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
        {
            return new ApiResponse<UserDTO>(false, "User not found.", null);
        }

        return new ApiResponse<UserDTO>(true, "User retrieved successfully.", new UserDTO
        (
            user.Id.ToString(),
            user.FirstName,
            user.LastName,
            user.Email,
            user.Role.Name,
            user.RoleId
        ));
    }
}
