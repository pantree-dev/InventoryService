using System.Data.SqlTypes;

namespace Pantree.InventoryService.Domain.Abstractions;

/// <summary>
/// Domain level dto that wraps a collection of domain models to represent paging
/// </summary>
public interface IPagedResult<T, out TU> 
    where T : struct
    where TU : class {
    
    /// <summary>
    /// The unique id of the first domain modal in the next page of results
    /// </summary>
    T? Next { get; }
    
    /// <summary>
    /// The unique id of the first domain modal in the previous page of results
    /// </summary>
    T? Previous { get; }
    
    /// <summary>
    /// The total number of domain models in the data storage
    /// </summary>
    int TotalCount { get; }
    
    /// <summary>
    /// The size of your result set
    /// </summary>
    int PageSize { get; }
    
    /// <summary>
    /// The total number of pages given the page size of your results
    /// </summary>
    int TotalPages { get; }
    
    /// <summary>
    /// The current result set
    /// </summary>
    IEnumerable<TU> Results { get; }
}