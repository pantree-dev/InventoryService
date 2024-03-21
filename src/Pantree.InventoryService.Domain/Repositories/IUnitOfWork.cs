namespace Pantree.InventoryService.Domain.Repositories;

/// <summary>
/// Provides a way to commit/persist changes to the domain objects.
/// </summary>
public interface IUnitOfWork {
    
    /// <summary>
    /// Starts a transaction for updating, inserting and removing multiple different
    /// domain objects to the data storage.
    /// </summary>
    /// <param name="ct">The current requests cancellation token</param>
    /// <returns></returns>
    Task BeginTransactionAsync(CancellationToken ct = default);
    
    /// <summary>
    /// Tells the data layer to persist all changes that have been made through the various
    /// domain model repositories.
    /// </summary>
    /// <param name="ct"></param>
    /// <returns></returns>
    Task CommitAsync(CancellationToken ct = default);

    /// <summary>
    /// Tells the data layer to discard all changes that the domain model repositories have made
    /// to the data storage.
    /// </summary>
    /// <param name="ct"></param>
    /// <returns></returns>
    Task RollbackAsync(CancellationToken ct = default);
}