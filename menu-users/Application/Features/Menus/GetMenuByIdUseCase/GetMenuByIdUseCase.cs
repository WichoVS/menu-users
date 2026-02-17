using System;
using menu_users.Application.DTOs.ApiResponse;
using menu_users.Application.DTOs.Menu;
using menu_users.Domain.Entities;
using menu_users.Domain.Interfaces.Features.Menus;
using menu_users.Domain.Interfaces.Repositories;

namespace menu_users.Application.Features.Menus.GetMenuByIdUseCase;

public class GetMenuByIdUseCase : IGetMenuByIdUseCase
{
    private readonly IMenuRepository _menuRepository;

    public GetMenuByIdUseCase(IMenuRepository menuRepository)
    {
        _menuRepository = menuRepository;
    }
    public async Task<ApiResponse<MenuDTO>> Execute(int id)
    {
        Menu? menu = await _menuRepository.GetByIdAsync(id);
        if (menu == null)
        {
            return new ApiResponse<MenuDTO>(false, "Menu not found.", null);
        }

        MenuDTO response = new MenuDTO
        (
            IdMenu: menu.Id,
            Name: menu.Name,
            Url: menu.Url,
            IsMain: menu.IsMain,
            ParentId: menu.ParentMenuId,
            MinHierarchy: menu.MinimumHierarchy,
            Children: menu.SubMenus != null
                ? menu.SubMenus.Where(sm => sm.IsActive).Select(sm => new MenuDTO
                (
                    IdMenu: sm.Id,
                    Name: sm.Name,
                    Url: sm.Url,
                    IsMain: sm.IsMain,
                    ParentId: sm.ParentMenuId,
                    MinHierarchy: sm.MinimumHierarchy,
                    Children: Array.Empty<MenuDTO>()
                )).ToArray()
                : Array.Empty<MenuDTO>()
        );

        return new ApiResponse<MenuDTO>(true, null, response);
    }
}
