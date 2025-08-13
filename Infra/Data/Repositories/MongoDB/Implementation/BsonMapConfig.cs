
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using ProjetoRl.Infra.Data.Repositories.MongoDB.Implementation.Motorcycles;
using ProjetoRl.Infra.Data.Repositories.MongoDB.Implementation.Rentals;
using ProjetoRl.Infra.Data.Repositories.MongoDB.Implementation.Users;

namespace ProjetoRl.Infra.Data.Repositories.MongoDB.Implementation;

/// <summary>Class configuration responsible for map general models in database.</summary>
public class BsonMapConfig
{
    /// <summary>Variable that verify if method was accessed another context.</summary>
    private bool _hit = false;

    /// <summary>Mapping method models.</summary>
    public void Config()
    {
        if (_hit)
            return;

        BsonClassMap.RegisterClassMap<BikeScheme>(map =>
        {
            map.AutoMap();
            map.MapIdField(u => u.ID)                
                .SetIdGenerator(StringObjectIdGenerator.Instance)
                .SetSerializer(new StringSerializer(BsonType.ObjectId));
            map.SetIgnoreExtraElements(true);
        });

        BsonClassMap.RegisterClassMap<RentalScheme>(map =>
        {
            map.AutoMap();
            map.MapIdField(a => a.ID)
                .SetIdGenerator(StringObjectIdGenerator.Instance)
                .SetSerializer(new StringSerializer(BsonType.ObjectId));
            map.SetIgnoreExtraElements(true);
        });

        BsonClassMap.RegisterClassMap<CourierScheme>(map =>
        {
            map.AutoMap();
            map.MapIdField(v => v.ID)
                .SetIdGenerator(StringObjectIdGenerator.Instance)
                .SetSerializer(new StringSerializer(BsonType.ObjectId));
            map.SetIgnoreExtraElements(true);
        });       

        BsonClassMap.RegisterClassMap<UserSchema>(map =>
        {
            map.AutoMap();
            map.MapIdMember(c => c.ID)
                .SetIdGenerator(StringObjectIdGenerator.Instance)
                .SetSerializer(new StringSerializer(BsonType.ObjectId));
            map.MapMember(u => u.Email)
                    .SetIgnoreIfNull(true)
                    .SetElementName("email") 
                    .SetIsRequired(false);
            map.SetIgnoreExtraElements(true);
        });       

        _hit = true;
    }
}