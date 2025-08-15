using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ProjetoRl.ProjetoRl.Domain.AccessTokens;

namespace ProjetoRl.Infra.Data.Repositories.MongoDB.Implementation.AccessTokens;


/// <summary>Representation access token in mongoDB database.</summary>
public class AccessTokenSchema
{

    /// <summary>Token identification code.</summary>
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string ID { get; private set; } = null!;

    /// <summary>Type of identification code generate token for access key.</summary>

    /// <summary>Token JWT code to control user access.</summary>
    [BsonElement("jwt_token")]
    public string JWTToken { get; set; } = null!;

    /// <summary>User identification code to reference the token.</summary>
    [BsonElement("user_id"), BsonRepresentation(BsonType.ObjectId)]
    public string UserID { get; set; } = null!;

    /// <summary>IP that generated the token .</summary>
    [BsonElement("ip")]
    #nullable enable
    [BsonRepresentation(BsonType.String)]
    public string? IP { get; set; } 

    /// <summary>Flag indicating whether the Token has been updated.</summary>
    [BsonElement("refreshed")]
    public bool Refreshed { get; set; }

    /// <summary>Creating date token.</summary>
    [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; }

    /// <summary>Expiration date token.</summary>
    [BsonElement("expires")]
    public DateTime Expires { get; set; }

    /// <summary>Date that token was edit.</summary>
    [BsonElement("refreshed_at")]
    public DateTime? RefreshedAt { get; set; }

    /// <summary>Indicated is token access was cancel.</summary>
    [BsonElement("canceled")]
    public bool Canceled { get; set; }

    /// <summary>Constructor for instantiating an existing token.</summary>
    /// <param name="iD">Identification code of the token.</param>
    /// <param name="jWTToken">JWT token.</param>
    /// <param name="refreshToken">Refresh token.</param>
    /// <param name="userId">User's identification code.</param>
    /// <param name="iP">IP address used by the user to generate the token.</param>
    /// <param name="refreshedAt">Flag indicating whether the token has been updated.</param>
    /// <param name="createdAt">Token creation date.</param>
    /// <param name="expires">Token expiration date.</param>
    /// <param name="refreshedAt">Date the token was changed.</param>
    /// <param name="canceled">Indicates if the access token has been revoked.</param>
    /// <param name="type">Type of passport.</param>

    public AccessTokenSchema
    (string iD, string jWTToken, string userID, string? iP,
     bool refreshed, DateTime createdAt, DateTime expires, DateTime? refreshedAt, bool canceled)
    {
        ID = iD;        
        JWTToken = jWTToken;        
        UserID = userID;
        IP = iP;
        Refreshed = refreshed;
        CreatedAt = createdAt;
        Expires = expires;
        RefreshedAt = refreshedAt;
        Canceled = canceled;
    }

    ///<summary>Convert entity from development access token to mongo context model.</summary>
    /// <param name="schema">List related access token from development template in mongo context.</param>
    public static implicit operator AccessToken(AccessTokenSchema schema)
    {
        if (schema == null)
            return null!;

        return new AccessToken(schema.ID, schema.JWTToken, schema.UserID, schema.IP, schema.Refreshed, schema.CreatedAt, schema.Expires, schema.RefreshedAt, schema.Canceled);
    }

    /// <summary>Convert mongo context template from development access token group to entity.</summary>
    /// <param name="entity">List related access token from development template.</param>
    public static implicit operator AccessTokenSchema(AccessToken entity)
    {
        if (entity == null)
            return null!;

        return new AccessTokenSchema(entity.ID, entity.JWTToken, entity.UserId, entity.IP, entity.Refreshed, entity.CreatedAt, entity.Expires, entity.RefreshedAt, entity.Canceled);
    }
}