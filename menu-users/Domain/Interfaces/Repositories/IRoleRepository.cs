using System;
using menu_users.Domain.Entities;

namespace menu_users.Domain.Interfaces.Repositories;

public interface IRoleRepository : IRepository<Role>
{
    Task<Role?> GetByNameAsync(string name);
    Task<Role?> GetLowestByHierarchyAsync();
}
