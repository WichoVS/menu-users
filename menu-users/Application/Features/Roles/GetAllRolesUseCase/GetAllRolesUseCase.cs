using System;
using menu_users.Application.DTOs.ApiResponse;
using menu_users.Application.DTOs.Role;
using menu_users.Domain.Entities;
using menu_users.Domain.Interfaces.Features.Roles;
using menu_users.Domain.Interfaces.Repositories;

namespace menu_users.Application.Features.Roles.GetAllRolesUseCase;

public class GetAllRolesUseCase : IGetAllRolesUseCase
{
    private readonly IRoleRepository _roleRepository;

    public GetAllRolesUseCase(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }
    public async Task<ApiResponse<IEnumerable<RoleDTO>>> Execute()
    {
        // Modificarlo luego para solo obtener los roles del usuario que hizo el request para que no pueda ver roles de mayor jerarquía
        // ya que tengo pensado usar este endpoint para llenar los select de roles en el front
        IEnumerable<Role> roles = await _roleRepository.GetAllAsync();
        var roleResponses = roles.Select(role => new RoleDTO
        (
            role.Id,
            role.Name,
            role.Hierarchy,
            role.IsActive,
            role.CreatedAt,
            role.UpdatedAt
        ));

        return new ApiResponse<IEnumerable<RoleDTO>>(true, null, roleResponses);
    }
}
