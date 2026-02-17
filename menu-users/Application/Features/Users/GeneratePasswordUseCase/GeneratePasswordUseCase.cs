using System;
using menu_users.Application.DTOs.ApiResponse;
using menu_users.Application.DTOs.User;
using menu_users.Domain.Entities;
using menu_users.Domain.Interfaces.Features.User;
using menu_users.Domain.Interfaces.Repositories;
using menu_users.Domain.Interfaces.Users;

namespace menu_users.Application.Features.Users.GeneratePasswordUseCase;

public class GeneratePasswordUseCase : IGeneratePasswordUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public GeneratePasswordUseCase(IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }
    public async Task<ApiResponse<GeneratedPasswordResponse>> Execute(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id.ToString());
        if (user == null)
        {
            return new ApiResponse<GeneratedPasswordResponse>(false, "User not found.", null);
        }

        string newGeneratedPassword = _passwordHasher.GetRandomGeneratedPassword();
        string hashedPassword = _passwordHasher.Hash(newGeneratedPassword);
        user.Password = hashedPassword;
        user.UpdatedAt = DateTime.UtcNow;

        User? updatedUser = await _userRepository.UpdateAsync(user);
        if (updatedUser == null)
        {
            return new ApiResponse<GeneratedPasswordResponse>(false, "Failed to update user password.", null);
        }

        GeneratedPasswordResponse gPassResponse = new GeneratedPasswordResponse(newGeneratedPassword);

        return new ApiResponse<GeneratedPasswordResponse>(true, "User password generated successfully.", gPassResponse);
    }
}
