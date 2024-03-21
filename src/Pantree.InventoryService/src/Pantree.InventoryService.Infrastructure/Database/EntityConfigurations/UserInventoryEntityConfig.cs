using Microsoft.EntityFrameworkCore;
using Pantree.InventoryService.Domain.Entities;

namespace Pantree.InventoryService.Infrastructure.Database.EntityConfigurations;

/// <summary>
/// Extension method for building the table structure for our user inventory domain model
/// </summary>
public static class UserInventoryEntityConfig {

    public static void RegisterUserInventoryEntity(this ModelBuilder builder) {
        builder.Entity<UserInventory>(cfg => {
            // configure the base table properties
            cfg.ToTable("user_inventory");
            cfg.HasKey(pk => pk.Id);
            
            // configure the unique index so that the user can only
            // hold a single record for any given product sku!
            cfg.HasIndex(i => new {
                i.UserId,
                i.ProductSku
            }).IsUnique();

            // configure the columns
            cfg.Property(p => p.Id)
                .HasColumnName("inventory_id")
                .HasColumnType("char(36)")
                .IsRequired();

            cfg.Property(p => p.UserId)
                .HasColumnName("user_id")
                .HasColumnType("char(36)")
                .IsRequired();

            cfg.Property(p => p.ProductSku)
                .HasColumnName("product_sku")
                .HasColumnType("char(36)")
                .IsRequired();

            cfg.Property(p => p.Amount)
                .HasColumnName("total_count")
                .HasDefaultValue(0)
                .IsRequired();

            cfg.Property(p => p.LastUpdated)
                .HasColumnName("last_updated_date")
                .HasColumnType("datetime(6)")
                .HasDefaultValue(DateTime.UtcNow)
                .IsRequired();
        });
    }
}