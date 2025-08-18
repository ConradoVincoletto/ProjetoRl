using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ProjetoRl.ProjetoRl.Domain.Users;

namespace ProjetoRl.Infra.Data.Repositories.MongoDB.Implementation.Users;

/// <summary>
/// Represents a courier scheme in the MongoDB database.
/// </summary>
public class CourierScheme
{

    /// <summary>
    /// Unique identifier for the courier scheme.
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
#nullable enable
    public string? ID { get; set; }

    /// <summary>
    /// Identifier for the courier.
    /// </summary>
    [BsonElement("identifier")]
    public string Identifier { get; set; } = null!;

    /// <summary>
    /// Name of the courier.
    /// </summary>
    [BsonElement("name")]
    public string Name { get; set; } = null!;

    /// <summary>
    /// CNPJ of the courier.
    /// </summary>

    [BsonElement("cnpj")]
    public string Cnpj { get; set; } = null!;

    /// <summary>
    /// Birth date of the courier.
    /// </summary>

    [BsonElement("birth_date")]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime BirthDate { get; set; }

    /// <summary>
    /// Driver's license number of the courier.
    /// </summary>

    [BsonElement("driver_license_number")]
    public string DriverLicenseNumber { get; set; } = null!;

    /// <summary>
    /// Type of driver's license of the courier.
    /// </summary>

    [BsonElement("driver_license_type")]
    public LicenseType DriverLicenseType { get; set; }

    /// <summary>
    /// Path to the driver's license image of the courier.
    /// </summary>

    [BsonElement("driver_license_image_path")]
    public string DriverLicenseImagePath { get; set; } = null!;

    /// <summary>
    /// State of the courier (active, inactive, etc.).
    /// </summary>

    [BsonElement("state")]
    public UserState State { get; private set; }

    /// <summary>
    /// Default constructor for the CourierScheme class.
    /// </summary>
    public CourierScheme() { }

    /// <summary>
    /// Constructor to initialize a CourierScheme with specific parameters.
    /// </summary> 
    public CourierScheme(string? iD,
                         string identifier,
                         string name,
                         string cnpj,
                         DateTime birthDate,
                         string driverLicenseNumber,
                         LicenseType driverLicenseType,
                         string driverLicenseImagePath,
                         UserState state)
    {
        ID = iD;
        Identifier = identifier;
        Name = name;
        Cnpj = cnpj;
        BirthDate = birthDate;
        DriverLicenseNumber = driverLicenseNumber;
        DriverLicenseType = driverLicenseType;
        DriverLicenseImagePath = driverLicenseImagePath;
        State = state;
    }

    /// <summary>
    /// Implicit conversion from CourierScheme to Courier.
    /// </summary>
    /// <param name="scheme">Courier scheme class.</param>

    public static implicit operator Courier(CourierScheme scheme)
    {
        if (scheme == null)
            return null!;

        return new(scheme.ID,
                   scheme.Identifier,
                   scheme.Name,
                   scheme.Cnpj,
                   scheme.BirthDate,
                   scheme.DriverLicenseNumber,
                   scheme.DriverLicenseType,
                   scheme.DriverLicenseImagePath,
                   scheme.State);
    }

    /// <summary>
    /// Implicit conversion from Courier to CourierScheme.
    /// </summary>
    /// <param name="entity">Courier entitie class.</param>

    public static implicit operator CourierScheme(Courier entity)
    {
        if (entity == null)
            return null!;

        return new(entity.ID,
                   entity.Identifier,
                   entity.Name,
                   entity.Cnpj,
                   entity.BirthDate,
                   entity.DriverLicenseNumber,
                   entity.DriverLicenseType,
                   entity.DriverLicenseImagePath,
                   entity.State);
    }
}

