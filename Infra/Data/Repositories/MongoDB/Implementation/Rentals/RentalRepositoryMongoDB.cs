using Domain.Rentals;
using MongoDB.Driver;
using ProjetoRl.Infra.Data.Repositories.MongoDB.Implementation.Rentals;
using ProjetoRl.Infra.Data.Repositories.MongoDB.Implementation.Users;
using ProjetoRl.ProjetoRl.Commom;
using ProjetoRl.ProjetoRl.Domain.Rentals;

namespace ProjetoRl.Data.Repositories.MongoDB.Implementation.Rentals;

/// <summary>Repository implementation of rental in MongoDB.</summary>
public class RentalRepositoryMongoDB : IRentalRepository
{
#nullable enable
    private readonly RentalContext _rentalCtx = null!;

    /// <summary>Constructor with parameters to initialize.</summary>
    public RentalRepositoryMongoDB(RentalContext rentalContext)
    {
        _rentalCtx = rentalContext;
    }

    /// <summary>Retrieves a page of rental data from the platform.</summary>
    public async Task<PagedResult<Rental>> ListAsync(string? bikeId,
                                                     string? courierId,
                                                     DateTime? startDateFrom,
                                                     DateTime? startDateTo,
                                                     uint pageIndex = 1,
                                                     uint pageSize = 50)
    {
        var builder = Builders<RentalScheme>.Filter;
        var sortBuilder = Builders<RentalScheme>.Sort.Descending(r => r.StartDate);

        var filter = builder.Empty;

        if (!string.IsNullOrEmpty(bikeId))
            filter &= builder.Eq(r => r.BikeId, bikeId);

        if (!string.IsNullOrEmpty(courierId))
            filter &= builder.Eq(r => r.CourierId, courierId);

        if (startDateFrom.HasValue)
            filter &= builder.Gte(r => r.StartDate, startDateFrom.Value);

        if (startDateTo.HasValue)
            filter &= builder.Lte(r => r.StartDate, startDateTo.Value);

        var rentals = await _rentalCtx.Rentals
            .Aggregate()
            .Match(filter)
            .Sort(sortBuilder)
            .Skip((pageIndex - 1) * (int)pageSize)
            .Limit((int)pageSize)
            .ToListAsync();

        return new PagedResult<Rental>(
            pageSize,
            pageIndex,
            pageIndex,
            rentals.Select(r => (Rental)r).ToList()
        );
    }

    /// <summary>Gets rental information by ID.</summary>
    public async Task<Rental?> GetByIdAsync(string id)
    {
        var builder = Builders<RentalScheme>.Filter;
        var filter = builder.Eq(r => r.ID, id);

        return await _rentalCtx.Rentals
            .Aggregate()
            .Match(filter)
            .FirstOrDefaultAsync();
    }

    public async Task<Rental?> GetRentalByBikeIdAsync(string id)
    {
        var builder = Builders<RentalScheme>.Filter;
        var filter = builder.Eq(r => r.BikeId, id);

        return await _rentalCtx.Rentals
            .Aggregate()
            .Match(filter)
            .FirstOrDefaultAsync();
    }

    /// <summary>Registers a new rental.</summary>
    public async Task<string> CreateAsync(Rental rental)
    {
        var rentalSchema = (RentalScheme)rental;
        await _rentalCtx.Rentals.InsertOneAsync(rentalSchema);
        return rentalSchema.ID!;
    }

    /// <summary>Edits a rental's information.</summary>
    public async Task EditAsync(Rental rental)
    {
        if (string.IsNullOrEmpty(rental.ID))
            throw new ArgumentException("ID do aluguel é obrigatório para atualizar.");

        var filter = Builders<RentalScheme>.Filter.Eq(r => r.ID, rental.ID);

        var update = Builders<RentalScheme>.Update
            .Set(r => r.BikeId, rental.BikeId)
            .Set(r => r.CourierId, rental.CourierId)
            .Set(r => r.PlanDays, rental.PlanDays)
            .Set(r => r.DailyCost, rental.DailyCost)
            .Set(r => r.StartDate, rental.StartDate)
            .Set(r => r.ExpectedEndDate, rental.ExpectedEndDate)
            .Set(r => r.ActualEndDate, rental.ActualEndDate)
            .Set(r => r.TotalCost, rental.TotalCost);

        var result = await _rentalCtx.Rentals.UpdateOneAsync(filter, update);

        if (result.MatchedCount == 0)
            throw new KeyNotFoundException($"Aluguel com ID {rental.ID} não encontrado para atualização.");
    }


    public async Task FinalizeRentalAsync(string rentalId, DateTime actualEndDate, decimal totalCost)
    {
        var filter = Builders<RentalScheme>.Filter.Eq(r => r.ID, rentalId);
        var update = Builders<RentalScheme>.Update
            .Set(r => r.ActualEndDate, actualEndDate)
            .Set(r => r.TotalCost, totalCost);

        await _rentalCtx.Rentals.UpdateOneAsync(filter, update);
    }



    /// <summary>Deletes a rental record.</summary>
    public async Task RemoveAsync(string id)
    {
        var filter = Builders<RentalScheme>.Filter.Eq(r => r.ID, id);
        await _rentalCtx.Rentals.DeleteOneAsync(filter);
    }
}
