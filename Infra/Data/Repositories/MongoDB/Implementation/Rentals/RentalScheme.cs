using Domain.Rentals;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProjetoRl.Infra.Data.Repositories.MongoDB.Implementation.Rentals;

/// <summary>
/// Represents a rental scheme in the MongoDB database.
/// </summary>
public class RentalScheme
{
    /// <summary>
    /// Unique identifier for the rental scheme.
    /// </summary>
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
#nullable enable
    public string? ID { get; set; }

    /// <summary>
    /// Identifier for the bike associated with the rental scheme.
    /// </summary>
    [BsonElement("bikeId")]
    public string BikeId { get; set; } = null!;

    /// <summary>
    /// Identifier for the courier associated with the rental scheme.
    /// </summary>
    [BsonElement("courierId")]
    public string CourierId { get; set; } = null!;

    /// <summary>
    /// Number of days included in the rental plan.
    /// </summary>
    [BsonElement("planDays")]
    public int PlanDays { get; set; }

    /// <summary>
    /// Cost per day for the rental scheme.
    /// </summary>
    [BsonElement("dailyCost")]
    public decimal DailyCost { get; set; }

    /// <summary>
    /// Start date of the rental scheme.
    /// </summary>
    [BsonElement("startDate")]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime StartDate { get; set; }

    /// <summary>
    /// Expected end date of the rental scheme. 
    /// </summary>
    [BsonElement("expectedEndDate")]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime ExpectedEndDate { get; set; }

    /// <summary>
    /// Actual end date of the rental scheme, if completed.
    /// </summary>
    [BsonElement("actualEndDate")]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime? ActualEndDate { get; set; }

    /// <summary>
    /// Total cost of the rental scheme, calculated based on the plan days and daily cost.
    /// </summary>
    [BsonElement("totalCost")]
    public decimal? TotalCost { get; set; }

    /// <summary>
    /// Default constructor for the RentalScheme class.
    /// </summary>
    public RentalScheme() { }

    /// <summary>
    /// Constructor to initialize a RentalScheme with specific parameters.
    /// </summary>
    public RentalScheme(string? iD,
                        string bikeId,
                        string courierId,
                        int planDays,
                        decimal dailyCost,
                        DateTime startDate,
                        DateTime expectedEndDate,
                        DateTime? actualEndDate,
                        decimal? totalCost)
    {
        ID = iD;
        BikeId = bikeId;
        CourierId = courierId;
        PlanDays = planDays;
        DailyCost = dailyCost;
        StartDate = startDate;
        ExpectedEndDate = expectedEndDate;
        ActualEndDate = actualEndDate;
        TotalCost = totalCost;
    }
    /// <summary>
    /// Implicit conversion from Rental to RentalScheme.
    /// </summary>
    /// <param name="entity">Rental entitie class.</param>
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
                   entity.ExpectedEndDate,
                   entity.ActualEndDate,
                   entity.TotalCost);
    }

    /// <summary>
    /// Implicit conversion from RentalScheme to Rental.
    /// </summary>
    /// <param name="scheme">Rental scheme class.</param>
    public static implicit operator Rental(RentalScheme scheme)
    {
        if (scheme == null)
            return null!;

        return new(scheme.ID,
                   scheme.BikeId,
                   scheme.CourierId,
                   scheme.PlanDays,
                   scheme.DailyCost,
                   scheme.StartDate,
                   scheme.ActualEndDate,
                   scheme.TotalCost);
    }
}

