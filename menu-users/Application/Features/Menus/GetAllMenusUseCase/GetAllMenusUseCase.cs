using System;
using menu_users.Application.DTOs.ApiResponse;
using menu_users.Application.DTOs.Menu;
using menu_users.Domain.Entities;
using menu_users.Domain.Interfaces.Features.Menus;
using menu_users.Domain.Interfaces.Repositories;

namespace menu_users.Application.Features.Menus.GetAllMenusUseCase;

public class GetAllMenusUseCase : IGetAllMenusUseCase
{
    private readonly IMenuRepository _menuRepository;

    public GetAllMenusUseCase(IMenuRepository menuRepository)
    {
        _menuRepository = menuRepository;
    }

    public async Task<ApiResponse<IEnumerable<MenuDTO>>> Execute()
    {
        IEnumerable<Menu> menus = await _menuRepository.GetAllAsync();

        // Por ahora se tiene pensado menu de dos niveles
        var menuDTOs = menus.Where(m => m.IsMain).Select(m => new MenuDTO
        (
            IdMenu: m.Id,
            Name: m.Name,
            Url: m.Url,
            IsMain: m.IsMain,
            ParentId: m.ParentMenuId,
            MinHierarchy: m.MinimumHierarchy,
            Children: m.SubMenus != null
                ? m.SubMenus.Where(sm => sm.IsActive).Select(sm => new MenuDTO
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
        ));

        return new ApiResponse<IEnumerable<MenuDTO>>(true, null, menuDTOs);
    }
}
