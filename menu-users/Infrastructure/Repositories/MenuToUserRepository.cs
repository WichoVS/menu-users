using System;
using menu_users.Domain.Entities;
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

    public async Task<IEnumerable<MenuToUser>> GetMenusByUserIdAsync(Guid userId)
    {
        return await _context.MenuToUsers.Where(mtU => mtU.UserId == userId).Include(mtU => mtU.Menu).ToListAsync();
    }

    public async Task<bool> DeleteAllMenusFromUserAsync(Guid userId)
    {
        var menusToDelete = _context.MenuToUsers.Where(mtU => mtU.UserId == userId);
        _context.MenuToUsers.RemoveRange(menusToDelete);
        await _context.SaveChangesAsync();
        return true;
    }

    // Implementar este método para asignar los menús por defecto a un usuario según su jerarquía
    public async Task<IEnumerable<MenuToUser>> SetDefaultMenusByHierarchyAsync(Guid userId, IEnumerable<Menu> menus)
    {
        List<MenuToUser> menuToUsers = new List<MenuToUser>();

        foreach (var menu in menus)
        {
            MenuToUser menuToUser = new MenuToUser
            {
                UserId = userId,
                MenuId = menu.Id,
                CreatedAt = DateTime.UtcNow
            };
            menuToUsers.Add(menuToUser);
        }

        await _context.MenuToUsers.AddRangeAsync(menuToUsers);
        await _context.SaveChangesAsync();

        return menuToUsers.AsEnumerable();
    }

    // Este método no es necesario implementarlo, ya que no se actualizan solo se asignan o eliminan los menús a los usuarios.
    public Task<MenuToUser?> UpdateAsync(MenuToUser entity)
    {
        throw new NotImplementedException();
    }
}
