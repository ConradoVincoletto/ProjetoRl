namespace ProjetoRl.Domain.Rentals.DTOs;

public class CreateRentalDTO
{
    public string BikeId { get; set; } = string.Empty;
    public string CourierId { get; set; } = string.Empty;
    public int PlanDays { get; set; }
    public decimal DailyCost { get; set; }
    public DateTime StartDate { get; set; }
}

public class UpdateRentalDTO : CreateRentalDTO { }

public class FinalizeRentalDTO
{
    public DateTime ActualEndDate { get; set; }
}
