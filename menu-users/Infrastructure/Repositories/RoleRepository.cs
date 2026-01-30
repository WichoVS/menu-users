using System;
using menu_users.Domain.Entities;
using menu_users.Domain.Interfaces.Repositories;

namespace menu_users.Infrastructure.Repositories;

public class RoleRepository : IRepository<Role>
{
    public Task AddAsync(Role entity)
    {
        throw new NotImplementedException();
    }

    public void Delete(Role entity)
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Role>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Role?> GetByIdAsync(object id)
    {
        throw new NotImplementedException();
    }

    public void Update(Role entity)
    {
        throw new NotImplementedException();
    }
}
