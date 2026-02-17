using System;
using menu_users.Application.DTOs.ApiResponse;
using menu_users.Domain.Entities;
using menu_users.Domain.Interfaces.Features.Menus;
using menu_users.Domain.Interfaces.Repositories;
using menu_users.Domain.Interfaces.Services;

namespace menu_users.Application.Features.Menus.DeleteMenuUseCase;

public class DeleteMenuUseCase : IDeleteMenuUseCase
{
    private readonly IMenuRepository _menuRepository;

    public DeleteMenuUseCase(IMenuRepository menuRepository)
    {
        _menuRepository = menuRepository;
    }
    public async Task<ApiResponse<bool>> Execute(int id)
    {
        Menu? menu = await _menuRepository.GetByIdAsync(id);

        if (menu == null)
        {
            return new ApiResponse<bool>(false, "Menu not found.", false);
        }

        bool result = await _menuRepository.DeleteAsync(menu);
        return new ApiResponse<bool>(result, result ? null : "Failed to delete menu.", result);
    }
}
