using System;
using menu_users.Domain.Entities;

namespace menu_users.Domain.Interfaces.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
}
