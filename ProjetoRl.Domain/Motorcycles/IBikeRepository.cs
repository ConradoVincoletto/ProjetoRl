using ProjetoRl.ProjetoRl.Commom;

namespace ProjetoRl.ProjetoRl.Domain.Motorcycles;

/// <summary>
/// Interface for motorcycle repository operations.
/// </summary>
public interface IBikeRepository
{
    /// <summary>
    /// Lists motorcycles based on various filters and pagination.
    /// </summary>    
    Task<PagedResult<Bike>> ListAsync(string? identifier,
                                      int? year,
                                      string? model,
                                      string? plate,
                                      uint pageIndex = 1,
                                      uint pageSize = 50);

    /// <summary>
    /// Retrieves a motorcycle by its unique identifier.
    /// </summary>   
    Task<Bike?> GetByIdAsync(string id);

    /// <summary>
    /// Retrieves a motorcycle by its license plate.
    /// </summary>
    Task<Bike?> GetByPlateAsync(string plate);

    /// <summary>
    /// Creates a new motorcycle record.
    /// </summary>
    Task<string> CreateAsync(Bike bike);

    /// <summary>
    /// Updates an existing motorcycle record.
    /// </summary>
    Task EditAsync(Bike bike);

    /// <summary>
    /// Removes a motorcycle record by its unique identifier.
    /// </summary>
    Task RemoveAsync(string id);
}
