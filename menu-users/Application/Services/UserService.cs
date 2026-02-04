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

        User createdUser = await _userRepository.AddAsync(newUser);

        UserDTO userDTO = new UserDTO(
            createdUser.Id.ToString(),
            createdUser.FirstName,
            createdUser.LastName,
            createdUser.Email,
            createdUser.RoleId
        );

        return new ApiResponse<UserDTO>(true, "User created successfully.", userDTO);
    }

    public async Task<ApiResponse<bool>> DeleteUserAsync(string id)
    {
        User? user = await _userRepository.GetByIdAsync(id);

        if (user == null)
        {
            return new ApiResponse<bool>(false, "User not found.", false);
        }

        bool result = await _userRepository.DeleteAsync(user);

        return new ApiResponse<bool>(true, "User deleted successfully.", result);
    }

    public async Task<ApiResponse<IEnumerable<UserDTO>>> GetAllUsersAsync()
    {
        IEnumerable<User> users = await _userRepository.GetAllAsync();

        var userDTOs = users.Select(user => new UserDTO(
            user.Id.ToString(),
            user.FirstName,
            user.LastName,
            user.Email,
            user.RoleId
        ));

        return new ApiResponse<IEnumerable<UserDTO>>(true, "Users retrieved successfully.", userDTOs);
    }

    public async Task<ApiResponse<UserDTO>> GetUserByIdAsync(string id)
    {
        User? user = await _userRepository.GetByIdAsync(id);
        if (user == null)
        {
            return new ApiResponse<UserDTO>(false, "User not found.", null);
        }

        UserDTO userDTO = new UserDTO(
            user.Id.ToString(),
            user.FirstName,
            user.LastName,
            user.Email,
            user.RoleId
        );

        return new ApiResponse<UserDTO>(true, "User retrieved successfully.", userDTO);
    }

    public async Task<ApiResponse<UserDTO>> UpdateUserAsync(string id, UpdateUserRequest request)
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
            updatedUser.RoleId
        );

        return new ApiResponse<UserDTO>(true, "User updated successfully.", userDTO);
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
