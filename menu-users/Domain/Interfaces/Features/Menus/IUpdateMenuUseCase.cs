using System;
using menu_users.Application.DTOs.ApiResponse;
using menu_users.Application.DTOs.Menu;
using menu_users.Application.Features.Menus.UpdateMenuUseCase;

namespace menu_users.Domain.Interfaces.Features.Menus;

public interface IUpdateMenuUseCase
{
    Task<ApiResponse<MenuDTO>> Execute(int id, UpdateMenuRequest request);
}
