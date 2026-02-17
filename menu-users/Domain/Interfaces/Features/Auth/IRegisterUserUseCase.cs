using System;
using menu_users.Application.DTOs.Auth;

namespace menu_users.Domain.Interfaces.Features.Auth;

public interface IRegisterUserUseCase
{
    Task<AuthResult> Execute(RegisterUserRequest request);
}
