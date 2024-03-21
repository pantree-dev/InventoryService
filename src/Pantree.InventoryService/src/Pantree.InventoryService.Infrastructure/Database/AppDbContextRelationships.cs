using Microsoft.EntityFrameworkCore;

namespace Pantree.InventoryService.Infrastructure.Database;

/// <summary>
/// Extension methods for providing domain model relationship mappings
/// </summary>
public static class AppDbContextRelationships {

    public static void RegisterAppDbContextRelationships(this ModelBuilder builder) {
        // TODO: once we know what domain models we need then come back and setup the relationships for our tables
    }
}