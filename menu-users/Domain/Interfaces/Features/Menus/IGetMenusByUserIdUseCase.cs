using System;
using menu_users.Application.DTOs.ApiResponse;
using menu_users.Application.DTOs.MenuToUser;

namespace menu_users.Domain.Interfaces.Features.Menus;

public interface IGetMenusByUserIdUseCase
{
    Task<ApiResponse<MenuToUserDTO>> Execute(string userId);
}
