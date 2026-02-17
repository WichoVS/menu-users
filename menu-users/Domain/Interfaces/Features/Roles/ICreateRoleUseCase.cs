using System;
using menu_users.Application.DTOs.ApiResponse;
using menu_users.Application.DTOs.Role;
using menu_users.Application.Features.Roles.CreateRoleUseCase;

namespace menu_users.Domain.Interfaces.Features.Roles;

public interface ICreateRoleUseCase
{
    Task<ApiResponse<RoleDTO>> Execute(CreateRoleRequest request);
}
