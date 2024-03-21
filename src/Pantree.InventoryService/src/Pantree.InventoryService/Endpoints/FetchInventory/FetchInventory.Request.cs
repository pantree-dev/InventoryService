using FastEndpoints;

namespace Pantree.InventoryService.Endpoints.FetchInventory;

public sealed class FetchInventoryRequest {
    
    // TODO: check if this is how we want to go about this! I'm assuming we leave the token auth to the gateway and then pass up the validated user-id in the header?
    [FromHeader(HeaderName = "user-id", IsRequired = true)]
    public Guid UserId { get; set; }
    
    public Guid? Cursor { get; set; }
    
    public int Limit { get; set; }
}