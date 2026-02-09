using System;
using menu_users.Application.DTOs.ApiResponse;
using menu_users.Application.DTOs.Menu;

namespace menu_users.Domain.Interfaces.Services;

public interface IMenuService
{
    Task<ApiResponse<IEnumerable<MenuDTO>>> GetMenuByUserIdAsync(string userId);
    Task<ApiResponse<MenuDTO>> CreateMenuAsync(CreateMenu request);
    Task<ApiResponse<MenuDTO>> UpdateMenuAsync(int id, MenuUpdate request);
    Task<ApiResponse<bool>> DeleteMenuAsync(int id);
    Task<ApiResponse<IEnumerable<MenuDTO>>> GetAllMenusAsync();
    Task<ApiResponse<MenuDTO>> GetMenuByIdAsync(int id);

    Task<ApiResponse<IEnumerable<MenuDTO>>> GetMenusByHierarchyAsync(int hierarchy);

}
