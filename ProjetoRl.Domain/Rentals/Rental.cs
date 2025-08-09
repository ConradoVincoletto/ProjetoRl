using ProjetoRl.Domain.Rentals.DTOs;

namespace Domain.Rentals;

public class Rental
{
    public string? ID { get; set; }
    public string BikeId { get; set; } = null!;
    public string CourierId { get; set; } = null!;
    public int PlanDays { get; set; }
    public decimal DailyCost { get; set; } 
    public DateTime StartDate { get; set; } 
    public DateTime ExpectedEndDate { get; set; }
    public DateTime? ActualEndDate { get; set; }
    public decimal? TotalCost { get; set; }

    public Rental() { }
    public Rental(string iD, string bikeId, string courierId, int planDays, decimal dailyCost, DateTime startDate, DateTime expectedEndDate)
    {
        ID = iD;
        BikeId = bikeId;
        CourierId = courierId;
        PlanDays = planDays;
        DailyCost = dailyCost;
        StartDate = startDate;
        ExpectedEndDate = expectedEndDate;
        ActualEndDate = null;
        TotalCost = null;
    }

    public Rental(CreateRentalDTO dto)
    {
        BikeId = dto.BikeId;
        CourierId = dto.CourierId;
        PlanDays = dto.PlanDays;
        DailyCost = dto.DailyCost;
        StartDate = dto.StartDate;
        ExpectedEndDate = dto.StartDate.AddDays(dto.PlanDays);
        ActualEndDate = null;
        TotalCost = null;
    }
}
