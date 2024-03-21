using MediatR;
using Newtonsoft.Json;
using Pantree.InventoryService.Domain.DomainEvents;
using Pantree.InventoryService.Domain.Entities;
using Pantree.InventoryService.Domain.Repositories;

namespace Pantree.InventoryService.Application.UserInventory.Commands;

public class AddToUserInventoryCommand(Guid productSku, Guid userId, int amount) : IRequest<Guid> {

    public Guid ProductSku { get; } = productSku;

    public Guid UserId { get; } = userId;

    public int Amount { get; } = amount;
}

internal sealed class AddToUserInventoryCommandHandler(
    IUserInventoryRepository userInventoryRepository, 
    IEventOutboxRepository outboxRepository, 
    IUnitOfWork unitOfWork
) : IRequestHandler<AddToUserInventoryCommand, Guid> {
    
    public async Task<Guid> Handle(AddToUserInventoryCommand request, CancellationToken cancellationToken) {
        await unitOfWork.BeginTransactionAsync(cancellationToken);
        try {
            // check if we have this product already in our inventory
            var inventory = await userInventoryRepository.GetByProductSkuForUserIdAsync(
                request.UserId,
                request.ProductSku,
                cancellationToken
            );

            return inventory is null
                ? await AddNewAsync(request, cancellationToken)
                : await UpdateExistingAsync(inventory, request, cancellationToken);
        }
        catch (Exception ex) {
            await unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }

    private async Task<Guid> AddNewAsync(AddToUserInventoryCommand req, CancellationToken ct) {
        // create & add the new entity to the repository
        var inventory = new Domain.Entities.UserInventory() {
            UserId = req.UserId,
            ProductSku = req.ProductSku,
            Amount = req.Amount
        };
        await userInventoryRepository.AddAsync(inventory, ct);
            
        // create our event and add that to the repository/data storage
        var evt = new UserInventoryProductChangedEvent {
            UserId = inventory.UserId,
            ProductSku = inventory.ProductSku,
            TotalAmount = inventory.Amount,
            IsNew = true
        };
        
        await outboxRepository.AddAsync(new EventOutbox {
            EventName = nameof(UserInventoryProductChangedEvent),
            EventData = JsonConvert.SerializeObject(evt),
        }, ct);
        
        // commit the current transaction and return the guid of the new inventory item
        await unitOfWork.CommitAsync(ct);
        return inventory.Id;
    }

    private async Task<Guid> UpdateExistingAsync(Domain.Entities.UserInventory inventory, 
        AddToUserInventoryCommand req,
        CancellationToken ct
    ) {
        // update the amount with the new "added" amount
        inventory.Amount += req.Amount;
        inventory.LastUpdated = DateTime.UtcNow;
        userInventoryRepository.Update(inventory);
                
        // create the event
        var evt = new UserInventoryProductChangedEvent {
            UserId = inventory.UserId,
            ProductSku = inventory.ProductSku,
            TotalAmount = inventory.Amount
        };
                
        // add our event to our data storage and then commit the transaction
        await outboxRepository.AddAsync(new EventOutbox {
            EventName = nameof(UserInventoryProductChangedEvent),
            EventData = JsonConvert.SerializeObject(evt),
        }, ct);
        await unitOfWork.CommitAsync(ct);
        return inventory.Id;
    }
}