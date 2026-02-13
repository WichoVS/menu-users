using System;
using menu_users.Application.DTOs.ApiResponse;
using menu_users.Application.DTOs.Menu;
using menu_users.Application.DTOs.Role;
using menu_users.Application.DTOs.User;
using menu_users.Domain.Entities;
using menu_users.Domain.Interfaces.Repositories;
using menu_users.Domain.Interfaces.Services;

namespace menu_users.Application.Services;

public class MenuService : IMenuService
{
    private readonly IMenuRepository _menuRepository;
    private readonly IUserService _userService;
    private readonly IRoleService _roleService;

    public MenuService(IMenuRepository menuRepository, IUserService userService, IRoleService roleService)
    {
        _menuRepository = menuRepository;
        _userService = userService;
        _roleService = roleService;
    }

    public async Task<ApiResponse<MenuDTO>> CreateMenuAsync(CreateMenu request)
    {
        Menu newMenu = new Menu
        {
            Name = request.Name,
            IsMain = request.IsMain,
            ParentMenuId = request.ParentId,
            Url = request.Url,
            MinimumHierarchy = request.MinHierarchy,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        Menu createdMenu = await _menuRepository.AddAsync(newMenu);

        MenuDTO response = new MenuDTO
        (
            IdMenu: createdMenu.Id,
            Name: createdMenu.Name,
            Url: createdMenu.Url,
            IsMain: createdMenu.IsMain,
            ParentId: createdMenu.ParentMenuId,
            MinHierarchy: createdMenu.MinimumHierarchy,
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

    public async Task<ApiResponse<MenuDTO>> GetMenuByIdAsync(int id)
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

    public async Task<ApiResponse<IEnumerable<MenuDTO>>> GetMenuByUserIdAsync(string userId)
    {
        ApiResponse<UserDTO> user = await _userService.GetUserByIdAsync(userId);
        if (!user.Success)
        {
            return new ApiResponse<IEnumerable<MenuDTO>>(false, "User not found.", null);
        }

        if (user.Data == null)
        {
            return new ApiResponse<IEnumerable<MenuDTO>>(false, "User data is null.", null);
        }

        ApiResponse<RoleResponse> role = await _roleService.GetRoleByIdAsync(user.Data.RoleId);
        if (!role.Success)
        {
            return new ApiResponse<IEnumerable<MenuDTO>>(false, "Role not found.", null);
        }

        if (role.Data == null)
        {
            return new ApiResponse<IEnumerable<MenuDTO>>(false, "Role data is null.", null);
        }

        IEnumerable<Menu> menus = await _menuRepository.GetMenusByUserHierarchyAsync(role.Data.Hierarchy);

        var menuDTOs = menus.Select(m => new MenuDTO
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

    public async Task<ApiResponse<IEnumerable<MenuDTO>>> GetMenusByHierarchyAsync(int hierarchy)
    {
        IEnumerable<Menu> menus = await _menuRepository.GetMenusByUserHierarchyAsync(hierarchy);

        var menuDTOs = menus.Select(m => new MenuDTO
        (
            IdMenu: m.Id,
            Name: m.Name,
            Url: m.Url,
            IsMain: m.IsMain,
            ParentId: m.ParentMenuId,
            MinHierarchy: m.MinimumHierarchy,
            Children: m.SubMenus.Count > 0
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

    public async Task<ApiResponse<MenuDTO>> UpdateMenuAsync(int id, MenuUpdate request)
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
