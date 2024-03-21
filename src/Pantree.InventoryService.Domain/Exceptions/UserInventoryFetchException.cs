namespace Pantree.InventoryService.Domain.Exceptions;

public class UserInventoryFetchException(string message = "There was a problem trying to fetch your Inventory.") 
    : Exception(message) {

    public string Code { get; } = "0001"; // TODO: code-list
}