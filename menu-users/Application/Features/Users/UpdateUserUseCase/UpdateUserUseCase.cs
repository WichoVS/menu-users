using System;
using menu_users.Application.DTOs.ApiResponse;
using menu_users.Application.DTOs.User;
using menu_users.Domain.Entities;
using menu_users.Domain.Interfaces.Features.User;
using menu_users.Domain.Interfaces.Repositories;

namespace menu_users.Application.Features.Users.UpdateUserUseCase;

public class UpdateUserUseCase : IUpdateUserUseCase
{
    private readonly IUserRepository _userRepository;
    public UpdateUserUseCase(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<ApiResponse<UserDTO>> Execute(Guid id, UpdateUserRequest request)
    {
        User? user = await _userRepository.GetByIdAsync(id);
        if (user == null)
        {
            return new ApiResponse<UserDTO>(false, "User not found.", null);
        }

        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.Email = request.Email;
        user.RoleId = request.RoleId;
        user.UpdatedAt = DateTime.UtcNow;

        User? updatedUser = await _userRepository.UpdateAsync(user);
        if (updatedUser == null)
        {
            return new ApiResponse<UserDTO>(false, "Failed to update user.", null);
        }

        UserDTO userDTO = new UserDTO(
            updatedUser.Id.ToString(),
            updatedUser.FirstName,
            updatedUser.LastName,
            updatedUser.Email,
            updatedUser.Role.Name,
            updatedUser.RoleId
        );

        return new ApiResponse<UserDTO>(true, "User updated successfully.", userDTO);
    }


}
