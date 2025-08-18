using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ProjetoRl.ProjetoRl.Domain.Motorcycles;

namespace ProjetoRl.Infra.Data.Repositories.MongoDB.Implementation.Motorcycles;

/// <summary>
/// Bike scheme for MongoDB representation.
/// </summary>
public class BikeScheme
{
    /// <summary>
    /// Unique identifier for the bike.
    /// </summary>
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string ID { get; set; } = null!;

    /// <summary>
    /// Identifier for the bike, typically a VIN or similar unique code.
    /// </summary>

    [BsonElement("identifier")]
    public string Identifier { get; set; } = null!;

    /// <summary>
    /// Year of manufacture for the bike.
    /// </summary>
    [BsonElement("year")]
    public int Year { get; set; }

    /// <summary>
    /// Model of the bike.
    /// </summary>
    [BsonElement("model")]
    public string Model { get; set; } = null!;

    /// <summary>
    /// License plate of the bike.
    /// </summary>
    [BsonElement("plate")]
    public string Plate { get; set; } = null!;

    /// <summary>
    /// Default constructor for BikeScheme.
    /// </summary>
    public BikeScheme() { }

    /// <summary>
    /// Constructor with parameters to initialize the BikeScheme.   
    /// </summary>    
    public BikeScheme(string id, string identifier, int year, string model, string plate)
    {
        ID = id;
        Identifier = identifier;
        Year = year;
        Model = model;
        Plate = plate;
    }

    /// <summary>
    /// Implicit conversion from Bike to BikeScheme.
    /// </summary>
    /// <param name="entity">Class entitie.</param>
    public static implicit operator BikeScheme(Bike entity)
    {
        if (entity == null)
            return null!;

        return new(entity.ID,
                   entity.Identifier,
                   entity.Year,
                   entity.Model,
                   entity.Plate);
    }

    /// <summary>
    /// Implicit conversion from BikeScheme to Bike.
    /// </summary>
    /// <param name="scheme">Bike scheme class.</param>
    public static implicit operator Bike(BikeScheme scheme)
    {
        if (scheme == null)
            return null!;

        return new()
        {
            ID = scheme.ID,
            Identifier = scheme.Identifier,
            Year = scheme.Year,
            Model = scheme.Model,
            Plate = scheme.Plate
        };
    }
}

