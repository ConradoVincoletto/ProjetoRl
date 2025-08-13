using Domain.Users;
using MongoDB.Driver;
using ProjetoRl.Infra.Data.Repositories.MongoDB.Implementation.Users;
using ProjetoRl.ProjetoRl.Commom;
using ProjetoRl.ProjetoRl.Domain.Users;

namespace ProjetoRl.Data.Repositories.MongoDB.Implementation.Couriers;

/// <summary>Repository implementation of courier in MongoDB.</summary>
public class CourierRepositoryMongoDB : ICourierRepository
{
#nullable enable
    private readonly CourierContext _courierCtx = null!;

    /// <summary>Constructor with parameters to initialize.</summary>
    public CourierRepositoryMongoDB(CourierContext courierContext)
    {
        _courierCtx = courierContext;
    }

    /// <summary>Retrieves a page of courier data from the platform.</summary>
    public async Task<PagedResult<Courier>> ListAsync(string? identifier,
                                                      string? name,
                                                      string? cnpj,
                                                      string? driverLicenseNumber,
                                                      IEnumerable<UserState>? states,
                                                      uint pageIndex = 1,
                                                      uint pageSize = 50)
    {
        var builder = Builders<CourierScheme>.Filter;
        var sortBuilder = Builders<CourierScheme>.Sort.Ascending(c => c.Name).Ascending(c => c.Identifier);

        var filter = builder.Empty;

        if (!string.IsNullOrEmpty(identifier))
            filter &= builder.Regex(c => c.Identifier, identifier);

        if (!string.IsNullOrEmpty(name))
            filter &= builder.Regex(c => c.Name, name);

        if (!string.IsNullOrEmpty(cnpj))
            filter &= builder.Regex(c => c.Cnpj, cnpj);

        if (!string.IsNullOrEmpty(cnpj))
            filter &= builder.Regex(c => c.DriverLicenseNumber, driverLicenseNumber);

        if (states != null && states.Any())
            filter &= builder.In(c => c.State, states);

        var couriers = await _courierCtx.Couriers
            .Aggregate()
            .Match(filter)
            .Sort(sortBuilder)
            .Skip((pageIndex - 1) * (int)pageSize)
            .Limit((int)pageSize)
            .ToListAsync();

        return new PagedResult<Courier>(pageSize, pageIndex, pageIndex, couriers.Select(c => (Courier)c).ToList());
    }

    /// <summary>Gets courier information by ID.</summary>
    public async Task<Courier?> GetByIdAsync(string id)
    {
        var builder = Builders<CourierScheme>.Filter;
        var filter = builder.Eq(c => c.ID, id);

        return await _courierCtx.Couriers
            .Aggregate()
            .Match(filter)
            .FirstOrDefaultAsync();
    }

    /// <summary>Registers a new courier.</summary>
    public async Task<string> CreateAsync(Courier courier)
    {
        var courierSchema = (CourierScheme)courier;
        await _courierCtx.Couriers.InsertOneAsync(courierSchema);
        return courierSchema.ID!;
    }

    /// <summary>Edits a courier's information.</summary>
    public async Task EditAsync(Courier courier)
    {
        var builder = Builders<CourierScheme>.Filter;
        var filter = builder.Eq(c => c.ID, courier.ID);

        await _courierCtx.Couriers.ReplaceOneAsync(filter, courier);
    }

    /// <summary>Deletes (soft delete) a courier.</summary>
    public async Task RemoveAsync(string id)
    {
        var filter = Builders<CourierScheme>.Filter.Eq(c => c.ID, id);

        var update = Builders<CourierScheme>.Update
            .Set(c => c.State, UserState.Removed);

        await _courierCtx.Couriers.UpdateOneAsync(filter, update);
    }

    /// <summary>Activate courier account.</summary>
    public async Task ActivateCourirerAccountAsync(string courierID)
    {
        var filter = Builders<CourierScheme>.Filter.Eq(c => c.ID, courierID);
        var update = Builders<CourierScheme>.Update.Set(c => c.State, UserState.Active);
        await _courierCtx.Couriers.UpdateOneAsync(filter, update);
    }

    /// <summary>Deactivate courier account.</summary>
    public async Task DeactivateCourierAccountAsync(string courierID)
    {
        var filter = Builders<CourierScheme>.Filter.Eq(c => c.ID, courierID);

        var update = Builders<CourierScheme>.Update.Set(c => c.State, UserState.Deactivated);
        await _courierCtx.Couriers.UpdateOneAsync(filter, update);

    }
}

