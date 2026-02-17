using System;
using menu_users.Application.DTOs.ApiResponse;
using menu_users.Application.DTOs.User;

namespace menu_users.Domain.Interfaces.Features.User;

public interface IGeneratePasswordUseCase
{
    Task<ApiResponse<GeneratedPasswordResponse>> Execute(Guid id);
}
