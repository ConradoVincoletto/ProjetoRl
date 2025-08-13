using Domain.Users;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProjetoRl.Infra.Data.Repositories.MongoDB.Implementation.Users;
public class CourierScheme
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
#nullable enable
    public string? ID { get; set; }

    [BsonElement("identifier")]
    public string Identifier { get; set; } = null!;

    [BsonElement("name")]
    public string Name { get; set; } = null!;

    [BsonElement("cnpj")]
    public string Cnpj { get; set; } = null!;

    [BsonElement("birth_date")]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime BirthDate { get; set; }

    [BsonElement("driver_license_number")]
    public string DriverLicenseNumber { get; set; } = null!;

    [BsonElement("driver_license_type")]
    public LicenseType DriverLicenseType { get; set; }

    [BsonElement("driver_license_image_path")]
    public string DriverLicenseImagePath { get; set; } = null!;

    [BsonElement("state")]
    public UserState State { get; private set; }

    public CourierScheme() { }
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

