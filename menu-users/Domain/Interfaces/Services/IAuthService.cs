using System;
using menu_users.Application.DTOs.Auth;

namespace menu_users.Domain.Interfaces.Services;

public interface IAuthService
{
    Task<AuthResult> LoginAsync(LoginRequest request);
    Task<AuthResult> SignInAsync(SignInRequest request);
}
