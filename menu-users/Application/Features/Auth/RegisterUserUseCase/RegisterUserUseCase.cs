using System;
using menu_users.Application.DTOs.Auth;
using menu_users.Domain.Interfaces.Features.Auth;
using menu_users.Domain.Interfaces.Repositories;
using menu_users.Domain.Interfaces.Services;
using menu_users.Domain.Interfaces.Users;
using menu_users.Domain.Entities;

namespace menu_users.Application.Features.Auth.RegisterUser;

public class RegisterUserUseCase : IRegisterUserUseCase
{
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IMenuToUserService _menuToUserService;

    public RegisterUserUseCase(
        IPasswordHasher passwordHasher,
        ITokenService tokenService,
        IUserRepository userRepository,
        IRoleRepository roleRepository,
        IMenuToUserService menuToUserService)
    {
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _menuToUserService = menuToUserService;
    }

    public async Task<AuthResult> Execute(RegisterUserRequest request)
    {
        User? existingUser = await _userRepository.GetByEmailAsync(request.Email);
        if (existingUser != null)
        {
            return new AuthResult(false, "El correo electrónico ya está en uso", null);
        }

        Role? role = await _roleRepository.GetLowestByHierarchyAsync();
        if (role == null || role.IsActive == false)
        {
            return new AuthResult(false, "Rol no válido", null);
        }

        var hashedPassword = _passwordHasher.Hash(request.Password);
        var newUser = new User()
        {
            Id = Guid.NewGuid(),
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Password = hashedPassword,
            RoleId = role.Id,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            IsActive = true
        };

        await _userRepository.AddAsync(newUser);

        await _menuToUserService.SetDefaultMenusByHierarchyAsync(newUser.Id);

        var token = _tokenService.GenerateToken(newUser, role);

        var authResponse = new AuthResponse
        (
            token: token,
            id: newUser.Id.ToString(),
            email: newUser.Email,
            firstName: newUser.FirstName,
            lastName: newUser.LastName,
            role: role.Name
        );

        return new AuthResult(true, null, authResponse);
    }
}
