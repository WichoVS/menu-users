using System;

namespace menu_users.Domain.Entities;

public class Menu
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public bool IsMain { get; set; }

    public int? ParentMenuId { get; set; }

    public Menu? ParentMenu { get; set; }

    public ICollection<Menu> SubMenus { get; set; } = new List<Menu>();

    public string Url { get; set; } = null!;

    public int MinimumHierarchy { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
    // Es la lista de los usuarios que tienen acceso a este menú
    public ICollection<MenuToUser> MenuUsers { get; set; } = new List<MenuToUser>();
}
