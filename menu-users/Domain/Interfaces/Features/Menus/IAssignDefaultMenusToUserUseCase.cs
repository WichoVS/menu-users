using System;
using menu_users.Application.DTOs.ApiResponse;
using menu_users.Application.DTOs.MenuToUser;

namespace menu_users.Domain.Interfaces.Features.Menus;

public interface IAssignDefaultMenusToUserUseCase
{
    Task<ApiResponse<MenuToUserDTO>> Execute(Guid userId);
}
