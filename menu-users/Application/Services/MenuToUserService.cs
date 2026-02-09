using System;
using menu_users.Application.DTOs.ApiResponse;
using menu_users.Application.DTOs.Menu;
using menu_users.Application.DTOs.MenuToUser;
using menu_users.Application.DTOs.User;
using menu_users.Domain.Entities;
using menu_users.Domain.Interfaces.Repositories;
using menu_users.Domain.Interfaces.Services;

namespace menu_users.Application.Services;

public class MenuToUserService : IMenuToUserService
{
    private readonly IMenuToUserRepository _menuToUserRepository;
    private readonly IMenuService _menuService;
    private readonly IUserService _userService;

    public MenuToUserService(IMenuToUserRepository menuToUserRepository, IMenuService menuService, IUserService userService)
    {
        _menuToUserRepository = menuToUserRepository;
        _menuService = menuService;
        _userService = userService;
    }

    public async Task<ApiResponse<MenuToUserDTO?>> AddMenuToUserAsync(MenuToUser menuToUser)
    {
        ApiResponse<UserDTO> user = await _userService.GetUserByIdAsync(menuToUser.UserId.ToString());
        if (!user.Success)
        {
            return new ApiResponse<MenuToUserDTO?>(false, user.Error, null);
        }

        ApiResponse<MenuDTO> menu = await _menuService.GetMenuByIdAsync(menuToUser.MenuId);
        if (!menu.Success)
        {
            return new ApiResponse<MenuToUserDTO?>(false, "Menu not found", null);
        }

        menuToUser.CreatedAt = DateTime.UtcNow;
        MenuToUser addedMenuToUser = await _menuToUserRepository.AddAsync(menuToUser);
        MenuToUserDTO menuToUserDTO = new MenuToUserDTO
        (
            IdMenuToUser: addedMenuToUser.Id,
            UserId: addedMenuToUser.UserId,
            Menu:
            [
                new MenuDTO
                (
                    IdMenu: menu.Data!.IdMenu,
                    Name: menu.Data.Name,
                    Url: menu.Data.Url,
                    IsMain: menu.Data.IsMain,
                    ParentId: menu.Data.ParentId,
                    MinHierarchy: menu.Data.MinHierarchy,
                    Children: Array.Empty<MenuDTO>()
                )
            ]
        );

        return new ApiResponse<MenuToUserDTO?>(true, "Menu assigned to user successfully", menuToUserDTO);
    }

    public async Task<ApiResponse<IEnumerable<MenuToUserDTO>>> GetMenusByUserIdAsync(Guid userId)
    {
        // Obtener los menús asignados a un usuario específico y agruparlos con su padre, si no tiene acceso a padre pero si al hijo,
        // no se cuenta y se omite

        IEnumerable<MenuToUser> menus = await _menuToUserRepository.GetMenusByUserIdAsync(userId);


        IEnumerable<MenuToUserDTO> menuDTOs = menus.Select(m => new MenuToUserDTO
        (
            IdMenuToUser: m.Id,
            UserId: m.UserId,
            Menu:
            [
                new MenuDTO
                (
                    IdMenu: m.Menu.Id,
                    Name: m.Menu.Name,
                    Url: m.Menu.Url,
                    IsMain: m.Menu.IsMain,
                    ParentId: m.Menu.ParentMenuId,
                    MinHierarchy: m.Menu.MinimumHierarchy,
                    Children: Array.Empty<MenuDTO>()
                )
            ]
        )).ToArray();
        return new ApiResponse<IEnumerable<MenuToUserDTO>>(true, "Menus retrieved successfully", menuDTOs);
    }

    public async Task<ApiResponse<bool>> RemoveMenuFromUserAsync(Guid userId, int menuId)
    {
        bool result = await _menuToUserRepository.DeleteAsync(new MenuToUser { UserId = userId, MenuId = menuId });


        if (result)
        {
            return new ApiResponse<bool>(true, "Menu removed from user successfully", true);
        }
        else
        {
            return new ApiResponse<bool>(false, "Menu not found for the user", false);
        }
    }

    public async Task<ApiResponse<IEnumerable<MenuToUserDTO>>> SetDefaultMenusByHierarchyAsync(Guid userId, IEnumerable<Menu> menus)
    {


        await _menuService.GetMenusByHierarchyAsync(userId, menus);
    }
}
