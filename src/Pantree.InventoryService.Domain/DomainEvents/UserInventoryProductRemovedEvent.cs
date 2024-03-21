namespace Pantree.InventoryService.Domain.DomainEvents;

/// <summary>
/// Domain level event that is emitted when a user completely removes the item from
/// their inventory. This event does not necessarily have to occur when the inventory
/// count of a product reaches 0 as the user might want to keep it known to their inventory.
/// </summary>
public sealed class UserInventoryProductRemovedEvent {
    
    /// <summary>
    /// The unique identifier for the inventory record.
    /// </summary>
    public Guid InventoryId { get; set; }
    
    /// <summary>
    /// The user identifier for the inventory that the record belongs too.
    /// </summary>
    public Guid UserId { get; set; }
    
    /// <summary>
    /// The product sku identifier that forms part of the key for the inventory.
    /// </summary>
    public Guid ProductSku { get; set; }
    
    /// <summary>
    /// The total amount that the user had of that inventory item at the time they
    /// removed it from their inventory.
    /// </summary>
    public int TotalAmount { get; set; }
    
    /// <summary>
    /// The date that the event was triggered.
    /// </summary>
    public DateTime EventDate { get; set; } = DateTime.UtcNow;
}