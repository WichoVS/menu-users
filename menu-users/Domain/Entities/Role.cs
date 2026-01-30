using System;

namespace menu_users.Domain.Entities;

public class Role
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int Hierarchy { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    // Navegación
    public ICollection<User> Users { get; set; } = new List<User>();
}
