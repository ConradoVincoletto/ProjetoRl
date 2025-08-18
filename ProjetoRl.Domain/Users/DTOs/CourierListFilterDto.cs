using Domain.Users;

namespace ProjetoRl.ProjetoRl.Domain.Users.DTOs;

/// <summary>
/// Data Transfer Object for filtering courier lists.
/// </summary>
public class CourierListFilterDTO
{
    /// <summary>
    /// Unique identifier for the courier.
    /// </summary>
    public string? Identifier { get; set; }

    /// <summary>
    /// Name of the courier.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// CNPJ (Cadastro Nacional da Pessoa Jurídica) of the courier, if applicable.
    /// </summary>
    public string? Cnpj { get; set; }

    /// <summary>
    /// Driver's license number of the courier, if applicable.
    /// </summary>
    public string? DriverLicenseNumber { get; set; }

    /// <summary>
    /// List of states to filter couriers by their state of operation.
    /// </summary>
    public IEnumerable<UserState>? States { get; set; }

    /// <summary>Current search page.</summary>
    public uint PageIndex { get; set; } = 1;

    /// <summary>Number of records returned per page.</summary>
    public uint PageSize { get; set; } = 50;
}

