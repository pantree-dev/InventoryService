namespace Pantree.InventoryService.Domain.DomainEvents;

/// <summary>
/// Domain level event that is emitted when a user adds a new product to their inventory
/// or updates an existing product with more items (increasing the total amount they have).
/// </summary>
public sealed class UserInventoryProductChangedEvent {
    
    /// <summary>
    /// The user account the event is tied too.
    /// </summary>
    public Guid UserId { get; set; }
    
    /// <summary>
    /// The product identifier to point to the affected product in the users inventory.
    /// </summary>
    public Guid ProductSku { get; set; }
    
    /// <summary>
    /// The total amount after the inventory has been modified. This ensures idempotency and duplicate event issues.
    /// </summary>
    public int TotalAmount { get; set; }

    /// <summary>
    /// Internal flag primarily used by the messaging to denote that the inventory item/product
    /// was actually added and is new to the users inventory.
    /// </summary>
    public bool IsNew { get; set; } = false;
    
    /// <summary>
    /// The date the event was triggered.
    /// </summary>
    public DateTime EventDate { get; set; } = DateTime.UtcNow;
}