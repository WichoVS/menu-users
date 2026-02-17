using System;
using menu_users.Application.DTOs.ApiResponse;
using menu_users.Application.DTOs.Menu;

namespace menu_users.Domain.Interfaces.Features.Menus;

public interface IGetMenuByIdUseCase
{
    Task<ApiResponse<MenuDTO>> Execute(int id);
}
