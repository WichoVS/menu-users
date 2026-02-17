using System;
using menu_users.Application.DTOs.ApiResponse;
using menu_users.Application.DTOs.Menu;
using menu_users.Domain.Entities;
using menu_users.Domain.Interfaces.Features.Menus;
using menu_users.Domain.Interfaces.Repositories;

namespace menu_users.Application.Features.Menus.GetMenusByUserIdUseCase;

public class GetMenusByUserIdUseCase : IGetMenusByUserIdUseCase
{
    private readonly IMenuRepository _menuRepository;
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;

    public GetMenusByUserIdUseCase(IMenuRepository menuRepository, IUserRepository userRepository, IRoleRepository roleRepository)
    {
        _menuRepository = menuRepository;
        _userRepository = userRepository;
        _roleRepository = roleRepository;
    }

    public async Task<ApiResponse<IEnumerable<MenuDTO>>> Execute(string userId)
    {
        User? user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            return new ApiResponse<IEnumerable<MenuDTO>>(false, "User not found.", null);
        }

        Role? role = await _roleRepository.GetByIdAsync(user.RoleId);
        if (role == null)
        {
            return new ApiResponse<IEnumerable<MenuDTO>>(false, "Role not found.", null);
        }

        IEnumerable<Menu> menus = await _menuRepository.GetMenusByUserHierarchyAsync(role.Hierarchy);

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
}
