namespace Pantree.InventoryService.Domain.Exceptions;

public sealed class UserInventoryInvalidAmountException(string message) : Exception(message) { }