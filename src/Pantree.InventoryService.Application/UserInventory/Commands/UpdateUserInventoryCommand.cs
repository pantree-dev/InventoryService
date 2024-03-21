using MediatR;
using Newtonsoft.Json;
using Pantree.InventoryService.Domain.DomainEvents;
using Pantree.InventoryService.Domain.Entities;
using Pantree.InventoryService.Domain.Exceptions;
using Pantree.InventoryService.Domain.Repositories;

namespace Pantree.InventoryService.Application.UserInventory.Commands;

public class UpdateUserInventoryCommand(Guid userId, Guid inventoryId, int amount) : IRequest<bool> {

    public Guid InventoryId { get; } = inventoryId;

    public Guid UserId { get; } = userId;

    public int Amount { get; } = amount;
}

internal sealed class UpdateUserInventoryCommandHandler(
    IUserInventoryRepository userInventoryRepository,
    IEventOutboxRepository outboxRepository,
    IUnitOfWork unitOfWork
) : IRequestHandler<UpdateUserInventoryCommand, bool> {
    
    public async Task<bool> Handle(UpdateUserInventoryCommand request, CancellationToken cancellationToken) {
        await unitOfWork.BeginTransactionAsync(cancellationToken);
        try {
            // try find the entity we wish to update
            var inventory = await userInventoryRepository.GetByIdAsync(request.InventoryId, cancellationToken);
            if (inventory is null) {
                return false; // generates the 404
            }
            
            // NOTE: we have this exception here so we can log when it happens/someone tries to access something
            if (inventory.UserId != request.UserId) {
                throw new UserInventoryInvalidAccessException(request.InventoryId, request.UserId);
            }
            
            // do some validation checks against the inventory item
            // this is typical checks we can't really do with fluent validation as it requires our model data
            if ((inventory.Amount + request.Amount) < 0) {
                throw new UserInventoryInvalidAmountException("Inventory amount cannot be less than zero.");
            }

            // update the domain model
            inventory.Amount += request.Amount;
            inventory.LastUpdated = DateTime.UtcNow;
            userInventoryRepository.Update(inventory);
            
            // create an event for the update model
            var evt = new UserInventoryProductChangedEvent {
                UserId = inventory.UserId,
                ProductSku = inventory.ProductSku,
                TotalAmount = inventory.Amount
            };
                
            // add our event to our data storage and then commit the transaction
            await outboxRepository.AddAsync(new EventOutbox {
                EventName = nameof(UserInventoryProductChangedEvent),
                EventData = JsonConvert.SerializeObject(evt),
            }, cancellationToken);
            
            // commit the transaction and return
            await unitOfWork.CommitAsync(cancellationToken);
            return true;
        }
        catch (Exception ex) {
            await unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }
}