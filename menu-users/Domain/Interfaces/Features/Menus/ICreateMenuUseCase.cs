using System;
using menu_users.Application.DTOs.ApiResponse;
using menu_users.Application.DTOs.Menu;
using menu_users.Application.Features.Menus.CreateMenuUseCase;

namespace menu_users.Domain.Interfaces.Features.Menus;

public interface ICreateMenuUseCase
{
    Task<ApiResponse<MenuDTO>> Execute(CreateMenuRequest request);
}
