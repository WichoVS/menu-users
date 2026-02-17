using System;
using menu_users.Application.DTOs.ApiResponse;
using menu_users.Domain.Interfaces.Features.Menus;
using menu_users.Domain.Interfaces.Repositories;

namespace menu_users.Application.Features.Menus.RemoveMenuToUserUseCase;

public class RemoveMenuToUserUseCase : IRemoveMenuToUserUseCase
{
    private readonly IMenuRepository _menuRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMenuToUserRepository _menuToUserRepository;

    public RemoveMenuToUserUseCase(IMenuRepository menuRepository, IUserRepository userRepository, IMenuToUserRepository menuToUserRepository)
    {
        _menuRepository = menuRepository;
        _userRepository = userRepository;
        _menuToUserRepository = menuToUserRepository;
    }

    public async Task<ApiResponse<bool>> Execute(Guid userId, int menuId)
    {

        // Verificamos si el usuario existe
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            return new ApiResponse<bool>(false, "User not found", false);
        }

        // Verificamos si el menú existe
        var menu = await _menuRepository.GetByIdAsync(menuId);
        if (menu == null)
        {
            return new ApiResponse<bool>(false, "Menu not found", false);
        }

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
}
