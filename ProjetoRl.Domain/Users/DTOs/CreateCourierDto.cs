using Microsoft.AspNetCore.Http;

namespace ProjetoRl.ProjetoRl.Domain.Users.DTOs;

public class CreateCourierDTO
{
    public string Identifier { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Cnpj { get; set; } = null!;
    public DateTime BirthDate { get; set; }
    public string DriverLicenseNumber { get; set; } = null!;
    public LicenseType DriverLicenseType { get; set; }
    public string? DriverLicenseImagePath { get; set; }
    public IFormFile? DriverLicenseImage { get; set; }
}


