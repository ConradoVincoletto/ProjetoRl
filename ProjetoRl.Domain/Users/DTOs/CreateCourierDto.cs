using Microsoft.AspNetCore.Http;

namespace ProjetoRl.ProjetoRl.Domain.Users.DTOs;

/// <summary>
/// Data Transfer Object for creating a new courier.
/// </summary>
public class CreateCourierDTO
{
    /// <summary>
    /// Unique identifier for the courier.
    /// </summary>
    public string Identifier { get; set; } = null!;

    /// <summary>
    /// Name of the courier.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Email address of the courier.
    /// </summary>
    public string Cnpj { get; set; } = null!;

    /// <summary>
    /// Birth date of the courier.
    /// </summary>
    public DateTime BirthDate { get; set; }

    /// <summary>
    /// Driver's license number of the courier.
    /// </summary>
    public string DriverLicenseNumber { get; set; } = null!;

    /// <summary>
    /// Type of driver's license held by the courier.
    /// </summary>
    public LicenseType DriverLicenseType { get; set; }

    /// <summary>
    /// Image path for the driver's license of the courier.
    /// </summary>
    public string? DriverLicenseImagePath { get; set; }

    /// <summary>
    /// Image file for the driver's license of the courier.
    /// </summary>
    public IFormFile? DriverLicenseImage { get; set; }
}


