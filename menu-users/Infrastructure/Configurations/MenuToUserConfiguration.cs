using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace menu_users.Infrastructure.Configurations;

public class MenuToUserConfiguration : IEntityTypeConfiguration<MenuToUser>
{
    public void Configure(EntityTypeBuilder<MenuToUser> builder)
    {
        builder.ToTable("MenuToUsers");

        builder.HasKey(mu => mu.Id);

        builder.Property(mu => mu.CreatedAt)
            .IsRequired();

        builder.HasOne(mu => mu.Menu)
            .WithMany(m => m.MenuUsers)
            .HasForeignKey(mu => mu.MenuId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(mu => mu.User)
            .WithMany(u => u.UserMenus)
            .HasForeignKey(mu => mu.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }

}
