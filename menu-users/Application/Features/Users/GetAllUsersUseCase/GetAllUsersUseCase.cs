using System;
using menu_users.Application.DTOs.ApiResponse;
using menu_users.Application.DTOs.User;
using menu_users.Domain.Interfaces.Features.User;
using menu_users.Domain.Interfaces.Repositories;

namespace menu_users.Application.Features.Users.GetAllUsersUseCase;

public class GetAllUsersUseCase : IGetAllUsersUseCase
{
    private readonly IUserRepository _userRepository;

    public GetAllUsersUseCase(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<ApiResponse<IEnumerable<UserDTO>>> Execute()
    {
        var users = await _userRepository.GetAllAsync();
        var userDTOs = users.Select(u => new UserDTO
        (
            u.Id.ToString(),
            u.FirstName,
            u.LastName,
            u.Email,
            u.Role.Name,
            u.RoleId
        ));

        return new ApiResponse<IEnumerable<UserDTO>>(true, "Users retrieved successfully.", userDTOs);
    }
}
