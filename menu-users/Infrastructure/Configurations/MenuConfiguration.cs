using System;
using menu_users.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace menu_users.Infrastructure.Configurations;

public class MenuConfiguration : IEntityTypeConfiguration<Menu>
{
    public void Configure(EntityTypeBuilder<Menu> builder)
    {
        builder.ToTable("Menus");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(m => m.IsMain)
            .IsRequired();

        builder.Property(m => m.Url)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(m => m.MinimumHierarchy)
            .IsRequired();

        builder.Property(m => m.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(m => m.CreatedAt)
            .IsRequired();

        builder.Property(m => m.UpdatedAt)
            .IsRequired();

        builder.HasOne(m => m.ParentMenu)
            .WithMany(m => m.SubMenus)
            .HasForeignKey(m => m.ParentMenuId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
