using ProjetoRl.ProjetoRl.Commom;

namespace ProjetoRl.ProjetoRl.Domain.Motorcycles;

public interface IBikeRepository
{
    Task<PagedResult<Bike>> ListAsync(string? identifier,
                                      int? year,
                                      string? model,
                                      string? plate,
                                      uint pageIndex = 1,
                                      uint pageSize = 50);

    Task<Bike?> GetByIdAsync(string id);
    Task<Bike?> GetByPlateAsync(string plate);
    Task<string> CreateAsync(Bike bike);

    Task EditAsync(Bike bike);

    Task RemoveAsync(string id);
}
