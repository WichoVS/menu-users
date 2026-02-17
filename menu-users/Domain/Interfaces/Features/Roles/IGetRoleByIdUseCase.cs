using System;
using menu_users.Application.DTOs.ApiResponse;
using menu_users.Application.DTOs.Role;

namespace menu_users.Domain.Interfaces.Features.Roles;

public interface IGetRoleByIdUseCase
{
    Task<ApiResponse<RoleDTO>> Execute(int id);
}
