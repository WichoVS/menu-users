using System;
using menu_users.Domain.Entities;
using menu_users.Domain.Interfaces.Repositories;
using menu_users.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace menu_users.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }


    public async Task<User> AddAsync(User entity)
    {
        await _context.Users.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> DeleteAsync(User entity)
    {
        // Solo pasa a poner en activo = false, no eliminamos completamente
        User? usr = await _context.Users.FirstOrDefaultAsync(u => u.Id == entity.Id);
        if (usr == null)
        {
            return false;
        }

        usr.IsActive = false;
        _context.Users.Update(usr);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _context.Users.Include(u => u.Role).Where(u => u.IsActive).ToListAsync();
    }

    public async Task<User?> GetByIdAsync(object id)
    {
        return await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Id.ToString() == (string)id);
    }

    public async Task<User?> UpdateAsync(User entity)
    {
        User? usr = await _context.Users.FirstOrDefaultAsync(u => u.Id == entity.Id);
        if (usr == null)
        {
            return null;
        }

        usr.FirstName = entity.FirstName;
        usr.LastName = entity.LastName;
        usr.Email = entity.Email;
        usr.RoleId = entity.RoleId;
        usr.Role = entity.Role;

        _context.Users.Update(usr);
        await _context.SaveChangesAsync();
        return usr;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Email == email);
    }

}
