using Microsoft.EntityFrameworkCore;
using Pantree.InventoryService.Infrastructure.Database.EntityConfigurations;
using Pantree.InventoryService.Infrastructure.Database.Functions;

namespace Pantree.InventoryService.Infrastructure.Database;

public sealed class AppDbContext(DbContextOptions<AppDbContext> opts) : DbContext(opts) {
    
    protected override void OnModelCreating(ModelBuilder builder) {
        // register the domain model table structures
        builder.RegisterEventOutboxEntity();
        builder.RegisterUserInventoryEntity();
        
        // register the functions that will provide help when querying our data
        GuidFunctions.Register(builder);
        
        // register the domain model relationships
        builder.RegisterAppDbContextRelationships();
        base.OnModelCreating(builder);
    }
}