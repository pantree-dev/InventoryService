using Microsoft.EntityFrameworkCore;
using Pantree.InventoryService.Domain.Abstractions;
using Pantree.InventoryService.Domain.Entities;
using Pantree.InventoryService.Domain.Repositories;
using Pantree.InventoryService.Domain.Shared;

namespace Pantree.InventoryService.Infrastructure.Database.Repositories;

/// <inheritdoc cref="IUserInventoryRepository" />
public sealed class UserInventoryRepository(AppDbContext ctx) : IUserInventoryRepository {

    /// <inheritdoc cref="IUserInventoryRepository.AddAsync" />
    public async Task<UserInventory> AddAsync(UserInventory entity, CancellationToken ct = default) {
        await ctx.Set<UserInventory>().AddAsync(entity, ct);
        await ctx.SaveChangesAsync(ct);
        return entity;
    }

    /// <inheritdoc cref="IUserInventoryRepository.Delete" />
    public void Delete(UserInventory entity) {
        ctx.Set<UserInventory>().Remove(entity);
        ctx.SaveChanges();
    }
    
    /// <inheritdoc cref="IUserInventoryRepository.FetchInventoryForUserAsync" />
    public async Task<IPagedResult<Guid, UserInventory>> FetchInventoryForUserAsync(Guid userId,
        Guid? cursor,
        int pageSize,
        CancellationToken ct = default
    ) {
        var query = ctx.Set<UserInventory>()
            .AsNoTracking()
            .Where(x =>
                x.UserId == userId
                && (cursor == null || x.Id >= cursor.Value)
            )
            .OrderBy(x => x.ProductSku);

        var totalCount = await query.CountAsync(ct);
        var results = await query.Take(pageSize + 1).ToListAsync(ct);

        return new PagedResult<Guid, UserInventory>(
            results.Take(pageSize),
            totalCount,
            pageSize,
            results.Count > pageSize ? results.Last().Id : null,
            results.Count > 0 ? results.First().Id : null
        );
    }

    /// <inheritdoc cref="IUserInventoryRepository.GetByIdAsync" />
    public async Task<UserInventory?> GetByIdAsync(Guid inventoryId, CancellationToken ct = default) {
        return await ctx.Set<UserInventory>()
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == inventoryId, ct);
    }
    
    /// <inheritdoc cref="IUserInventoryRepository.GetByProductSkuForUserIdAsync" />
    public async Task<UserInventory?> GetByProductSkuForUserIdAsync(Guid userId,
        Guid productSku,
        CancellationToken ct = default
    ) {
        return await ctx.Set<UserInventory>()
            .AsNoTracking()
            .FirstOrDefaultAsync(x => 
                x.UserId == userId 
                && x.ProductSku == productSku, 
                ct
            );
    }

    /// <inheritdoc cref="IUserInventoryRepository.Update" />
    public void Update(UserInventory entity) {
        ctx.Set<UserInventory>().Update(entity);
        ctx.SaveChanges();
    }
}