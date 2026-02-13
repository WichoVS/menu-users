using System;
using menu_users.Application.DTOs.Auth;
using menu_users.Domain.Entities;
using menu_users.Domain.Interfaces.Repositories;
using menu_users.Domain.Interfaces.Services;
using menu_users.Domain.Interfaces.Users;
using menu_users.Infrastructure.Repositories;

namespace menu_users.Application.Services;

public class AuthService : IAuthService
{
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly ITokenService _tokenService;
    private readonly IMenuToUserService _menuToUserService;

    public AuthService(
        IPasswordHasher passwordHasher,
        IUserRepository userRepository,
        IRoleRepository roleRepository,
        ITokenService tokenService,
        IMenuToUserService menuToUserService)
    {
        _passwordHasher = passwordHasher;
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _tokenService = tokenService;
        _menuToUserService = menuToUserService;
    }

    public async Task<AuthResult> LoginAsync(LoginRequest request)
    {
        // Buscar usuario
        User? userByEmail = await _userRepository.GetByEmailAsync(request.Email);

        if (userByEmail == null)
        {
            return new AuthResult(false, "Usuario o contraseña incorrectos", null);
        }

        if (userByEmail.IsActive == false)
        {
            return new AuthResult(false, "Usuario inactivo", null);
        }

        if (!_passwordHasher.Verify(userByEmail.Password, request.Password))
        {
            return new AuthResult(false, "Usuario o contraseña incorrectos", null);
        }

        var token = _tokenService.GenerateToken(userByEmail, userByEmail.Role);

        var authResponse = new AuthResponse
        (
            id: userByEmail.Id.ToString(),
            email: userByEmail.Email,
            firstName: userByEmail.FirstName,
            lastName: userByEmail.LastName,
            role: userByEmail.Role.Name,
            token: token
        );

        return new AuthResult(true, null, authResponse);
    }

    public async Task<AuthResult> SignInAsync(SignInRequest request)
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