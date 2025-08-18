using ProjetoRl.Domain.Rentals.DTOs;

namespace Domain.Rentals;

/// <summary>
/// Represents a rental transaction for a motorcycle.
/// </summary>
public class Rental
{
    /// <summary>
    /// Unique identifier for the rental.
    /// </summary>
    public string? ID { get; set; }

    /// <summary>
    /// Unique identifier for the motorcycle being rented.
    /// </summary>
    public string BikeId { get; set; } = null!;

    /// <summary>
    /// Unique identifier for the courier renting the motorcycle.
    /// </summary>
    public string CourierId { get; set; } = null!;

    /// <summary>
    /// Number of days included in the rental plan.
    /// </summary>
    public int PlanDays { get; set; }

    /// <summary>
    /// Daily cost of the rental.
    /// </summary>
    public decimal DailyCost { get; set; }

    /// <summary>
    /// Start date of the rental.
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// Expected end date of the rental based on the plan days.
    /// </summary>
    public DateTime ExpectedEndDate { get; set; }

    /// <summary>
    /// Actual end date of the rental, if finalized.
    /// </summary>
    public DateTime? ActualEndDate { get; set; }

    /// <summary>
    /// Total cost of the rental, calculated based on the plan days and any penalties or discounts.
    /// </summary>
    public decimal? TotalCost { get; set; }

    /// <summary>
    /// Default constructor for Rental class.
    /// </summary>
    public Rental() { }

    /// <summary>
    /// Constructor for Rental class with parameters.
    /// </summary>
    public Rental(string? id,
                  string bikeId,
                  string courierId,
                  int planDays,
                  decimal dailyCost,
                  DateTime startDate,
                  DateTime? actualEndDate,
                  decimal? totalCost)
    {
        ID = id;
        BikeId = bikeId;
        CourierId = courierId;
        PlanDays = planDays;
        DailyCost = dailyCost;
        StartDate = startDate;
        ExpectedEndDate = startDate.AddDays(planDays);
        ActualEndDate = actualEndDate;
        TotalCost = totalCost;
    }

    /// <summary>
    /// Constructor for Rental class using CreateRentalDTO.
    public Rental(CreateRentalDTO dto)
        : this(null, dto.BikeId, dto.CourierId, dto.PlanDays, dto.DailyCost,
               dto.StartDate, null, null)
    { }

    /// <summary>
    /// Constructor for Rental class using UpdateRentalDTO.
    public Rental(UpdateRentalDTO dto)
        : this(null, dto.BikeId, dto.CourierId, dto.PlanDays, dto.DailyCost,
               dto.StartDate, null, null)
    { }

    /// <summary>
    /// Finalizes the rental by setting the actual end date and calculating the total cost based on the rental plan and any penalties or discounts.
    /// </summary>
    public void FinalizeRental(DateTime actualEndDate)
    {
        ActualEndDate = actualEndDate;

        // Calcula valor base
        TotalCost = DailyCost * PlanDays;

        if (actualEndDate < ExpectedEndDate)
        {
            // Devolução antecipada → multa
            var unusedDays = (ExpectedEndDate - actualEndDate).Days;
            decimal multaPercent = PlanDays switch
            {
                7 => 0.20m,
                15 => 0.40m,
                _ => 0m
            };
            var multa = (DailyCost * unusedDays) * multaPercent;
            TotalCost = (DailyCost * (PlanDays - unusedDays)) + multa;
        }
        else if (actualEndDate > ExpectedEndDate)
        {
            // Devolução atrasada → R$50 por diária extra
            var extraDays = (actualEndDate - ExpectedEndDate).Days;
            TotalCost += extraDays * 50m;
        }
    }

}


