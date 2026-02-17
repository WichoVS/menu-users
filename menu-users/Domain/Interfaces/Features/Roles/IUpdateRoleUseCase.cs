using System;
using menu_users.Application.DTOs.ApiResponse;
using menu_users.Application.DTOs.Role;
using menu_users.Application.Features.Roles.UpdateRoleUseCase;

namespace menu_users.Domain.Interfaces.Features.Roles;

public interface IUpdateRoleUseCase
{
    Task<ApiResponse<RoleDTO>> Execute(int id, UpdateRoleRequest request);
}
