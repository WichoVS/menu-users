using System;
using menu_users.Application.DTOs.ApiResponse;
using menu_users.Application.DTOs.Menu;
using menu_users.Application.DTOs.MenuToUser;
using menu_users.Application.DTOs.Role;
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
    private readonly IRoleService _roleService;

    public MenuToUserService(IMenuToUserRepository menuToUserRepository,
    IMenuService menuService,
    IUserService userService,
    IRoleService roleService)
    {
        _menuToUserRepository = menuToUserRepository;
        _menuService = menuService;
        _userService = userService;
        _roleService = roleService;
    }

    public async Task<ApiResponse<MenuToUserDTO?>> AddMenuToUserAsync(Guid userId, int menuId)
    {
        ApiResponse<UserDTO> user = await _userService.GetUserByIdAsync(userId.ToString());
        if (!user.Success)
        {
            return new ApiResponse<MenuToUserDTO?>(false, user.Error, null);
        }

        ApiResponse<MenuDTO> menu = await _menuService.GetMenuByIdAsync(menuId);
        if (!menu.Success)
        {
            return new ApiResponse<MenuToUserDTO?>(false, "Menu not found", null);
        }

        // Verificamos si el menú ya está asignado al usuario
        IEnumerable<MenuToUser> existingMenus = await _menuToUserRepository.GetMenusByUserIdAsync(userId);
        if (existingMenus.Any(m => m.MenuId == menuId))
        {
            return new ApiResponse<MenuToUserDTO?>(true, "Menu already assigned to user", null);
        }

        // En caso de que el menú no esté asignado, procedemos a asignarlo al usuario
        // Primero verificando si es un menú hijo, en ese caso se asigna el menú padre también
        if (menu.Data!.ParentId != null)
        {
            int parentMenuId = menu.Data.ParentId.Value;

            // Verificamos si el menú padre ya está asignado al usuario
            if (!existingMenus.Any(m => m.MenuId == parentMenuId))
            {
                MenuToUser parentMenuToUser = new MenuToUser
                {
                    UserId = userId,
                    MenuId = parentMenuId,
                    CreatedAt = DateTime.UtcNow
                };

                await _menuToUserRepository.AddAsync(parentMenuToUser);
            }
        }

        MenuToUser menuToUser = new MenuToUser
        {
            UserId = userId,
            MenuId = menuId,
            CreatedAt = DateTime.UtcNow
        };

        MenuToUser addedMenuToUser = await _menuToUserRepository.AddAsync(menuToUser);
        MenuToUserDTO menuToUserDTO = new MenuToUserDTO
        (
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

    public async Task<ApiResponse<MenuToUserDTO>> GetMenusByUserIdAsync(Guid userId)
    {
        IEnumerable<MenuToUser> menus = await _menuToUserRepository.GetMenusByUserIdAsync(userId);

        List<MenuDTO> menuDTOs = new List<MenuDTO>();

        foreach (var menu in menus)
        {
            menuDTOs.Add(new MenuDTO
            (
                IdMenu: menu.Menu.Id,
                Name: menu.Menu.Name,
                Url: menu.Menu.Url,
                IsMain: menu.Menu.IsMain,
                ParentId: menu.Menu.ParentMenuId,
                MinHierarchy: menu.Menu.MinimumHierarchy,
                Children: Array.Empty<MenuDTO>()
            ));
        }

        MenuToUserDTO groupedMenus = new MenuToUserDTO(
                UserId: userId,
                Menu: GetGroupedMenus(menuDTOs).ToArray()
        );

        return new ApiResponse<MenuToUserDTO>(true, "Menus retrieved successfully", groupedMenus);
    }

    public Task<ApiResponse<bool>> RemoveAllMenusFromUserAsync(Guid userId)
    {
        throw new NotImplementedException();
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

    public async Task<ApiResponse<MenuToUserDTO>> SetDefaultMenusByHierarchyAsync(Guid userId)
    {
        // Verificamos que el user existe
        ApiResponse<UserDTO> user = await _userService.GetUserByIdAsync(userId.ToString());
        if (!user.Success)
        {
            return new ApiResponse<MenuToUserDTO>(false, user.Error, null);
        }

        // Obtenemos todos los roles para verificar el rol del usuario
        ApiResponse<IEnumerable<RoleResponse>> roleRes = await _roleService.GetAllRolesAsync();
        if (!roleRes.Success)
        {
            return new ApiResponse<MenuToUserDTO>(false, user.Error, null);
        }

        // Verificamos el Role del Usuario
        RoleResponse? roleUser = roleRes.Data!.FirstOrDefault(r => r.Id == user.Data!.RoleId);
        if (roleUser == null)
        {
            return new ApiResponse<MenuToUserDTO>(false, "Role not valid", null);
        }

        // Como esta funcion es para setear los menus por defecto del usuario según su jerarquía,
        // primero eliminamos todos los menus asignados al usuario
        bool deleteMenusResult = await _menuToUserRepository.DeleteAllMenusFromUserAsync(userId);
        if (!deleteMenusResult)
        {
            return new ApiResponse<MenuToUserDTO>(false, "Failed to delete menus", null);
        }

        // Ahora obtenemos todos los menus a los que el rol del usuario tiene acceso
        ApiResponse<IEnumerable<MenuDTO>> accessibleMenus = await _menuService.GetMenusByHierarchyAsync(roleUser.Hierarchy);
        if (!accessibleMenus.Success)
        {
            return new ApiResponse<MenuToUserDTO>(false, accessibleMenus.Error, null);
        }


        // Flatten la lista de menús y submenús para asignarlos al usuario
        List<Menu> menusToAssign = new List<Menu>();

        foreach (var menu in accessibleMenus.Data!)
        {
            Menu menuToAssign = new Menu
            {
                Id = menu.IdMenu,
                Name = menu.Name,
                Url = menu.Url,
                IsMain = menu.IsMain,
                ParentMenuId = menu.ParentId,
                MinimumHierarchy = menu.MinHierarchy
            };

            menusToAssign.Add(menuToAssign);

            if (menu.Children != null && menu.Children.Any())
            {
                foreach (var subMenu in menu.Children)
                {
                    Menu subMenuToAssign = new Menu
                    {
                        Id = subMenu.IdMenu,
                        Name = subMenu.Name,
                        Url = subMenu.Url,
                        IsMain = subMenu.IsMain,
                        ParentMenuId = subMenu.ParentId,
                        MinimumHierarchy = subMenu.MinHierarchy
                    };

                    menusToAssign.Add(subMenuToAssign);
                }
            }
        }

        IEnumerable<MenuToUser> assignedMenus = await _menuToUserRepository.SetDefaultMenusByHierarchyAsync(userId, menusToAssign);

        List<MenuDTO> menuDTOs = new List<MenuDTO>();
        foreach (var menu in assignedMenus)
        {
            menuDTOs.Add(new MenuDTO
            (
                IdMenu: menu.Menu.Id,
                Name: menu.Menu.Name,
                Url: menu.Menu.Url,
                IsMain: menu.Menu.IsMain,
                ParentId: menu.Menu.ParentMenuId,
                MinHierarchy: menu.Menu.MinimumHierarchy,
                Children: Array.Empty<MenuDTO>()
            ));
        }

        MenuToUserDTO groupedMenus = new MenuToUserDTO(
                UserId: userId,
                Menu: menuDTOs.ToArray()
        );

        return new ApiResponse<MenuToUserDTO>(true, "Default menus set successfully", groupedMenus);
    }


    private List<MenuDTO> GetGroupedMenus(List<MenuDTO> menusToGroup)
    {
        List<MenuDTO> parentMenus = new List<MenuDTO>();
        List<MenuDTO> childMenus = new List<MenuDTO>();
        List<MenuDTO> groupedMenus = new List<MenuDTO>();


        foreach (var menu in menusToGroup)
        {
            if (menu.ParentId == null)
            {
                parentMenus.Add(menu);
            }
            else
            {
                childMenus.Add(menu);
            }
        }
        // Ahora agrupamos los menús hijos dentro de su menú padre correspondiente
        var childrenGrouped = childMenus.GroupBy(c => c.ParentId);


        foreach (var parentMenu in parentMenus)
        {
            var children = childrenGrouped.FirstOrDefault(g => g.Key == parentMenu.IdMenu)?.ToList() ?? new List<MenuDTO>();

            MenuDTO groupedMenu = new MenuDTO
            (
                IdMenu: parentMenu.IdMenu,
                Name: parentMenu.Name,
                Url: parentMenu.Url,
                IsMain: parentMenu.IsMain,
                ParentId: parentMenu.ParentId,
                MinHierarchy: parentMenu.MinHierarchy,
                Children: children.ToArray()
            );


            groupedMenus.Add(groupedMenu);
        }

        return groupedMenus;
    }
}
