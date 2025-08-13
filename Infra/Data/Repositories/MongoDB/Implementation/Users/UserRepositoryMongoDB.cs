using Domain.Users;
using MongoDB.Driver;
using ProjetoRl.Infra.Data.Repositories.MongoDB.Implementation.Users;
using ProjetoRl.ProjetoRl.Commom;
using ProjetoRl.ProjetoRl.Domain.Users;

namespace ProjetoRl.Data.Repositories.MongoDB.Implementation.Users;

/// <summary>Repository implementation of user in MongoDB.</summary> 
public class UserRepositoryMongoDB : IUserRepository
{
    #nullable enable
    /// <summary>Implementation of the user repository in MongoDB.</summary>
    private readonly UserContext _userCtx = null!;

    /// <summary>Constructor with parameters to initializing.</summary>
    /// <param name="config">Object containing application settings</param>
    public UserRepositoryMongoDB(UserContext userContext)
    {
        _userCtx = userContext;
    }

    /// <summary>Retrieves a page of user data from the platform..</summary>
    /// <param name="searchExpression">Search term applied to name and email address.</param>
    /// <param name="states">User states.</param>
    /// <param name="pageIndex">Current search page index.</param>
    /// <param name="pageSize">Number of records returned per page.</param>
    /// <returns>Data page containing the search results.</returns>
    public async Task<PagedResult<User>> ListAsync(string? email, string? firstName, string? lastName, IEnumerable<UserState>? states, IEnumerable<Role>? roles, uint pageIndex = 1, uint pageSize = 50)
    {
        var builder = Builders<UserSchema>.Filter;
        var sortBuilder = Builders<UserSchema>.Sort.Ascending(u => u.FirstName).Ascending(u => u.Email);


        var filter = builder.Ne(u => u.State, UserState.Removed);

        // O usuário utilizou critérios de busca.
        if (!string.IsNullOrEmpty(email))
             filter &= builder.Regex(u => u.Email, email);

        if (!string.IsNullOrEmpty(firstName))
            filter &= builder.Regex(u => u.FirstName, firstName);

        if (!string.IsNullOrEmpty(lastName))
            filter &= builder.Regex(u => u.LastName, lastName);

        var users = await _userCtx.Users
            .Aggregate()
            .Match(filter)
            .Sort(sortBuilder)
            .Skip((pageIndex - 1) * pageSize)
            .Limit(pageSize)
            .ToListAsync();

        return new PagedResult<User>(pageSize, pageIndex, pageIndex, users.Select(u => (User)u).ToList());
    }

    /// <summary>Obtains user information from the repository based on their identification code.</summary>
    /// <param name="id">Identification code of the user that will be used as a filter.</param>
    /// <returns>Entity that represents the user found.</returns>
    public async Task<User?> GetByIdAsync(string id)
    {
        var builder = Builders<UserSchema>.Filter;
        var filter = builder.Eq(u => u.ID, id);

        return await _userCtx.Users
            .Aggregate()
            .Match(filter)
            .FirstOrDefaultAsync();
    }

    /// <summary>Gets the information of a user in the repository based on their email address.</summary>
    /// <param name="email">Email address of the user that will be used as a filter.</param>
    /// <returns>Entity that represents the user found.</returns>
    public async Task<User?> GetByEmailAsync(string email)
    {
        var builder = Builders<UserSchema>.Filter;
        var filter = builder.Eq(u => u.Email, email) &
            builder.Ne(u => u.State, UserState.Removed);

        return await _userCtx.Users
            .Aggregate()
            .Match(filter)
            .FirstOrDefaultAsync();
    }

    /// <summary>Registers a new user in the repository.</summary>
    /// <param name="user">Entity containing the information of the user to be registered.</param>
    /// <returns>Identification code generated for the newly registered user.</returns>
    public async Task<string> CreateAsync(User user)
    {
        var userSchema = (UserSchema)user;
        await _userCtx.Users.InsertOneAsync(userSchema);
        return userSchema.ID!;
    }

    /// <summary>Edits a user's information in the repository.</summary>
    /// <param name="user">Entity containing the information of the user to be edited.</param>
    public async Task EditAsync(User user)
    {
        var builder = Builders<UserSchema>.Filter;
        var filter = builder.Eq(u => u.ID, user.ID);

        await _userCtx.Users.ReplaceOneAsync(filter, user);
    }

    /// <summary>Deletes a user.</summary>
    /// <param name="id">Identification code of the user to be deleted.</param>
    public async Task RemoveAsync(string id)
    {
        var filter = Builders<UserSchema>.Filter.Eq(u => u.ID, id);

        var update = Builders<UserSchema>.Update
            .Set(u => u.State, UserState.Removed);

        await _userCtx.Users.UpdateOneAsync(filter, update);
    }

    /// <summary>Retrieves the access profiles of a specific user from the repository.</summary>
    /// <param name="userID">Identification code of the user for whom the access profiles are to be retrieved.</param>
    /// <returns>List of access profiles assigned to the user.</returns>
    public async Task<IEnumerable<Role>> GetUserRolesAsync(string userID)
    {
        var builder = Builders<UserSchema>.Filter;
        var filter = builder.Eq(u => u.ID, userID);

        return await _userCtx.Users
            .Aggregate()
            .Match(filter)
            .Project(u => u.Roles)
            .FirstOrDefaultAsync();
    }

    /// <summary>Activate user account.</summary>
    /// <param name="userID">Identification code of the user to be activate.</param>
    public async Task ActivateAccountAsync(string userID)
    {
        var filter = Builders<UserSchema>.Filter.Eq(u => u.ID, userID);

        var update = Builders<UserSchema>.Update.Set(u => u.State, UserState.Active);

        await _userCtx.Users.UpdateOneAsync(filter, update);
    }

    /// <summary>Deactivate user account.</summary>
    /// <param name="userID">Identification code of the user to be deactivate.</param>
    /// <param name="byUser">Flag that indicates if deactivation is made by user.</param>
    public async Task DeactivateAccountAsync(string userID, bool byUser)
    {
        var filter = Builders<UserSchema>.Filter.Eq(u => u.ID, userID);

        if (byUser)
        {
            var update = Builders<UserSchema>.Update.Set(ubu => ubu.State, UserState.DeactivatedByUser);
            await _userCtx.Users.UpdateOneAsync(filter, update);
        }
        else
        {
            var update = Builders<UserSchema>.Update.Set(u => u.State, UserState.Deactivated);
            await _userCtx.Users.UpdateOneAsync(filter, update);
        }

    }
}