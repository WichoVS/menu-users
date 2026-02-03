using System;
using menu_users.Application.DTOs.ApiResponse;
using menu_users.Application.DTOs.Menu;
using menu_users.Domain.Entities;
using menu_users.Domain.Interfaces.Repositories;
using menu_users.Domain.Interfaces.Services;

namespace menu_users.Application.Services;

public class MenuService : IMenuService
{
    private readonly IMenuRepository _menuRepository;
    private readonly IUserService _userService;

    public MenuService(IMenuRepository menuRepository, IUserService userService)
    {
        _menuRepository = menuRepository;
        _userService = userService;
    }

    public async Task<ApiResponse<MenuDTO>> CreateMenuAsync(CreateMenu request)
    {
        Menu newMenu = new Menu
        {
            Name = request.Name,
            IsMain = request.IsMain,
            ParentMenuId = request.ParentId,
            Url = request.Url,
            MinimumHierarchy = request.MinHierarchy
        };

        Menu createdMenu = await _menuRepository.AddAsync(newMenu);

        MenuDTO response = new MenuDTO
        (
            IdMenu: createdMenu.Id,
            Name: createdMenu.Name,
            Route: createdMenu.Url,
            IsMain: createdMenu.IsMain.ToString(),
            ParentId: createdMenu.ParentMenuId,
            Children: Array.Empty<MenuDTO>()
        );

        return new ApiResponse<MenuDTO>(true, null, response);
    }

    public async Task<ApiResponse<bool>> DeleteMenuAsync(int id)
    {
        Menu? menu = await _menuRepository.GetByIdAsync(id);

        if (menu == null)
        {
            return new ApiResponse<bool>(false, "Menu not found.", false);
        }

        bool result = await _menuRepository.DeleteAsync(menu);
        return new ApiResponse<bool>(result, result ? null : "Failed to delete menu.", result);
    }

    public async Task<ApiResponse<IEnumerable<MenuDTO>>> GetAllMenusAsync()
    {
        IEnumerable<Menu> menus = await _menuRepository.GetAllAsync();

        // Por ahora se tiene pensado menu de dos niveles
        var menuDTOs = menus.Select(m => new MenuDTO
        (
            IdMenu: m.Id,
            Name: m.Name,
            Route: m.Url,
            IsMain: m.IsMain.ToString(),
            ParentId: m.ParentMenuId,
            Children: m.SubMenus != null
                ? m.SubMenus.Where(sm => sm.IsActive).Select(sm => new MenuDTO
                (
                    IdMenu: sm.Id,
                    Name: sm.Name,
                    Route: sm.Url,
                    IsMain: sm.IsMain.ToString(),
                    ParentId: sm.ParentMenuId,
                    Children: Array.Empty<MenuDTO>()
                )).ToArray()
                : Array.Empty<MenuDTO>()
        ));

        return new ApiResponse<IEnumerable<MenuDTO>>(true, null, menuDTOs);
    }

    public async Task<ApiResponse<IEnumerable<MenuDTO>>> GetMenuByUserIdAsync(int userId)
    {
        User? user = await _menuRepository.GetUserByIdAsync(userId);
    }

    public async Task<ApiResponse<MenuDTO>> UpdateMenuAsync(int id, MenuUpdate request)
    {
        throw new NotImplementedException();
    }

    public async Task<ApiResponse<IEnumerable<MenuDTO>>> GetAllMenusAsync(int userId)
    {
        throw new NotImplementedException();
    }
}
