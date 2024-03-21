using Pantree.InventoryService.Domain.Entities;

namespace Pantree.InventoryService.Endpoints.FetchInventory;

/// <summary>
/// Represents a single record within a users inventory and holds information
/// on the product sku and the total amount they have of that product.
/// </summary>
public sealed class FetchInventoryResponse {
    
    /// <inheritdoc cref="UserInventory.Id" />
    public Guid Id { get; set; }
    
    /// <inheritdoc cref="UserInventory.UserId" />
    public Guid UserId { get; set; }
    
    /// <inheritdoc cref="UserInventory.ProductSku" />
    public Guid ProductSku { get; set; }
    
    /// <inheritdoc cref="UserInventory.Amount" />
    public int Amount { get; set; }
    
    /// <inheritdoc cref="UserInventory.LastUpdated" />
    public DateTime LastUpdated { get; set; }
}