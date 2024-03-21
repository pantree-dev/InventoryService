using Microsoft.EntityFrameworkCore;
using Pantree.InventoryService.Domain.Entities;

namespace Pantree.InventoryService.Infrastructure.Database.EntityConfigurations;

/// <summary>
/// Extension method for building the table structure for our event outbox model
/// </summary>
public static class EventOutboxEntityConfig {

    public static void RegisterEventOutboxEntity(this ModelBuilder builder) {
        builder.Entity<EventOutbox>(cfg => {
            // configure the base table properties
            cfg.ToTable("event_outbox");
            cfg.HasKey(pk => pk.Id);
            
            // configure the columns
            cfg.Property(p => p.Id)
                .HasColumnName("outbox_id")
                .ValueGeneratedOnAdd()
                .IsRequired();

            cfg.Property(p => p.EventName)
                .HasColumnName("event_name")
                .HasMaxLength(4098)
                .IsRequired();

            cfg.Property(p => p.EventData)
                .HasColumnName("event_data")
                .HasColumnType("mediumtext")
                .IsRequired();

            cfg.Property(p => p.CreatedDate)
                .HasColumnName("created_date")
                .HasColumnType("datetime(6)")
                .HasDefaultValue(DateTime.UtcNow)
                .IsRequired();

            cfg.Property(p => p.SentDate)
                .HasColumnName("sent_date")
                .HasColumnType("datetime(6)")
                .HasDefaultValue(null)
                .IsRequired(false);

            cfg.Property(p => p.TotalAttempts)
                .HasColumnName("total_attempts")
                .HasDefaultValue(0)
                .IsRequired();
        });
    }
}