using System;
using menu_users.Application.DTOs.Menu;
using menu_users.Application.DTOs.MenuToUser;
using menu_users.Domain.Interfaces.Repositories;
using menu_users.Domain.Interfaces.Services;

namespace menu_users.Application.Services;

public class AccessAssignmentService : IAccessAssignmentService
{
    private readonly IMenuToUserRepository _menuToUserRepository;
    public AccessAssignmentService(IMenuToUserRepository menuToUserRepository)
    {
        _menuToUserRepository = menuToUserRepository;
    }

    // En este metodo recibimos el userId y una lista de los menus para agregar, la lista que recibimos
    // se encuentra flatten, es decir, no tiene la estructura de menu padre con hijos.
    // Se retorna un MenuToUserDTO con la estructura de menu padre con hijos.
    public async Task<MenuToUserDTO> AssignDefaultMenusToUserAsync(string userId, List<MenuDTO> defaultMenus)
    {
        MenuToUserDTO assignedMenus = new MenuToUserDTO(
            UserId: Guid.Parse(userId),
            Menu: defaultMenus.ToArray()
        );

        List<MenuToUser> menuToUserList = new List<MenuToUser>();
        foreach (var menu in defaultMenus)
        {
            var menuToUser = await _menuToUserRepository.AddAsync(
                new MenuToUser
                {
                    MenuId = menu.IdMenu,
                    UserId = Guid.Parse(userId),
                    CreatedAt = DateTime.UtcNow,
                });
            menuToUserList.Add(menuToUser);
        }

        List<MenuDTO> menuDTOs = new List<MenuDTO>();
        foreach (var menu in menuToUserList)
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

        assignedMenus = new MenuToUserDTO(
            UserId: Guid.Parse(userId),
            Menu: GetGroupedMenus(menuDTOs.ToList()).ToArray()
        );

        return assignedMenus;
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
