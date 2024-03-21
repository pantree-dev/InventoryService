using MediatR;
using Pantree.InventoryService.Domain.Abstractions;
using Pantree.InventoryService.Domain.Repositories;

namespace Pantree.InventoryService.Application.UserInventory.Queries;

/// <summary>
/// Query for getting a paged result set of a users current inventory.
/// </summary>
public sealed class FetchPagedUserInventoryQuery(Guid userId, Guid? cursor, int pageSize)
    : IRequest<IPagedResult<Guid, Domain.Entities.UserInventory>> {
    
    public Guid UserId { get; } = userId;

    public Guid? Cursor { get; } = cursor;

    public int PageSize { get; } = pageSize;
}

internal sealed class FetchPagedUserInventoryQueryHandler(IUserInventoryRepository repository)
    : IRequestHandler<FetchPagedUserInventoryQuery, IPagedResult<Guid, Domain.Entities.UserInventory>> {
    
    public async Task<IPagedResult<Guid, Domain.Entities.UserInventory>> Handle(FetchPagedUserInventoryQuery request, 
        CancellationToken cancellationToken
    ) {
        return await repository.FetchInventoryForUserAsync(
            request.UserId,
            request.Cursor,
            request.PageSize,
            cancellationToken
        );
    }
}