using MongoDB.Driver;
using ProjetoRl.Infra.Data.Repositories.MongoDB.Implementation.Motorcycles;
using ProjetoRl.Infra.Data.Repositories.MongoDB.Implementation.Users;
using ProjetoRl.ProjetoRl.Commom;
using ProjetoRl.ProjetoRl.Domain.Motorcycles;

namespace ProjetoRl.Data.Repositories.MongoDB.Implementation.Bikes;

/// <summary>Repository implementation of bike in MongoDB.</summary>
public class BikeRepositoryMongoDB : IBikeRepository
{
    #nullable enable
    private readonly BikeContext _bikeCtx = null!;

    /// <summary>Constructor with parameters to initialize.</summary>
    public BikeRepositoryMongoDB(BikeContext bikeContext)
    {
        _bikeCtx = bikeContext;
    }

    /// <summary>Retrieves a page of bike data from the platform.</summary>
    public async Task<PagedResult<Bike>> ListAsync(string? identifier,
                                                   int? year,
                                                   string? model,
                                                   string? plate,
                                                   uint pageIndex = 1,
                                                   uint pageSize = 50)
    {
        var builder = Builders<BikeScheme>.Filter;
        var sortBuilder = Builders<BikeScheme>.Sort.Ascending(b => b.Model).Ascending(b => b.Identifier);

        var filter = builder.Empty;

        if (!string.IsNullOrEmpty(identifier))
            filter &= builder.Regex(b => b.Identifier, identifier);

        if (year.HasValue)
            filter &= builder.Eq(b => b.Year, year.Value);

        if (!string.IsNullOrEmpty(model))
            filter &= builder.Regex(b => b.Model, model);

        if (!string.IsNullOrEmpty(plate))
            filter &= builder.Regex(b => b.Plate, plate);

        var bikes = await _bikeCtx.Bikes
            .Aggregate()
            .Match(filter)
            .Sort(sortBuilder)
            .Skip((pageIndex - 1) * (int)pageSize)
            .Limit((int)pageSize)
            .ToListAsync();

        return new PagedResult<Bike>(
            pageSize,
            pageIndex,
            pageIndex,
            bikes.Select(b => (Bike)b).ToList()
        );
    }

    /// <summary>Gets bike information by ID.</summary>
    public async Task<Bike?> GetByIdAsync(string id)
    {
        var builder = Builders<BikeScheme>.Filter;
        var filter = builder.Eq(b => b.ID, id);

        return await _bikeCtx.Bikes
            .Aggregate()
            .Match(filter)
            .FirstOrDefaultAsync();
    }

    public async Task<Bike?> GetByPlateAsync(string plate)
    {
        var builder = Builders<BikeScheme>.Filter;
        var filter = builder.Eq(b => b.Plate, plate);

        return await _bikeCtx.Bikes
            .Aggregate()
            .Match(filter)
            .FirstOrDefaultAsync();
    }

    /// <summary>Registers a new bike.</summary>
    public async Task<string> CreateAsync(Bike bike)
    {
        var bikeSchema = (BikeScheme)bike;
        await _bikeCtx.Bikes.InsertOneAsync(bikeSchema);
        return bikeSchema.ID;
    }

    /// <summary>Edits a bike's information.</summary>
    public async Task EditAsync(Bike bike)
    {
        var builder = Builders<BikeScheme>.Filter;
        var filter = builder.Eq(b => b.ID, bike.ID);

        await _bikeCtx.Bikes.ReplaceOneAsync(filter, bike);
    }

    /// <summary>Deletes a bike record.</summary>
    public async Task RemoveAsync(string id)
    {
        var filter = Builders<BikeScheme>.Filter.Eq(b => b.ID, id);
        await _bikeCtx.Bikes.DeleteOneAsync(filter);
    }
}
