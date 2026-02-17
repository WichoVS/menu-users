using System;
using menu_users.Application.DTOs.ApiResponse;

namespace menu_users.Domain.Interfaces.Features.User;

public interface IRemoveUserUseCase
{
    Task<ApiResponse<bool>> Execute(Guid id);
}
