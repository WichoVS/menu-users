using System;
using menu_users.Application.DTOs.ApiResponse;
using menu_users.Application.DTOs.Auth;
using menu_users.Application.DTOs.User;
using menu_users.Domain.Entities;
using menu_users.Domain.Interfaces.Repositories;
using menu_users.Domain.Interfaces.Services;
using menu_users.Domain.Interfaces.Users;

namespace menu_users.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public UserService(IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<ApiResponse<UserDTO>> CreateUserAsync(SignInRequest request)
    {
        User? user = await _userRepository.GetByEmailAsync(request.Email);

        if (user != null)
        {
            return new ApiResponse<UserDTO>(false, "User with the same email already exists.", null);
        }

        string hashedPassword = _passwordHasher.Hash(getRandomGeneratedPassword());


        User newUser = new User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Password = hashedPassword, // In a real application, hash the password
            RoleId = request.RoleId,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

    }

    public Task<ApiResponse<bool>> DeleteUserAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<ApiResponse<IEnumerable<UserDTO>>> GetAllUsersAsync()
    {
        throw new NotImplementedException();
    }

    public Task<ApiResponse<UserDTO>> GetUserByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<ApiResponse<UserDTO>> UpdateUserAsync(int id, UpdateUserRequest request)
    {
        throw new NotImplementedException();
    }

    public string getRandomGeneratedPassword()
    {
        // Genera una string aleatoria de 12 caracteres que incluya letras mayúsculas, minúsculas, números y símbolos
        const string upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string lower = "abcdefghijklmnopqrstuvwxyz";
        const string digits = "0123456789";
        const string symbols = "!@#$%^&*()-_=+[]{}|;:,.<>?";
        const string allChars = upper + lower + digits + symbols;

        var random = new Random();
        char[] password = new char[12];

        for (int i = 0; i < password.Length; i++)
        {
            password[i] = allChars[random.Next(allChars.Length)];
        }

        return new string(password);
    }
}
