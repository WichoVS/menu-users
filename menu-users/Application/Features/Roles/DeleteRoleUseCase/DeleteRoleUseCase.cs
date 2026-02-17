using System;
using menu_users.Application.DTOs.ApiResponse;
using menu_users.Domain.Entities;
using menu_users.Domain.Interfaces.Features.Roles;
using menu_users.Domain.Interfaces.Repositories;

namespace menu_users.Application.Features.Roles.DeleteRoleUseCase;

public class DeleteRoleUseCase : IDeleteRoleUseCase
{
    private readonly IRoleRepository _roleRepository;

    public DeleteRoleUseCase(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }
    public async Task<ApiResponse<bool>> Execute(int id)
    {
        Role? role = await _roleRepository.GetByIdAsync(id);

        if (role == null)
        {
            return new ApiResponse<bool>(false, "Role not found.", false);
        }

        bool result = await _roleRepository.DeleteAsync(role);

        return new ApiResponse<bool>(true, null, result);
    }
}
