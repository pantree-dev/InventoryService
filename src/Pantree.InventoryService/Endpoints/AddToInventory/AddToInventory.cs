using FastEndpoints;
using MediatR;
using Pantree.InventoryService.Application.UserInventory.Commands;

namespace Pantree.InventoryService.Endpoints.AddToInventory;

public sealed class AddToInventory : Endpoint<AddToInventoryRequest, AddToInventoryResponse> {
    
    // ReSharper disable once MemberCanBePrivate.Global
    public IMediator Mediator { get; set; } = null!;
    
    public override void Configure() {
        Post("inventory");
    }

    public override async Task HandleAsync(AddToInventoryRequest req, CancellationToken ct) {
        var command = new AddToUserInventoryCommand(req.ProductSku, req.UserId, req.Amount);
        var response = await Mediator.Send(command, ct);

        await SendOkAsync(new AddToInventoryResponse {
            Id = response,
            ProductSku = req.ProductSku,
            Total = 0 // TODO?
        }, ct);
    }
}