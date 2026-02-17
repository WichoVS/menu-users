using System;
using menu_users.Application.DTOs.ApiResponse;
using menu_users.Application.DTOs.Role;
using menu_users.Domain.Entities;
using menu_users.Domain.Interfaces.Features.Roles;
using menu_users.Domain.Interfaces.Repositories;

namespace menu_users.Application.Features.Roles.GetRoleByIdUseCase;

public class GetRoleByIdUseCase : IGetRoleByIdUseCase
{
    private readonly IRoleRepository _roleRepository;

    public GetRoleByIdUseCase(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<ApiResponse<RoleDTO>> Execute(int id)
    {
        Role? role = await _roleRepository.GetByIdAsync(id);

        if (role == null)
        {
            return new ApiResponse<RoleDTO>(false, "Role not found.", null);
        }

        RoleDTO response = new RoleDTO
        (
            role.Id,
            role.Name,
            role.Hierarchy,
            role.IsActive,
            role.CreatedAt,
            role.UpdatedAt
        );

        return new ApiResponse<RoleDTO>(true, null, response);
    }
}
