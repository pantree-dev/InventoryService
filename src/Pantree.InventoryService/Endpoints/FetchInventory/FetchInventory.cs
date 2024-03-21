using FastEndpoints;
using MediatR;
using Pantree.InventoryService.Application.UserInventory.Queries;
using Pantree.InventoryService.Domain.Abstractions;
using Pantree.InventoryService.Domain.Entities;
using Pantree.InventoryService.Domain.Shared;

namespace Pantree.InventoryService.Endpoints.FetchInventory;

/// <summary>
/// Endpoint that fetches a paged list of items in a users inventory.
/// </summary>
public sealed class FetchInventory : EndpointWithMapping<
    FetchInventoryRequest, 
    IPagedResult<Guid, FetchInventoryResponse>, 
    IPagedResult<Guid, UserInventory>
> {

    // ReSharper disable once MemberCanBePrivate.Global
    public IMediator Mediator { get; set; } = null!;
    
    public override void Configure() {
        Get("inventory");
    }

    public override async Task HandleAsync(FetchInventoryRequest req, CancellationToken ct) {
        // use our mediatr pipeline to query the data
        var query = new FetchPagedUserInventoryQuery(req.UserId, req.Cursor, req.Limit);
        var queryResult = await Mediator.Send(query, ct);

        // map the query result to our view-model and return okay
        var results = MapFromEntity(queryResult);
        await SendOkAsync(results, ct);
    }

    public override IPagedResult<Guid, FetchInventoryResponse> MapFromEntity(IPagedResult<Guid, UserInventory> e) {
        return new PagedResult<Guid, FetchInventoryResponse>(
            e.Results.Select(x => new FetchInventoryResponse {
                Id = x.Id,
                UserId = x.UserId,
                ProductSku = x.ProductSku,
                Amount = x.Amount,
                LastUpdated = x.LastUpdated
            }),
            e.TotalCount,
            e.PageSize,
            e.Next,
            e.Previous
        );
    }
}