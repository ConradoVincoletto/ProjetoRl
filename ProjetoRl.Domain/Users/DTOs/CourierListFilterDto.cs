using Domain.Users;

namespace ProjetoRl.ProjetoRl.Domain.Users.DTOs;

public class CourierListFilterDTO
{
    public string? Identifier { get; set; }
    public string? Name { get; set; }
    public string? Cnpj { get; set; }
    public string? DriverLicenseNumber { get; set; }
    public IEnumerable<UserState>? States { get; set; }

    /// <summary>Current search page.</summary>
    public uint PageIndex { get; set; } = 1;

    /// <summary>Number of records returned per page.</summary>
    public uint PageSize { get; set; } = 50;
}

