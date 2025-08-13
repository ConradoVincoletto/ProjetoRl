using Domain.Users;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ProjetoRl.ProjetoRl.Domain.Users;

namespace ProjetoRl.Infra.Data.Repositories.MongoDB.Implementation.Users;

/// <summary>Dto represents a user in database MongoDB.</summary>
public class UserSchema
{
#nullable enable
    /// <summary>User identification code.</summary>
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? ID { get; set; }

    /// <summary>User first name.</summary>
    [BsonElement("first_name")]
    public string FirstName { get; set; }

    /// <summary>User last name.</summary>
    [BsonElement("last_name")]
    public string LastName { get; set; }

    /// <summary>User email address.</summary>
    [BsonElement("email")]
    public string Email { get; set; }

    /// <summary>User cell phone.</summary>
    [BsonElement("cell_phone")]
#nullable enable
    public string? Cellphone { get; set; } = null!;

    /// <summary>Roles linked for user.</summary>
    [BsonElement("roles")]
    public IEnumerable<Role> Roles { get; set; }

    /// <summary>User state situation.</summary>
    [BsonElement("state")]
    public UserState State { get; set; }

    /// <summary>Constructor with parameters to initialization.</summary>
    /// <param name="iD">Identification permission.</param>
    /// <param name="firstName">User first name.</param>
    /// <param name="lastName">User last name.</param>
    /// <param name="email">User email address.</param>
    /// <param name="cellphone">User cell phone.</param>
    /// <param name="roles">Roles linked for user.</param>
    /// <param name="state">User state situation.</param>
    public UserSchema(string? iD, string firstName, string lastName, string email, string? cellphone, IEnumerable<Role> roles, UserState state)
    {
        ID = iD;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Cellphone = cellphone;
        Roles = roles;
        State = state;
    }

    ///<summary>Convert entity from development user to mongo context model.</summary>
    /// <param name="scheme">List related user from development template in mongo context.</param>
    public static implicit operator User(UserSchema scheme)
    {
        if (scheme == null)
            return null!;

        return new(scheme.ID,
                   scheme.FirstName,
                   scheme.LastName,
                   scheme.Email,
                   scheme.Cellphone,
                   scheme.Roles,
                   scheme.State);
    }

    ///<summary>Convert entity from development user to mongo context model.</summary>
    /// <param name="entity">List related user from development template in mongo context.</param>
    public static implicit operator UserSchema(User entity)
    {
        if (entity == null)
            return null!;

        return new(entity.ID,
                   entity.FirstName,
                   entity.LastName,
                   entity.Email,
                   entity.Cellphone,
                   entity.Roles,
                   entity.State);
    }
}