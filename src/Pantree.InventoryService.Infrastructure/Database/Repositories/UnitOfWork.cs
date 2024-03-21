using Microsoft.EntityFrameworkCore.Storage;
using Pantree.InventoryService.Domain.Repositories;

namespace Pantree.InventoryService.Infrastructure.Database.Repositories;

/// <inheritdoc cref="IUnitOfWork" />
public sealed class UnitOfWork(AppDbContext ctx) : IUnitOfWork, IDisposable {
    
    private IDbContextTransaction? _transaction;

    public void Dispose() => _transaction?.Dispose();

    /// <inheritdoc cref="IUnitOfWork.BeginTransactionAsync" />
    public async Task BeginTransactionAsync(CancellationToken ct = default) {
        _transaction = await ctx.Database.BeginTransactionAsync(ct);
    }

    /// <inheritdoc cref="IUnitOfWork.CommitAsync" />
    public async Task CommitAsync(CancellationToken ct = default) {
        await _transaction?.CommitAsync(ct)!;
    }

    /// <inheritdoc cref="IUnitOfWork.RollbackAsync" />
    public async Task RollbackAsync(CancellationToken ct = default) {
        await _transaction?.RollbackAsync(ct)!;
    }
}