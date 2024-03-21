namespace Pantree.InventoryService.Domain.Entities;

/// <summary>
/// Domain model that maps a product sku to a user account including the total number of
/// that product the user has.
/// </summary>
public sealed class UserInventory {

    /// <summary>
    /// Unique identifier/primary key for the domain models record.
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();
    
    /// <summary>
    /// The user identifier that forms the unique index between user & product.
    /// </summary>
    public Guid UserId { get; set; }
    
    /// <summary>
    /// The product sku identifier that forms part of the unique index between user & product.
    /// </summary>
    public Guid ProductSku { get; set; }

    /// <summary>
    /// The total amount of that product the user currently holds.
    /// </summary>
    public int Amount { get; set; } = 0;
    
    /// <summary>
    /// The last time that product inventory amount was updated.
    /// </summary>
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
}