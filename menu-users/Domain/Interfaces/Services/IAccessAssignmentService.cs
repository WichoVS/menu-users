using System;
using menu_users.Application.DTOs.Menu;
using menu_users.Application.DTOs.MenuToUser;

namespace menu_users.Domain.Interfaces.Services;

public interface IAccessAssignmentService
{
    Task<MenuToUserDTO> AssignDefaultMenusToUserAsync(string userId, List<MenuDTO> defaultMenus);
}
