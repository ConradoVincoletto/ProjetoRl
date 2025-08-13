using MongoDB.Driver;
using ProjetoRl.Infra.Data.Repositories.MongoDB.Implementation.Rentals;

namespace ProjetoRl.Infra.Data.Repositories.MongoDB.Implementation.Users;

/// <summary>Context using to access to data users.</summary>
public class RentalContext
{
    /// <summary>MongoDB database where stored users data.</summary>
    private readonly IMongoDatabase _database = null!;

    /// <summary>Constructor with parameters to initialization.</summary>
    /// <param name="client">Cliente MongoDB utilizado para consulta de dados.</param>
    public RentalContext(IMongoClient client)
    {
        if (client != null)
            _database = client.GetDatabase("projetoRl");

        // var bsonConfig = new BsonMapConfig();
        // bsonConfig.Config();
    }

    /// <summary>Users collection.</summary>
    public IMongoCollection<RentalScheme> Rentals
    {
        get
        {
            return _database.GetCollection<RentalScheme>("rental"); 
        }
    }
}