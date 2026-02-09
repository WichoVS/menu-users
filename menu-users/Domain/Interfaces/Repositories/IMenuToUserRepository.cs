using System;
using menu_users.Application.DTOs.ApiResponse;
using menu_users.Domain.Entities;

namespace menu_users.Domain.Interfaces.Repositories;

public interface IMenuToUserRepository : IRepository<MenuToUser>
{
    Task<IEnumerable<MenuToUser>> SetDefaultMenusByHierarchyAsync(Guid userId, IEnumerable<Menu> menus);
    Task<IEnumerable<MenuToUser>> GetMenusByUserIdAsync(Guid userId);
}
