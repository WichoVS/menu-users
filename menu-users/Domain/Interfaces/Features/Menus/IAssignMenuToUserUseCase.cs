using System;
using menu_users.Application.DTOs.ApiResponse;
using menu_users.Application.DTOs.Menu;
using menu_users.Application.DTOs.MenuToUser;

namespace menu_users.Domain.Interfaces.Features.Menus;

public interface IAssignMenuToUserUseCase
{
    Task<ApiResponse<MenuToUserDTO>> Execute(Guid userId, int menu);
}
