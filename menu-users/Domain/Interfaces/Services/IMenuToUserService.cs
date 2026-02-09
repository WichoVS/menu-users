using System;
using menu_users.Application.DTOs.ApiResponse;
using menu_users.Application.DTOs.MenuToUser;
using menu_users.Domain.Entities;

namespace menu_users.Domain.Interfaces.Services;

public interface IMenuToUserService
{
    Task<ApiResponse<IEnumerable<MenuToUserDTO>>> SetDefaultMenusByHierarchyAsync(Guid userId, IEnumerable<Menu> menus);
    Task<ApiResponse<IEnumerable<MenuToUserDTO>>> GetMenusByUserIdAsync(Guid userId);
    Task<ApiResponse<MenuToUserDTO?>> AddMenuToUserAsync(MenuToUser menuToUser);
    Task<ApiResponse<bool>> RemoveMenuFromUserAsync(Guid userId, int menuId);
}
