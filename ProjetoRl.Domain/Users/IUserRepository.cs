
using Domain.Users;
using ProjetoRl.ProjetoRl.Commom;

namespace ProjetoRl.ProjetoRl.Domain.Users;

/// <summary>
/// Interface method of user account.
/// </summary>

public interface IUserRepository
{

    /// <summary>
    /// Mehot list all user account.
    /// </summary>
    Task<PagedResult<User>> ListAsync(string? email,
                                      string? firstName,
                                      string? lastName,
                                      IEnumerable<UserState>? states,
                                      IEnumerable<Role>? roles,
                                      uint pageIndex = 1,
                                      uint pageSize = 50);
                                      

    /// <summary>
    /// Get user by identification code.
    /// </summary>  
    Task<User?> GetByIdAsync(string id);

    /// <summary>
    /// Get user by email.
    /// </summary>
    Task<User?> GetByEmailAsync(string email);

    /// <summary>
    /// Create a new user account.
    /// </summary>
    Task<string> CreateAsync(User user);

    /// <summary>
    /// Edita a ixisting user account.
    /// </summary>
    Task EditAsync(User user);

    /// <summary>
    /// Remove a user account.
    /// </summary>
    Task RemoveAsync(string id);

    /// <summary>
    /// Get user which function.
    /// </summary>

    Task<IEnumerable<Role>> GetUserRolesAsync(string userID);

    /// <summary>
    /// Activted a user account
    /// </summary>
    Task ActivateAccountAsync(string userID);

    /// <summary>
    /// Deativeted a user account.
    /// </summary>
    Task DeactivateAccountAsync(string userID, bool byUser);
}
