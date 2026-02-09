using System;
using menu_users.Domain.Entities;
using menu_users.Domain.Interfaces.Repositories;
using menu_users.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace menu_users.Infrastructure.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly AppDbContext _context;

    public RoleRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Role> AddAsync(Role entity)
    {
        await _context.Roles.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> DeleteAsync(Role entity)
    {
        Role? role = await _context.Roles.FindAsync(entity.Id);
        if (role == null)
        {
            return false;
        }

        //Será necesario actualizar el rol de los usuarios a un Rol por defecto

        role.IsActive = false;
        _context.Roles.Update(role);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<Role>> GetAllAsync()
    {
        return await _context.Roles.Where(r => r.IsActive).ToListAsync();
    }

    public async Task<Role?> GetByIdAsync(object id)
    {
        return await _context.Roles.FindAsync(id);
    }

    public async Task<Role?> GetByNameAsync(string name)
    {
        return await _context.Roles.FirstOrDefaultAsync(r => r.Name == name);
    }

    public async Task<Role?> GetLowestByHierarchyAsync()
    {
        return await _context.Roles
            .Where(r => r.IsActive == true)
            .OrderBy(r => r.Hierarchy)
            .FirstOrDefaultAsync();
    }

    public async Task<Role?> UpdateAsync(Role entity)
    {
        Role? role = await _context.Roles.FindAsync(entity.Id);
        if (role == null)
        {
            return null;
        }

        role.Name = entity.Name;
        role.Hierarchy = entity.Hierarchy;
        _context.Roles.Update(role);
        await _context.SaveChangesAsync();
        return role;
    }
}
