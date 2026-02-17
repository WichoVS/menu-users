using System;
using menu_users.Application.DTOs.ApiResponse;
using menu_users.Application.DTOs.Menu;
using menu_users.Application.DTOs.MenuToUser;
using menu_users.Domain.Entities;
using menu_users.Domain.Interfaces.Features.Menus;
using menu_users.Domain.Interfaces.Repositories;

namespace menu_users.Application.Features.Menus.AssignMenuToUserUseCase;

public class AssignMenuToUserUseCase : IAssignMenuToUserUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IMenuRepository _menuRepository;

    private readonly IMenuToUserRepository _menuToUserRepository;

    public AssignMenuToUserUseCase(IUserRepository userRepository,
    IMenuRepository menuRepository, IMenuToUserRepository menuToUserRepository)
    {
        _userRepository = userRepository;
        _menuRepository = menuRepository;
        _menuToUserRepository = menuToUserRepository;
    }

    public async Task<ApiResponse<MenuToUserDTO>> Execute(Guid userId, int menu)
    {
        User? user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            return new ApiResponse<MenuToUserDTO>(false, "User not found", null);
        }

        Menu? menuEntity = await _menuRepository.GetByIdAsync(menu);
        if (menuEntity == null)
        {
            return new ApiResponse<MenuToUserDTO>(false, "Menu not found", null);
        }

        // Verificamos si el menú ya está asignado al usuario
        IEnumerable<MenuToUser> existingMenus = await _menuToUserRepository.GetMenusByUserIdAsync(userId);
        if (existingMenus.Any(m => m.MenuId == menuEntity.Id))
        {
            return new ApiResponse<MenuToUserDTO>(true, "Menu already assigned to user", null);
        }

        // En caso de que el menú no esté asignado, procedemos a asignarlo al usuario
        // Primero verificando si es un menú hijo, en ese caso se asigna el menú padre también
        if (menuEntity.ParentMenuId != null)
        {
            int parentMenuId = menuEntity.ParentMenuId.Value;

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
            MenuId = menuEntity.Id,
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
                    IdMenu: addedMenuToUser.Menu.Id,
                    Name: addedMenuToUser.Menu.Name,
                    Url: addedMenuToUser.Menu.Url,
                    IsMain: addedMenuToUser.Menu.IsMain,
                    ParentId: addedMenuToUser.Menu.ParentMenuId,
                    MinHierarchy: addedMenuToUser.Menu.MinimumHierarchy,
                    Children: Array.Empty<MenuDTO>()
                )
            ]
        );

        return new ApiResponse<MenuToUserDTO>(true, "Menu assigned to user successfully", menuToUserDTO);
    }
}
