using Pantree.InventoryService.Domain.Abstractions;
using Pantree.InventoryService.Domain.Entities;

namespace Pantree.InventoryService.Domain.Repositories;

// TODO: check if we want to add EF Projection instead of mapping the domain object a DTO in another layer

/// <summary>
/// The primary repository for fetching, inserting and updating the domain entities for a User Inventory item.
/// </summary>
public interface IUserInventoryRepository {

    /// <summary>
    /// Adds the domain model to the current data storage transaction.
    /// </summary>
    /// <param name="entity">The domain model to add</param>
    /// /// <param name="ct">The current request cancellation token</param>
    /// <returns>The domain model added</returns>
    Task<UserInventory> AddAsync(UserInventory entity, CancellationToken ct = default);

    /// <summary>
    /// Removes the domain model from the current data storage transaction.
    /// </summary>
    /// <param name="entity">The entity to be removed</param>
    void Delete(UserInventory entity);
    
    /// <summary>
    /// Fetch a paged set of inventory items for a given user.
    /// </summary>
    /// <param name="userId">The current user identifier</param>
    /// <param name="cursor">The identifier for the first element in the page set you want</param>
    /// <param name="pageSize">The size of the result set you want</param>
    /// <param name="ct">The current request cancellation token</param>
    /// <returns></returns>
    Task<IPagedResult<Guid, UserInventory>> FetchInventoryForUserAsync(Guid userId, 
        Guid? cursor,
        int pageSize,
        CancellationToken ct = default
    );

    /// <summary>
    /// Get a single inventory item by the unique identifier.
    /// </summary>
    /// <param name="inventoryId">The inventory id</param>
    /// <param name="ct">The current request cancellation token</param>
    /// <returns></returns>
    Task<UserInventory?> GetByIdAsync(Guid inventoryId, CancellationToken ct = default);

    /// <summary>
    /// Get a single inventory item that matches the user-id and the product sku.
    /// </summary>
    /// <param name="userId">The current user identifier</param>
    /// <param name="productSku">The product SKU</param>
    /// <param name="ct">The current request cancellation token</param>
    /// <returns></returns>
    Task<UserInventory?> GetByProductSkuForUserIdAsync(Guid userId,
        Guid productSku,
        CancellationToken ct = default
    );

    /// <summary>
    /// Updates the domain model record.
    /// </summary>
    /// <param name="entity">The domain model you want to update</param>
    void Update(UserInventory entity);
}