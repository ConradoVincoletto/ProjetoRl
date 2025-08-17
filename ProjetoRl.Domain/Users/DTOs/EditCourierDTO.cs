using Microsoft.AspNetCore.Http;

namespace ProjetoRl.ProjetoRl.Domain.Users.DTOs;

public class EditCourierDTO
{
    public string Identifier { get; set; } = null!;
    public string Name { get; set; } = null!;    
    public DateTime BirthDate { get; set; }
    public string? DriverLicenseImagePath { get; set; }
    public IFormFile? DriverLicenseImage { get; set; }
}

