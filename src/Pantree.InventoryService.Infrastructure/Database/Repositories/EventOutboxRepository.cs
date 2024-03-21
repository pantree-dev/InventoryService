using Pantree.InventoryService.Domain.Entities;
using Pantree.InventoryService.Domain.Repositories;

namespace Pantree.InventoryService.Infrastructure.Database.Repositories;

/// <inheritdoc cref="IEventOutboxRepository" />
public sealed class EventOutboxRepository(AppDbContext ctx) : IEventOutboxRepository {

    public async Task<EventOutbox> AddAsync(EventOutbox entity, CancellationToken ct = default) {
        await ctx.Set<EventOutbox>().AddAsync(entity, ct);
        await ctx.SaveChangesAsync(ct);
        return entity;
    }
}