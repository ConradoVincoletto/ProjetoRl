
namespace ProjetoRl.ProjetoRl.Commom;

/// <summary>Used to store pagination and pagination results of a query.</summary>
public class PagedResult<T>
{
    /// <summary>Total records returned from the database.</summary>
    public long TotalRows { get; }

    /// <summary>Total pages the query returned with the current filters.</summary>
    public long Pages { get; }

    /// <summary>Current page of the query.</summary>
    public long CurrentPageIndex { get; }

    /// <summary>Data returned from the database query.</summary>
    public IEnumerable<T> Data { get; }

    /// <summary>Constructor with initialization parameters.</summary>
    /// <param name="totalRows">Total records returned from the database.</param>
    /// <param name="pages">Total pages the query returned with the current filters.</param>
    /// <param name="currentPageIndex">Current page of the query.</param>
    /// <param name="data">Data returned from the database query.</param>
    public PagedResult(long totalRows, long pages, long currentPageIndex, IEnumerable<T> data)
    {
        TotalRows = totalRows;
        Pages = pages;
        CurrentPageIndex = currentPageIndex;
        Data = data;
    }
}
