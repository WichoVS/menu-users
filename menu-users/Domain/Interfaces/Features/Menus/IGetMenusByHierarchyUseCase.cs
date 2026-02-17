using System;
using menu_users.Application.DTOs.ApiResponse;
using menu_users.Application.DTOs.Menu;

namespace menu_users.Domain.Interfaces.Features.Menus;

public interface IGetMenusByHierarchyUseCase
{
    Task<ApiResponse<IEnumerable<MenuDTO>>> Execute(int hierarchy);
}
