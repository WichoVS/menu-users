using System;
using menu_users.Application.DTOs.ApiResponse;
using menu_users.Application.DTOs.Menu;
using menu_users.Domain.Entities;
using menu_users.Domain.Interfaces.Features.Menus;
using menu_users.Domain.Interfaces.Repositories;

namespace menu_users.Application.Features.Menus.UpdateMenuUseCase;

public class UpdateMenuUseCase : IUpdateMenuUseCase
{
    private readonly IMenuRepository _menuRepository;

    public UpdateMenuUseCase(IMenuRepository menuRepository)
    {
        _menuRepository = menuRepository;
    }
    public async Task<ApiResponse<MenuDTO>> Execute(int id, UpdateMenuRequest request)
    {
        Menu? menu = await _menuRepository.GetByIdAsync(id);
        if (menu == null)
        {
            return new ApiResponse<MenuDTO>(false, "Menu not found.", null);
        }

        menu.Name = request.Name;
        menu.Url = request.Url;
        menu.IsMain = request.IsMain;
        menu.ParentMenuId = request.ParentId;
        menu.MinimumHierarchy = request.MinHierarchy;
        menu.UpdatedAt = DateTime.UtcNow;

        Menu? updatedMenu = await _menuRepository.UpdateAsync(menu);
        if (updatedMenu == null)
        {
            return new ApiResponse<MenuDTO>(false, "Failed to update menu.", null);
        }

        MenuDTO response = new MenuDTO
        (
            IdMenu: updatedMenu.Id,
            Name: updatedMenu.Name,
            Url: updatedMenu.Url,
            IsMain: updatedMenu.IsMain,
            ParentId: updatedMenu.ParentMenuId,
            MinHierarchy: updatedMenu.MinimumHierarchy,
            Children: Array.Empty<MenuDTO>()
        );

        return new ApiResponse<MenuDTO>(true, null, response);
    }
}
