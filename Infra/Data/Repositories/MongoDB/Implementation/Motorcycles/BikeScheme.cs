using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ProjetoRl.ProjetoRl.Domain.Motorcycles;

namespace ProjetoRl.Infra.Data.Repositories.MongoDB.Implementation.Motorcycles;

public class BikeScheme
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string ID { get; set; } = null!;

    [BsonElement("identifier")]
    public string Identifier { get; set; } = null!;

    [BsonElement("year")]
    public int Year { get; set; }

    [BsonElement("model")]
    public string Model { get; set; } = null!;

    [BsonElement("plate")]
    public string Plate { get; set; } = null!;

    public BikeScheme() { }
    
    public BikeScheme(string id, string identifier, int year, string model, string plate)
    {
        ID = id;
        Identifier = identifier;
        Year = year;
        Model = model;
        Plate = plate;
    }

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

