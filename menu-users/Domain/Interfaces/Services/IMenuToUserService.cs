using System;
using menu_users.Application.DTOs.ApiResponse;
using menu_users.Application.DTOs.MenuToUser;
using menu_users.Domain.Entities;

namespace menu_users.Domain.Interfaces.Services;

public interface IMenuToUserService
{
    Task<ApiResponse<MenuToUserDTO>> SetDefaultMenusByHierarchyAsync(Guid userId);
    Task<ApiResponse<MenuToUserDTO>> GetMenusByUserIdAsync(Guid userId);
    Task<ApiResponse<MenuToUserDTO?>> AddMenuToUserAsync(Guid userId, int menuId);
    Task<ApiResponse<bool>> RemoveMenuFromUserAsync(Guid userId, int menuId);
    Task<ApiResponse<bool>> RemoveAllMenusFromUserAsync(Guid userId);
}
