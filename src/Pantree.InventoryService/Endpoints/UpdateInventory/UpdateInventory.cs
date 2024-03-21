using FastEndpoints;
using MediatR;
using Pantree.InventoryService.Application.UserInventory.Commands;
using Pantree.InventoryService.Domain.Exceptions;

namespace Pantree.InventoryService.Endpoints.UpdateInventory;

public sealed class UpdateInventory : Endpoint<UpdateInventoryRequest, EmptyResponse> {
    
    // ReSharper disable once MemberCanBePrivate.Global
    public IMediator Mediator { get; set; } = null!;

    public override void Configure() {
        Put("inventory/{InventoryId}");
    }

    public override async Task HandleAsync(UpdateInventoryRequest req, CancellationToken ct) {
        try {
            var command = new UpdateUserInventoryCommand(req.UserId, req.InventoryId, req.Amount);
            await Mediator.Send(command, ct);
            await SendNoContentAsync(ct);
        }
        catch (UserInventoryInvalidAmountException invalid) {
            ThrowError(invalid.Message);
        }
        catch (UserInventoryInvalidAccessException) {
            await SendNotFoundAsync(ct);
        }
    }
}