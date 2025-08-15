using MongoDB.Driver;
using ProjetoRl.Infra.Data.Repositories.MongoDB.Implementation.Users;

namespace ProjetoRl.Infra.Data.Repositories.MongoDB.Implementation.AccessTokens;

/// <summary>Context using to access data to AccessTokens.</summary>
public class AccessTokenContext
{
    /// <summary>MongoDB database where user stored.</summary>
    private readonly IMongoDatabase _database = null!;

    /// <summary>Parameters constructor to initialization.</summary>
    /// <param name="client">Cliente MongoDB utilizado para consulta de dados.</param>
    public AccessTokenContext(IMongoClient client)
    {
        if (client != null)
            _database = client.GetDatabase("projetoRl");

        // var bsonConfig = new BsonMapConfig();
        // bsonConfig.Config();
    }

    /// <summary>AccessTokens colletion.</summary>
    public IMongoCollection<AccessTokenSchema> AccessTokens
    {
        get
        {
            return _database.GetCollection<AccessTokenSchema>("access_token");
        }
    }

    /// <summary>Users collection.</summary>
    public IMongoCollection<UserSchema> Users
    {
        get
        {
            return _database.GetCollection<UserSchema>("user");
        }
    }
}