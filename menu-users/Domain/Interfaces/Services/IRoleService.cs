using System;
using menu_users.Application.DTOs.ApiResponse;
using menu_users.Application.DTOs.Role;

namespace menu_users.Domain.Interfaces.Services;

public interface IRoleService
{
    Task<ApiResponse<IEnumerable<RoleResponse>>> GetAllRolesAsync();
    Task<ApiResponse<RoleResponse>> GetRoleByIdAsync(int id);
    Task<ApiResponse<RoleResponse>> CreateRoleAsync(RoleCreateRequest request);
    Task<ApiResponse<RoleResponse>> UpdateRoleAsync(int id, RoleUpdateRequest request);
    Task<ApiResponse<bool>> DeleteRoleAsync(int id);
}
