using System;
using menu_users.Application.DTOs.Auth;
using menu_users.Domain.Entities;
using menu_users.Domain.Interfaces.Features.Auth;
using menu_users.Domain.Interfaces.Repositories;
using menu_users.Domain.Interfaces.Users;

namespace menu_users.Application.Features.Auth.LoginUserUseCase;

public class LoginUserUseCase : ILoginUserUseCase
{

    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;
    private readonly IUserRepository _userRepository;

    public LoginUserUseCase(
        IPasswordHasher passwordHasher,
        ITokenService tokenService,
        IUserRepository userRepository)
    {
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
        _userRepository = userRepository;
    }

    public async Task<AuthResult> Execute(LoginUserRequest request)
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

}
