using System;
using menu_users.Domain.Entities;
using menu_users.Domain.Interfaces.Repositories;

namespace menu_users.Infrastructure.Repositories;

public class MenuRepository : IMenuRepository
{
    public Task<Menu> AddAsync(Menu entity)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(Menu entity)
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

    public Task<Menu?> UpdateAsync(Menu entity)
    {
        throw new NotImplementedException();
    }
}
