using Domain.Rentals;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace ProjetoRl.Infra.Data.Repositories.MongoDB.Implementation.Rentals;

public class RentalScheme
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
#nullable enable
    public string? ID { get; set; }

    [BsonElement("bikeId")]
    public string BikeId { get; set; } = null!;

    [BsonElement("courierId")]
    public string CourierId { get; set; } = null!;

    [BsonElement("planDays")]
    public int PlanDays { get; set; }

    [BsonElement("dailyCost")]
    public decimal DailyCost { get; set; }

    [BsonElement("startDate")]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime StartDate { get; set; }

    [BsonElement("expectedEndDate")]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime ExpectedEndDate { get; set; }

    [BsonElement("actualEndDate")]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime? ActualEndDate { get; set; }

    [BsonElement("totalCost")]
    public decimal? TotalCost { get; set; }

    // Construtor padrão
    public RentalScheme() { }

    // Construtor parametrizado
    public RentalScheme(string? iD, string bikeId, string courierId, int planDays, decimal dailyCost, DateTime startDate, DateTime expectedEndDate)
    {
        ID = iD;
        BikeId = bikeId;
        CourierId = courierId;
        PlanDays = planDays;
        DailyCost = dailyCost;
        StartDate = startDate;
        ExpectedEndDate = expectedEndDate;
    }

    public static implicit operator RentalScheme(Rental entity)
    {
        if (entity == null)
            return null!;

        return new(entity.ID,
                   entity.BikeId,
                   entity.CourierId,
                   entity.PlanDays,
                   entity.DailyCost,
                   entity.StartDate,
                   entity.ExpectedEndDate);
    }

    public static implicit operator Rental(RentalScheme scheme)
    {
        if (scheme == null)
            return null!;

        return new(scheme.ID,
                   scheme.BikeId,
                   scheme.CourierId,
                   scheme.PlanDays,
                   scheme.DailyCost,
                   scheme.StartDate);
    }
}

