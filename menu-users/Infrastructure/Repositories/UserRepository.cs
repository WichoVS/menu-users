using System;
using menu_users.Domain.Entities;
using menu_users.Domain.Interfaces.Repositories;

namespace menu_users.Infrastructure.Repositories;

public class UserRepository : IRepository<User>
{
    public Task AddAsync(User entity)
    {
        throw new NotImplementedException();
    }

    public void Delete(User entity)
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<User>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<User?> GetByIdAsync(object id)
    {
        throw new NotImplementedException();
    }

    public void Update(User entity)
    {
        throw new NotImplementedException();
    }
}
