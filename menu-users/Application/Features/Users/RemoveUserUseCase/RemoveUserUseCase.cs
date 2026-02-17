using System;
using menu_users.Application.DTOs.ApiResponse;
using menu_users.Domain.Interfaces.Features.User;
using menu_users.Domain.Interfaces.Repositories;

namespace menu_users.Application.Features.Users.RemoveUserUseCase;

public class RemoveUserUseCase : IRemoveUserUseCase
{
    private readonly IUserRepository _userRepository;

    public RemoveUserUseCase(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    public async Task<ApiResponse<bool>> Execute(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
        {
            return new ApiResponse<bool>(false, "User not found.", false);
        }
        await _userRepository.DeleteAsync(user);
        return new ApiResponse<bool>(true, "User removed successfully.", true);
    }
}
