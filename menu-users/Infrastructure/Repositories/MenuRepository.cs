using System;
using menu_users.Domain.Entities;
using menu_users.Domain.Interfaces.Repositories;
using menu_users.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace menu_users.Infrastructure.Repositories;

public class MenuRepository : IMenuRepository
{
    private readonly AppDbContext _context;

    public MenuRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Menu> AddAsync(Menu entity)
    {
        await _context.Menus.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> DeleteAsync(Menu entity)
    {
        Menu? menu = await _context.Menus.FindAsync(entity.Id);
        if (menu == null)
        {
            return false;
        }

        // Solo pasa a poner en activo = false, no eliminamos completamente
        menu.IsActive = false;
        _context.Menus.Update(menu);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<Menu>> GetAllAsync()
    {
        return await _context.Menus.Include(ch => ch.SubMenus).Where(m => m.IsActive).ToListAsync();
    }

    public Task<Menu?> GetByIdAsync(object id)
    {
        return _context.Menus.Include(ch => ch.SubMenus).FirstOrDefaultAsync(m => m.Id == (int)id);
    }

    public async Task<IEnumerable<Menu>> GetMenusByUserHierarchyAsync(int hierarchy)
    {
        return await _context.Menus
            .Where(m => m.MinimumHierarchy <= hierarchy && m.IsActive && m.ParentMenuId == null)
            .Include(ch => ch.SubMenus)
            .ToListAsync();
    }

    public async Task<Menu?> UpdateAsync(Menu entity)
    {
        Menu? menuExists = await _context.Menus.FirstOrDefaultAsync(m => m.Id == entity.Id);
        if (menuExists == null)
        {
            return null;
        }

        menuExists.Name = entity.Name;
        menuExists.Url = entity.Url;
        menuExists.IsMain = entity.IsMain;
        menuExists.ParentMenuId = entity.ParentMenuId;
        menuExists.MinimumHierarchy = entity.MinimumHierarchy;
        _context.Menus.Update(menuExists);
        await _context.SaveChangesAsync();
        return menuExists;
    }
}
