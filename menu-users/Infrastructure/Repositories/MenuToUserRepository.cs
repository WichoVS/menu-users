using System;
using menu_users.Domain.Interfaces.Repositories;
using menu_users.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace menu_users.Infrastructure.Repositories;

public class MenuToUserRepository : IMenuToUserRepository
{
    private readonly AppDbContext _context;

    public MenuToUserRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<MenuToUser> AddAsync(MenuToUser entity)
    {
        await _context.MenuToUsers.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> DeleteAsync(MenuToUser entity)
    {
        MenuToUser? menuToUser = await _context.MenuToUsers.FindAsync(entity.Id);
        if (menuToUser == null)
        {
            return false;
        }

        _context.MenuToUsers.Remove(menuToUser);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<MenuToUser>> GetAllAsync()
    {
        return await _context.MenuToUsers.Include(mtU => mtU.Menu).Include(mtU => mtU.User).ToListAsync();
    }

    public async Task<MenuToUser?> GetByIdAsync(object id)
    {
        MenuToUser? menuToUser = await _context.MenuToUsers.Include(mtU => mtU.Menu).Include(mtU => mtU.User).FirstOrDefaultAsync(mtU => mtU.Id == (int)id);
        return menuToUser;
    }

    // Implementar este método para asignar los menús por defecto a un usuario según su jerarquía
    public Task<IEnumerable<MenuToUser>> SetDefaultMenusByHierarchy(Guid userId)
    {

    }

    // Este método no es necesario implementarlo, ya que no se actualizan solo se asignan o eliminan los menús a los usuarios.
    public Task<MenuToUser?> UpdateAsync(MenuToUser entity)
    {
        throw new NotImplementedException();
    }
}
