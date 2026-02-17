using System;
using menu_users.Application.DTOs.ApiResponse;
using menu_users.Application.DTOs.Role;
using menu_users.Domain.Entities;
using menu_users.Domain.Interfaces.Features.Roles;
using menu_users.Domain.Interfaces.Repositories;

namespace menu_users.Application.Features.Roles.UpdateRoleUseCase;

public class UpdateRoleUseCase : IUpdateRoleUseCase
{
    private readonly IRoleRepository _roleRepository;

    public UpdateRoleUseCase(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<ApiResponse<RoleDTO>> Execute(int id, UpdateRoleRequest request)
    {
        Role? role = await _roleRepository.GetByIdAsync(id);

        if (role == null)
        {
            return new ApiResponse<RoleDTO>(false, "Role not found.", null);
        }

        role.Name = request.Name;
        role.Hierarchy = request.Hierarchy;
        role.UpdatedAt = DateTime.UtcNow;

        Role? updatedRole = await _roleRepository.UpdateAsync(role);

        if (updatedRole == null)
        {
            return new ApiResponse<RoleDTO>(false, "Failed to update role.", null);
        }

        RoleDTO response = new RoleDTO
        (
            updatedRole.Id,
            updatedRole.Name,
            updatedRole.Hierarchy,
            updatedRole.IsActive,
            updatedRole.CreatedAt,
            updatedRole.UpdatedAt
        );

        return new ApiResponse<RoleDTO>(true, null, response);
    }
}
