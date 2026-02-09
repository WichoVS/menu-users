using System;

namespace menu_users.Domain.Interfaces.Repositories;

public interface IMenuToUserRepository : IRepository<MenuToUser>
{
    Task<IEnumerable<MenuToUser>> SetDefaultMenusByHierarchy(Guid userId);
}
