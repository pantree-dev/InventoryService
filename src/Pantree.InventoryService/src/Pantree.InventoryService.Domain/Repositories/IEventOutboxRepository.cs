using Pantree.InventoryService.Domain.Entities;

namespace Pantree.InventoryService.Domain.Repositories;

/// <summary>
/// Primary repository for fetching, inserting and managing the outbox table
/// and event outbox domain model.
/// </summary>
public interface IEventOutboxRepository {

    /// <summary>
    /// Adds the domain model for the event to the current data storage transaction.
    /// </summary>
    /// <param name="entity">The domain model to add</param>
    /// <param name="ct">The current request cancellation token</param>
    /// <returns>The domain model added</returns>
    Task<EventOutbox> AddAsync(EventOutbox entity, CancellationToken ct = default);
}