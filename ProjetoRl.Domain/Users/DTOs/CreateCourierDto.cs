namespace ProjetoRl.Domain.Users.DTOs;

public class CreateCourierDTO
{
    public string Identifier { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Cnpj { get; set; } = null!;
    public DateTime BirthDate { get; set; }
    public string DriverLicenseNumber { get; set; } = null!;
    public string DriverLicenseType { get; set; } = null!;
    public string DriverLicenseImagePath { get; set; } = null!;
}

public class UpdateCourierDTO : CreateCourierDTO { }
