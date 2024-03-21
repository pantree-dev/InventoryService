using FastEndpoints;
using MediatR;
using Pantree.InventoryService.Application.UserInventory.Commands;
using Pantree.InventoryService.Domain.Exceptions;

namespace Pantree.InventoryService.Endpoints.DeleteInventory;

public sealed class DeleteInventory : Endpoint<DeleteInventoryRequest, EmptyResponse> {
    
    // ReSharper disable once MemberCanBePrivate.Global
    public IMediator Mediator { get; set; } = null!;
    
    public override void Configure() {
        Delete("inventory/{InventoryId}");
    }

    public override async Task HandleAsync(DeleteInventoryRequest req, CancellationToken ct) {
        try {
            var command = new DeleteUserInventoryCommand(req.UserId, req.InventoryId);
            await Mediator.Send(command, ct);
            await SendNoContentAsync(ct);
        }
        catch (UserInventoryInvalidAccessException) {
            await SendNotFoundAsync(ct);
        }
    }
}