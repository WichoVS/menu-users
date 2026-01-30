using System;
using menu_users.Domain.Entities;
using menu_users.Domain.Interfaces.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace menu_users.Infrastructure.Data;

public class AppDbContext : DbContext
{
    private IServiceProvider _serviceProvider;

    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Menu> Menus => Set<Menu>();


    public AppDbContext(DbContextOptions<AppDbContext> options, IServiceProvider serviceProvider) : base(options)
    {
        _serviceProvider = serviceProvider;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        optionsBuilder.UseSeeding((context, _) =>
        {
            var hasher = context.GetService<IPasswordHasher>();
            SeedAsync(context, hasher, CancellationToken.None).GetAwaiter().GetResult();
        });


        optionsBuilder.UseAsyncSeeding(async (context, _, cancellationToken) =>
        {
            var passwordHasher = context.GetService<IPasswordHasher>();
            await SeedAsync(context, passwordHasher, cancellationToken);
        });
    }

    private static async Task SeedAsync(
        DbContext context,
        IPasswordHasher passwordHasher,
        CancellationToken cancellationToken)
    {
        // 1- Crear el Rol de SuperUser por defecto si no existe.
        Role? superUserRole = context.Set<Role>().FirstOrDefault(r => r.Name == "SUPERUSER");
        if (superUserRole == null)
        {
            superUserRole = new Role
            {
                Name = "SUPERUSER",
                // Usaremos 100 como el tope máximo de jerarquía.
                Hierarchy = 100,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true
            };
            context.Set<Role>().Add(superUserRole);
            await context.SaveChangesAsync(cancellationToken);
        }

        Role? adminRole = context.Set<Role>().FirstOrDefault(r => r.Name == "Administrator");
        if (adminRole == null)
        {
            adminRole = new Role
            {
                Name = "Administrator",
                Hierarchy = 80,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true
            };
            context.Set<Role>().Add(adminRole);
            await context.SaveChangesAsync(cancellationToken);
        }

        Role? usuarioRole = context.Set<Role>().FirstOrDefault(r => r.Name == "Usuario");
        if (usuarioRole == null)
        {
            usuarioRole = new Role
            {
                Name = "Usuario",
                Hierarchy = 2,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true
            };
            context.Set<Role>().Add(usuarioRole);
            await context.SaveChangesAsync(cancellationToken);
        }

        // 2- Crear el Menú Administrador y Menú de Inicio por defecto si no existe.
        Menu? adminMenu = context.Set<Menu>().FirstOrDefault(m => m.Name == "Administración");
        if (adminMenu == null)
        {
            adminMenu = new Menu
            {
                Name = "Administración",
                IsMain = true,
                Url = "/administracion",
                MinimumHierarchy = adminRole.Hierarchy,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true
            };
            context.Set<Menu>().Add(adminMenu);
            await context.SaveChangesAsync(cancellationToken);
        }

        Menu? adminHomeMenu = context.Set<Menu>().FirstOrDefault(m => m.Name == "Inicio");
        if (adminHomeMenu == null)
        {
            adminHomeMenu = new Menu
            {
                Name = "Inicio",
                IsMain = true,
                Url = "/inicio",
                MinimumHierarchy = usuarioRole.Hierarchy,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true
            };
            context.Set<Menu>().Add(adminHomeMenu);
            await context.SaveChangesAsync(cancellationToken);
        }

        Menu? usuarioMenu = context.Set<Menu>().FirstOrDefault(m => m.Name == "Usuario");
        if (usuarioMenu == null)
        {
            usuarioMenu = new Menu
            {
                Name = "Usuario",
                IsMain = true,
                Url = "/usuario",
                MinimumHierarchy = usuarioRole.Hierarchy,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true
            };
            context.Set<Menu>().Add(usuarioMenu);
            await context.SaveChangesAsync(cancellationToken);
        }

        Menu? perfilMenu = context.Set<Menu>().FirstOrDefault(m => m.Name == "Perfil");
        if (perfilMenu == null)
        {
            perfilMenu = new Menu
            {
                Name = "Perfil",
                IsMain = false,
                ParentMenuId = usuarioMenu.Id,
                Url = "/usuario/perfil",
                MinimumHierarchy = usuarioRole.Hierarchy,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true
            };
            context.Set<Menu>().Add(perfilMenu);
            await context.SaveChangesAsync(cancellationToken);
        }

        Menu? cerrarSesionMenu = context.Set<Menu>().FirstOrDefault(m => m.Name == "Cerrar Sesión");
        if (cerrarSesionMenu == null)
        {
            cerrarSesionMenu = new Menu
            {
                Name = "Cerrar Sesión",
                IsMain = false,
                ParentMenuId = usuarioMenu.Id,
                Url = "/usuario/cerrar-sesion",
                MinimumHierarchy = usuarioRole.Hierarchy,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true
            };
            context.Set<Menu>().Add(cerrarSesionMenu);
            await context.SaveChangesAsync(cancellationToken);
        }

        // 3- Crear el Usuario Administrador por defecto si no existe.
        User? adminUser = context.Set<User>().FirstOrDefault(u => u.Email == "administrador@menuusers.com");
        if (adminUser == null)
        {
            adminUser = new User
            {
                FirstName = "Admin",
                LastName = "User",
                Email = "administrador@menuusers.com",
                Password = passwordHasher.Hash("Admin@123"),
                RoleId = adminRole.Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true
            };
            context.Set<User>().Add(adminUser);
            await context.SaveChangesAsync(cancellationToken);
        }
    }

}
