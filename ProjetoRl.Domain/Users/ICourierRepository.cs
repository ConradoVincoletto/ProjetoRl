using ProjetoRl.ProjetoRl.Commom;

namespace ProjetoRl.ProjetoRl.Domain.Users;

/// <summary>
/// Interface method of Courier.
/// </summary>

public interface ICourierRepository
{

    /// <summary>
    /// List all couriers.
    /// </summary>
    public Task<PagedResult<Courier>> ListAsync(string? identifier,
                                                string? name,
                                                string? cnpj,
                                                string? driverLicenseNumber,
                                                IEnumerable<UserState>? states,
                                                uint pageIndex = 1,
                                                uint pageSize = 50);

    /// <summary>
    /// Get a courier by identification code.
    /// </summary>
    Task<Courier?> GetByIdAsync(string id);

    /// <summary>
    /// Create a new courier.
    /// </summary>
    Task<string> CreateAsync(Courier courier);

    /// <summary>
    /// Edita a courier existing.
    /// </summary>
    Task EditAsync(Courier courier);

    /// <summary>
    /// Remove a courier existing.
    /// </summary>
    Task RemoveAsync(string id);

    /// <summary>
    /// Activeted a courier.
    /// </summary>
    public Task ActivateCourirerAccountAsync(string id);

    /// <summary>
    /// Deactiveted a courier.
    /// </summary>
    public Task DeactivateCourierAccountAsync(string id);
}
