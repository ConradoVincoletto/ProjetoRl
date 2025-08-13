using ProjetoRl.ProjetoRl.Domain.Users.DTOs;

namespace Domain.Users;

public class Courier
{
    public string? ID { get; set; }
    public string Identifier { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Cnpj { get; set; } = null!;
    public DateTime BirthDate { get; set; }
    public string DriverLicenseNumber { get; set; } = null!;
    public LicenseType DriverLicenseType { get; set; }
    public string DriverLicenseImagePath { get; set; } = null!;

    /// <summary>User account state.</summary>
    public UserState State { get; private set; }

    public Courier() { }

    public Courier(string? iD, string identifier, string name, string cnpj, DateTime birthDate, string driverLicenseNumber, LicenseType driverLicenseType, string driverLicenseImagePath, UserState state)
    {
        ID = iD;
        Identifier = identifier;
        Name = name;
        Cnpj = cnpj;
        BirthDate = birthDate;
        DriverLicenseNumber = driverLicenseNumber;
        DriverLicenseType = driverLicenseType;
        DriverLicenseImagePath = driverLicenseImagePath;
        State = state;
    }

    public Courier(CreateCourierDTO dto)
    {
        Identifier = dto.Identifier;
        Name = dto.Name;
        Cnpj = dto.Cnpj;
        BirthDate = dto.BirthDate;
        DriverLicenseNumber = dto.DriverLicenseNumber;
        DriverLicenseType = dto.DriverLicenseType;
        DriverLicenseImagePath = dto.DriverLicenseImagePath;
        State = UserState.Active; // Default state
    }

      public Courier(Courier courier, EditCourierDTO dto)
    {
        Identifier = dto.Identifier;
        Name = dto.Name;
        Cnpj = courier.Cnpj; 
        BirthDate = dto.BirthDate;
        DriverLicenseNumber = courier.DriverLicenseNumber;
        DriverLicenseType = dto.DriverLicenseType;
        DriverLicenseImagePath = dto.DriverLicenseImagePath;
        State = UserState.Active; // Default state
    }
}
