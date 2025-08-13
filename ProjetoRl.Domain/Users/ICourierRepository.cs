
using Domain.Users;
using ProjetoRl.ProjetoRl.Commom;

namespace ProjetoRl.ProjetoRl.Domain.Users;

public interface ICourierRepository
{
    public Task<PagedResult<Courier>> ListAsync(string? identifier,
                                                string? name,
                                                string? cnpj,
                                                string? driverLicenseNumber,
                                                IEnumerable<UserState>? states,
                                                uint pageIndex = 1,
                                                uint pageSize = 50);

    Task<Courier?> GetByIdAsync(string id);

    Task<string> CreateAsync(Courier courier);

    Task EditAsync(Courier courier);

    Task RemoveAsync(string id);

    public Task ActivateCourirerAccountAsync(string id);

    public Task DeactivateCourierAccountAsync(string id);
}
