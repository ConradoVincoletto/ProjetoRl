using Domain.Rentals;
using ProjetoRl.ProjetoRl.Commom;

namespace ProjetoRl.ProjetoRl.Domain.Rentals;

/// <summary>
/// Interface for rental repository operations.
/// </summary>
public interface IRentalRepository
{
    /// <summary>
    /// Lists rentals based on various filters and pagination.
    /// </summary> 
    Task<PagedResult<Rental>> ListAsync(string? bikeId, string? courierId,
        DateTime? startDateFrom, DateTime? startDateTo,
        uint pageIndex = 1, uint pageSize = 50);

    /// <summary>
    /// Retrieves a rental by its unique identifier.
    /// </summary>
    Task<Rental?> GetByIdAsync(string id);

    /// <summary>
    /// Retrieves a rental by its motorcycle identifier.
    Task<Rental?> GetRentalByBikeIdAsync(string id);

    /// <summary>
    /// Creates a new rental record.
    Task<string> CreateAsync(Rental rental);

    /// <summary>
    /// Updates an existing rental record.
    Task EditAsync(Rental rental);

    /// <summary>
    /// Removes a rental record by its unique identifier.
    /// </summary>
    Task RemoveAsync(string id);
}
