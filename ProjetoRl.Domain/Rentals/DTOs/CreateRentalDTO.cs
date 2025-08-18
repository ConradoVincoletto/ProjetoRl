namespace ProjetoRl.Domain.Rentals.DTOs;

/// <summary>
/// Data Transfer Object for creating a new rental. 
/// </summary>
public class CreateRentalDTO
{
    public string BikeId { get; set; } = string.Empty;
    public string CourierId { get; set; } = string.Empty;
    public int PlanDays { get; set; }
    public decimal DailyCost { get; set; }
    public DateTime StartDate { get; set; }
}
/// <summary>
/// Data Transfer Object for updating an existing rental.
/// </summary>
public class UpdateRentalDTO : CreateRentalDTO { }

/// <summary>
/// Data Transfer Object for finalizing a rental.
/// </summary>
public class FinalizeRentalDTO
{
    /// <summary>
    /// Unique identifier for the rental.
    /// </summary>
    public DateTime ActualEndDate { get; set; }
}
