

using Domain.Users;
using ProjetoRl.ProjetoRl.Commom;

namespace ProjetoRl.ProjetoRl.Domain.Users;

public interface IUserRepository
{
    Task<PagedResult<User>> ListAsync(string? email, string? firstName, string? lastName,
        IEnumerable<UserState>? states, IEnumerable<Role>? roles, uint pageIndex = 1, uint pageSize = 50);

    Task<User?> GetByIdAsync(string id);

    Task<User?> GetByEmailAsync(string email);

    Task<string> CreateAsync(User user);

    Task EditAsync(User user);

    Task RemoveAsync(string id);

    Task<IEnumerable<Role>> GetUserRolesAsync(string userID);

    Task ActivateAccountAsync(string userID);

    Task DeactivateAccountAsync(string userID, bool byUser);
}
