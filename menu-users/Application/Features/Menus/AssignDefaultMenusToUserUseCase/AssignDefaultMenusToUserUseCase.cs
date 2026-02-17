using System;
using menu_users.Application.DTOs.ApiResponse;
using menu_users.Application.DTOs.Menu;
using menu_users.Application.DTOs.MenuToUser;
using menu_users.Domain.Entities;
using menu_users.Domain.Interfaces.Features.Menus;
using menu_users.Domain.Interfaces.Repositories;
using menu_users.Domain.Interfaces.Services;

namespace menu_users.Application.Features.Menus.AssignDefaultMenusToUserUseCase;

public class AssignDefaultMenusToUserUseCase : IAssignDefaultMenusToUserUseCase
{
    private readonly IMenuRepository _menuRepository;
    private readonly IMenuToUserRepository _menuToUserRepository;
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IAccessAssignmentService _accessAssignmentService;
    public AssignDefaultMenusToUserUseCase(
        IMenuRepository menuRepository,
        IMenuToUserRepository menuToUserRepository,
        IUserRepository userRepository,
        IRoleRepository roleRepository,
        IAccessAssignmentService accessAssignmentService)
    {
        _menuRepository = menuRepository;
        _menuToUserRepository = menuToUserRepository;
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _accessAssignmentService = accessAssignmentService;
    }
    public async Task<ApiResponse<MenuToUserDTO>> Execute(Guid userId)
    {
        // Verificamos que el user existe
        User? user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            return new ApiResponse<MenuToUserDTO>(false, "User not found", null);
        }

        // Obtenemos todos los roles para verificar el rol del usuario
        IEnumerable<Role> roleRes = await _roleRepository.GetAllAsync();
        if (!roleRes.Any())
        {
            return new ApiResponse<MenuToUserDTO>(false, "No roles available", null);
        }

        // Verificamos el Role del Usuario
        Role? roleUser = roleRes.FirstOrDefault(r => r.Id == user.RoleId);
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
        IEnumerable<Menu> accessibleMenus = await _menuRepository.GetMenusByUserHierarchyAsync(roleUser.Hierarchy);
        if (!accessibleMenus.Any())
        {
            return new ApiResponse<MenuToUserDTO>(false, "No accessible menus for the user's hierarchy", null);
        }


        // Flatten la lista de menús y submenús para asignarlos al usuario
        List<MenuDTO> menusToAssign = new List<MenuDTO>();

        foreach (var menu in accessibleMenus)
        {
            MenuDTO menuToAssign = new MenuDTO
            (
                IdMenu: menu.Id,
                Name: menu.Name,
                Url: menu.Url,
                IsMain: menu.IsMain,
                ParentId: menu.ParentMenuId,
                MinHierarchy: menu.MinimumHierarchy,
                Children: Array.Empty<MenuDTO>()
            );

            menusToAssign.Add(menuToAssign);

            if (menu.SubMenus != null && menu.SubMenus.Any())
            {
                foreach (var subMenu in menu.SubMenus)
                {
                    MenuDTO subMenuToAssign = new MenuDTO
                    (
                        IdMenu: subMenu.Id,
                        Name: subMenu.Name,
                        Url: subMenu.Url,
                        IsMain: subMenu.IsMain,
                        ParentId: subMenu.ParentMenuId,
                        MinHierarchy: subMenu.MinimumHierarchy,
                        Children: Array.Empty<MenuDTO>()
                    );

                    menusToAssign.Add(subMenuToAssign);
                }
            }
        }

        MenuToUserDTO assignedMenus = await _accessAssignmentService.AssignDefaultMenusToUserAsync(userId.ToString(), menusToAssign);

        return new ApiResponse<MenuToUserDTO>(true, "Default menus set successfully", assignedMenus);
    }
}
