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

    public Rental(CreateRentalDTO dto)
        : this(null, dto.BikeId, dto.CourierId, dto.PlanDays, dto.DailyCost,
               dto.StartDate, null, null)
    { }

    public Rental(UpdateRentalDTO dto)
        : this(null, dto.BikeId, dto.CourierId, dto.PlanDays, dto.DailyCost,
               dto.StartDate, null, null)
    { }

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


