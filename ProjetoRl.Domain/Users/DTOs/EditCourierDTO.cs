using Microsoft.AspNetCore.Http;

namespace ProjetoRl.ProjetoRl.Domain.Users.DTOs;

/// <summary>
/// Data Transfer Object for editing an existing courier.
/// </summary>
public class EditCourierDTO
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
    public DateTime BirthDate { get; set; }

    /// <summary>
    /// Driver's license number of the courier.
    /// </summary>
    public string? DriverLicenseImagePath { get; set; }

    /// <summary>
    /// Image file for the driver's license of the courier.
    /// </summary>
    public IFormFile? DriverLicenseImage { get; set; }
}

