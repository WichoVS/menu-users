using System;
using menu_users.Application.DTOs.ApiResponse;
using menu_users.Application.DTOs.Role;
using menu_users.Domain.Entities;
using menu_users.Domain.Interfaces.Features.Roles;
using menu_users.Domain.Interfaces.Repositories;

namespace menu_users.Application.Features.Roles.CreateRoleUseCase;

public class CreateRoleUseCase : ICreateRoleUseCase
{
    private readonly IRoleRepository _roleRepository;

    public CreateRoleUseCase(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }
    public async Task<ApiResponse<RoleDTO>> Execute(CreateRoleRequest request)
    {
        Role? role = await _roleRepository.GetByNameAsync(request.Name);

        if (role != null)
        {
            return new ApiResponse<RoleDTO>(false, "Role with the same name already exists.", null);
        }

        Role newRole = new Role
        {
            Name = request.Name,
            Hierarchy = request.Hierarchy,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _roleRepository.AddAsync(newRole);
        RoleDTO response = new RoleDTO
        (
            newRole.Id,
            newRole.Name,
            newRole.Hierarchy,
            newRole.IsActive,
            newRole.CreatedAt,
            newRole.UpdatedAt
        );

        return new ApiResponse<RoleDTO>(true, null, response);
    }
}
