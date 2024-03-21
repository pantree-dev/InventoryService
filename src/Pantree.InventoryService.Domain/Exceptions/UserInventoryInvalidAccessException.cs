namespace Pantree.InventoryService.Domain.Exceptions;

/// <summary>
/// Exception that gets thrown when the User-id from the request does not
/// match the User-id in the database that marks the owner of the inventory
/// record with the inventory-id supplied.
/// </summary>
public sealed class UserInventoryInvalidAccessException(Guid inventoryId, Guid userId, string message = "You are not the owner of this resource.") 
    : Exception(message) {

    /// <summary>
    /// The id of the user that tried to access the inventory
    /// </summary>
    public Guid UserId { get; } = userId;

    /// <summary>
    /// The id of the inventory item that the user-id tried to access but is not the owner of
    /// </summary>
    public Guid InventoryId { get; } = inventoryId;
}