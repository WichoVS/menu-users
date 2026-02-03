using System;
using menu_users.Application.DTOs.ApiResponse;
using menu_users.Application.DTOs.Role;
using menu_users.Domain.Entities;
using menu_users.Domain.Interfaces.Repositories;
using menu_users.Domain.Interfaces.Services;

namespace menu_users.Application.Services;

public class RoleService : IRoleService
{
    private readonly IRoleRepository _roleRepository;

    public RoleService(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<ApiResponse<RoleResponse>> CreateRoleAsync(RoleCreateRequest request)
    {
        Role? role = await _roleRepository.GetByNameAsync(request.Name);

        if (role != null)
        {
            return new ApiResponse<RoleResponse>(false, "Role with the same name already exists.", null);
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
        RoleResponse response = new RoleResponse
        (
            newRole.Id,
            newRole.Name,
            newRole.Hierarchy,
            newRole.IsActive,
            newRole.CreatedAt,
            newRole.UpdatedAt
        );

        return new ApiResponse<RoleResponse>(true, null, response);
    }

    public async Task<ApiResponse<bool>> DeleteRoleAsync(int id)
    {
        Role? role = await _roleRepository.GetByIdAsync(id);

        if (role == null)
        {
            return new ApiResponse<bool>(false, "Role not found.", false);
        }

        bool result = await _roleRepository.DeleteAsync(role);

        return new ApiResponse<bool>(true, null, result);
    }

    public async Task<ApiResponse<IEnumerable<RoleResponse>>> GetAllRolesAsync()
    {
        // Modificarlo luego para solo obtener los roles del usuario que hizo el request para que no pueda ver roles de mayor jerarquía
        // ya que tengo pensado usar este endpoint para llenar los select de roles en el front
        IEnumerable<Role> roles = await _roleRepository.GetAllAsync();
        var roleResponses = roles.Select(role => new RoleResponse
        (
            role.Id,
            role.Name,
            role.Hierarchy,
            role.IsActive,
            role.CreatedAt,
            role.UpdatedAt
        ));

        return new ApiResponse<IEnumerable<RoleResponse>>(true, null, roleResponses);
    }

    public async Task<ApiResponse<RoleResponse>> GetRoleByIdAsync(int id)
    {
        Role? role = await _roleRepository.GetByIdAsync(id);

        if (role == null)
        {
            return new ApiResponse<RoleResponse>(false, "Role not found.", null);
        }

        RoleResponse response = new RoleResponse
        (
            role.Id,
            role.Name,
            role.Hierarchy,
            role.IsActive,
            role.CreatedAt,
            role.UpdatedAt
        );

        return new ApiResponse<RoleResponse>(true, null, response);
    }

    public async Task<ApiResponse<RoleResponse>> UpdateRoleAsync(int id, RoleUpdateRequest request)
    {
        Role? role = await _roleRepository.GetByIdAsync(id);

        if (role == null)
        {
            return new ApiResponse<RoleResponse>(false, "Role not found.", null);
        }

        role.Name = request.Name;
        role.Hierarchy = request.Hierarchy;
        role.UpdatedAt = DateTime.UtcNow;

        Role? updatedRole = await _roleRepository.UpdateAsync(role);

        if (updatedRole == null)
        {
            return new ApiResponse<RoleResponse>(false, "Failed to update role.", null);
        }

        RoleResponse response = new RoleResponse
        (
            updatedRole.Id,
            updatedRole.Name,
            updatedRole.Hierarchy,
            updatedRole.IsActive,
            updatedRole.CreatedAt,
            updatedRole.UpdatedAt
        );

        return new ApiResponse<RoleResponse>(true, null, response);
    }
}
