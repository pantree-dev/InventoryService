using Microsoft.EntityFrameworkCore;
using Pantree.InventoryService.Infrastructure.Database;

namespace Pantree.InventoryService.Helpers;

public static class HostExtensions {

    public static IHost PreStartup(this IHost host) {
        // create a scope for the pre-startup (this gives us access to repos, etc)
        using var scope = host.Services.CreateScope();
        var serviceProvider = scope.ServiceProvider;
        
        // run any migrations and do any checks
        var ctx = serviceProvider.GetRequiredService<AppDbContext>();
        if (ctx!.Database!.ProviderName!.Contains("Sqlite")) {
            // this makes sure our test-db that is local is created with all latest changes
            ctx.Database.EnsureCreated();
        }
        else if (ctx.Database.GetPendingMigrations().Any()) {
            // this will make sure that when running in prod, that all the latest migrations are applied before
            // the application/microservice is started up fully.
            ctx.Database.Migrate();
        }
        
        // TODO: we could also do some pre-seeding of our service if we need specific data before startup
        
        return host;
    }
}