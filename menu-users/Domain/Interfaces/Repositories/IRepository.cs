using System;

namespace menu_users.Domain.Interfaces.Repositories;

public interface IRepository<T>
{
    Task<T?> GetByIdAsync(object id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> AddAsync(T entity);
    Task<T?> UpdateAsync(T entity);
    Task<bool> DeleteAsync(T entity);
}
