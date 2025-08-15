using MongoDB.Driver;
using ProjetoRl.ProjetoRl.Domain.AccessTokens;

namespace ProjetoRl.Infra.Data.Repositories.MongoDB.Implementation.AccessTokens;
#nullable enable
/// <summary>AccessTokens implementation repository in MongoDB.</summary>
public class AccessTokenRepositoryMongoDB : IAccessTokenRepository
{
    /// <summary>Context using to access data to AccessTokens.</summary>
    private readonly AccessTokenContext _accessTokenCtx = null!;

    /// <summary>Parameters constructor for initialization.</summary>
    /// <param name="config">Object that contaim configurations application  Objeto que contém as configurações da aplicação.</param>
    public AccessTokenRepositoryMongoDB(AccessTokenContext accessTokenContext)
    {
        _accessTokenCtx = accessTokenContext;
    }

    /// <summary>Obtain token details.</summary>
    /// <param name="jWTToken">Token desired.</param>
    /// <returns>Detailed Token Information.</returns>
    public async Task<AccessToken?> GetAsync(string jWTToken)
    {
        var builderToken = Builders<AccessTokenSchema>.Filter;
        var filter = builderToken.Eq(t => t.JWTToken, jWTToken);

        return await _accessTokenCtx.AccessTokens
            .Aggregate()
            .Match(filter)
            .FirstOrDefaultAsync();
    }

    /// <summary>Update a token.</summary>
    /// <param name="token">Token will be update.</param>
    public async Task SaveAsync(AccessToken token)
    {
        if (string.IsNullOrEmpty(token.ID))
        {
            AccessTokenSchema accessTokenModel = token;
            await _accessTokenCtx.AccessTokens.InsertOneAsync(accessTokenModel);
        }
        else
        {
            var filter = Builders<AccessTokenSchema>.Filter.Eq(c => c.ID, token.ID);
            var update = Builders<AccessTokenSchema>.Update
                .Set(c => c.Canceled, token.Canceled)
                .Set(c => c.Refreshed, token.Refreshed)
                .Set(c => c.RefreshedAt, token.RefreshedAt);

            await _accessTokenCtx.AccessTokens.UpdateOneAsync(filter, update);
        }
    }

    /// <summary>Remove a token.</summary>
    /// <param name="accessTokenId">Token will be remove.</param>
    public async Task RemoveAsync(string accessTokenId)
    {
        await _accessTokenCtx.AccessTokens.DeleteOneAsync(t => t.ID == accessTokenId);
    }

    /// <summary>Obtain the last generate token a user.</summary>
    /// <param name="userId">User identification code to desired recover the last token.</param>
    /// <returns>Last token access for a user.</returns>
    public async Task<AccessToken?> GetLastAccessTokenAsync(string userId)
    {
        var filter = Builders<AccessTokenSchema>.Filter.Eq(t => t.UserID, userId);

        return await _accessTokenCtx.AccessTokens
            .Aggregate()
            .Match(filter)
            .SortByDescending(t => t.CreatedAt)
            .FirstOrDefaultAsync();
    }

    /// <summary>Cancel a Token.</summary>
    /// <param name="userId">User identification code to desired cancel a token.</param>
    /// <param name="type">Type of passport that identify external service to pertence the identification code.</param>
    public async Task CancelTokenAsync(string userId)
    {
        var builder = Builders<AccessTokenSchema>.Filter;
        var filter = builder.Eq(u => u.UserID, userId);
        
        filter = filter & builder.Eq(a => a.ID, userId);

        var update = Builders<AccessTokenSchema>.Update
            .Set(c => c.Canceled, true);

        await _accessTokenCtx.AccessTokens.UpdateManyAsync(filter, update);
    }


}