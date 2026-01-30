using System;
using menu_users.Domain.Entities;
using menu_users.Domain.Interfaces.Repositories;

namespace menu_users.Infrastructure.Repositories;

public class MenuRepository : IRepository<Menu>
{
    public Task AddAsync(Menu entity)
    {
        throw new NotImplementedException();
    }

    public void Delete(Menu entity)
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Menu>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Menu?> GetByIdAsync(object id)
    {
        throw new NotImplementedException();
    }

    public void Update(Menu entity)
    {
        throw new NotImplementedException();
    }
}
