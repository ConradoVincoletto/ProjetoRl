using MongoDB.Driver;

namespace ProjetoRl.Infra.Data.Repositories.MongoDB.Implementation.Users;

/// <summary>Context using to access to data users.</summary>
public class CourierContext
{
    /// <summary>MongoDB database where stored users data.</summary>
    private readonly IMongoDatabase _database = null!;

    /// <summary>Constructor with parameters to initialization.</summary>
    /// <param name="client">Cliente MongoDB utilizado para consulta de dados.</param>
    public CourierContext(IMongoClient client)
    {
        if (client != null)
            _database = client.GetDatabase("projetoRl");

        // var bsonConfig = new BsonMapConfig();
        // bsonConfig.Config();
    }

    /// <summary>Users collection.</summary>
    public IMongoCollection<CourierScheme> Couriers
    {
        get
        {
            return _database.GetCollection<CourierScheme>("courier"); 
        }
    }
}