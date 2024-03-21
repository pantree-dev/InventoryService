using FastEndpoints;

namespace Pantree.InventoryService.Endpoints.UpdateInventory;

public sealed class UpdateInventoryRequest {
    
    // TODO: check if this is how we want to go about this! I'm assuming we leave the token auth to the gateway and then pass up the validated user-id in the header?
    [FromHeader(HeaderName = "user-id", IsRequired = true)]
    public Guid UserId { get; set; }
    
    public Guid InventoryId { get; set; }
    
    public int Amount { get; set; }
}