using System;
using menu_users.Application.DTOs.ApiResponse;

namespace menu_users.Domain.Interfaces.Features.Roles;

public interface IDeleteRoleUseCase
{
    Task<ApiResponse<bool>> Execute(int id);
}
