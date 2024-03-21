using MediatR;
using Newtonsoft.Json;
using Pantree.InventoryService.Domain.DomainEvents;
using Pantree.InventoryService.Domain.Entities;
using Pantree.InventoryService.Domain.Exceptions;
using Pantree.InventoryService.Domain.Repositories;

namespace Pantree.InventoryService.Application.UserInventory.Commands;

public class DeleteUserInventoryCommand(Guid userId, Guid inventoryId) : IRequest<bool> {

    public Guid UserId { get; } = userId;

    public Guid InventoryId { get; } = inventoryId;
}

internal sealed class DeleteUserInventoryCommandHandler(
    IUserInventoryRepository userInventoryRepository,
    IEventOutboxRepository outboxRepository,
    IUnitOfWork unitOfWork
) : IRequestHandler<DeleteUserInventoryCommand, bool> {
    
    public async Task<bool> Handle(DeleteUserInventoryCommand request, CancellationToken cancellationToken) {
        await unitOfWork.BeginTransactionAsync(cancellationToken);
        try {
            // try and find the entity we want from the database
            var inventory = await userInventoryRepository.GetByIdAsync(request.InventoryId, cancellationToken);
            if (inventory is null) {
                return false; // generates the 404
            }

            // NOTE: we have this exception here so we can log when it happens/someone tries to access something
            if (inventory.UserId != request.UserId) {
                throw new UserInventoryInvalidAccessException(request.InventoryId, request.UserId);
            }
            
            // delete the record
            userInventoryRepository.Delete(inventory);
            
            // create an event and attach it to our outbox
            var evt = new UserInventoryProductRemovedEvent {
                InventoryId = inventory.Id,
                UserId = inventory.UserId,
                ProductSku = inventory.ProductSku,
                TotalAmount = inventory.Amount,
            };
            
            await outboxRepository.AddAsync(new EventOutbox {
                EventName = nameof(UserInventoryProductRemovedEvent),
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