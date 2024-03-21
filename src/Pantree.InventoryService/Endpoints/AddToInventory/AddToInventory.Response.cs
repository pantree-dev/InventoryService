namespace Pantree.InventoryService.Endpoints.AddToInventory;

public sealed class AddToInventoryResponse {
    
    public Guid Id { get; set; }
    
    public Guid ProductSku { get; set; }
    
    public int Total { get; set; }
}