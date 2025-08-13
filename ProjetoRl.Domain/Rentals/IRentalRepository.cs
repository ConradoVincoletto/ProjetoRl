using Domain.Rentals;
using ProjetoRl.ProjetoRl.Commom;

namespace ProjetoRl.ProjetoRl.Domain.Rentals;

public interface IRentalRepository
{
    Task<PagedResult<Rental>> ListAsync(string? bikeId, string? courierId,
        DateTime? startDateFrom, DateTime? startDateTo,
        uint pageIndex = 1, uint pageSize = 50);

    Task<Rental?> GetByIdAsync(string id);

    Task<Rental?> GetRentalByBikeIdAsync(string id);

    Task<string> CreateAsync(Rental rental);

    Task EditAsync(Rental rental);

    Task RemoveAsync(string id);
}
