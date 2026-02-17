using System;
using menu_users.Application.DTOs.ApiResponse;
using menu_users.Domain.Entities;
using menu_users.Domain.Interfaces.Features.User;
using menu_users.Domain.Interfaces.Repositories;
using menu_users.Domain.Interfaces.Users;

namespace menu_users.Application.Features.Users.UpdatePasswordUseCase;

public class UpdatePasswordUseCase : IUpdatePasswordUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    public UpdatePasswordUseCase(IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<ApiResponse<bool>> Execute(Guid id, string newPassword)
    {
        User? user = await _userRepository.GetByIdAsync(id);
        if (user == null)
        {
            return new ApiResponse<bool>(false, "User not found.", false);
        }

        user.Password = _passwordHasher.Hash(newPassword);
        await _userRepository.UpdateAsync(user);
        return new ApiResponse<bool>(true, "Password updated successfully.", true);
    }
}
