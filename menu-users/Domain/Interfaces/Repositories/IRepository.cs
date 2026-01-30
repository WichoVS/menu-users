using System;

namespace menu_users.Domain.Interfaces.Repositories;

public interface IRepository<T> : IDisposable
{
    Task<T?> GetByIdAsync(object id);
    Task<IEnumerable<T>> GetAllAsync();
    Task AddAsync(T entity);
    void Update(T entity);
    void Delete(T entity);
}
