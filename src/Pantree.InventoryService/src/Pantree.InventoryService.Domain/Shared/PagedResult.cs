using System.Data.SqlTypes;
using Pantree.InventoryService.Domain.Abstractions;

namespace Pantree.InventoryService.Domain.Shared;

/// <inheritdoc cref="IPagedResult{T,TU}"/>
public class PagedResult<T, TU> : IPagedResult<T, TU> 
    where T : struct
    where TU : class {
    
    /// <inheritdoc cref="IPagedResult{T,TU}.Next"/>
    public T? Next { get; init; }

    /// <inheritdoc cref="IPagedResult{T,TU}.Previous"/>
    public T? Previous { get; init; }

    /// <inheritdoc cref="IPagedResult{T,TU}.TotalCount"/>
    public int TotalCount { get; init; }
    
    /// <inheritdoc cref="IPagedResult{T,TU}.PageSize"/>
    public int PageSize { get; init; }
    
    /// <inheritdoc cref="IPagedResult{T,TU}.TotalPages"/>
    public int TotalPages { get; init; }

    /// <inheritdoc cref="IPagedResult{T,TU}.Results"/>
    public IEnumerable<TU> Results { get; init; }
    
    public PagedResult(IEnumerable<TU> results, 
        int totalCount, 
        int pageSize, 
        T? next, 
        T? previous
    ) {
        Results = results;
        Next = next;
        Previous = previous;
        PageSize = pageSize;
        TotalCount = totalCount;
        
        // workout the total pages
        var realLimit = pageSize <= 0 ? 1 : pageSize;
        TotalPages = totalCount <= 0 ? 1 : (totalCount + realLimit - 1) / realLimit;
    }
}