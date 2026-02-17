using System;
using menu_users.Application.DTOs.ApiResponse;
using menu_users.Application.DTOs.Menu;
using menu_users.Domain.Entities;
using menu_users.Domain.Interfaces.Features.Menus;
using menu_users.Domain.Interfaces.Repositories;

namespace menu_users.Application.Features.Menus.CreateMenuUseCase;

public class CreateMenuUseCase : ICreateMenuUseCase
{
    private readonly IMenuRepository _menuRepository;

    public CreateMenuUseCase(IMenuRepository menuRepository)
    {
        _menuRepository = menuRepository;
    }

    public async Task<ApiResponse<MenuDTO>> Execute(CreateMenuRequest request)
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
}
