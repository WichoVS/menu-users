using System;
using menu_users.Application.DTOs.ApiResponse;
using menu_users.Application.DTOs.Menu;

namespace menu_users.Domain.Interfaces.Features.Menus;

public interface IGetAllMenusUseCase
{
    Task<ApiResponse<IEnumerable<MenuDTO>>> Execute();
}
