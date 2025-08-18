using ProjetoRl.ProjetoRl.Domain.Users.DTOs;

namespace ProjetoRl.ProjetoRl.Domain.Users;

/// <summary>
/// Represents a courier in the system.
/// </summary>

public class Courier
{
    /// <summary>
    /// Unique identifier for the courier.
    /// </summary>
    public string? ID { get; set; }

    /// <summary>
    /// Unique identifier for the courier, such as a CPF or CNPJ.
    /// </summary>
    public string Identifier { get; set; } = null!;

    /// <summary>
    /// Name of the courier.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// CNPJ of the courier, if applicable.
    /// </summary>
    public string Cnpj { get; set; } = null!;

    /// <summary>
    /// Date of birth of the courier.
    /// </summary>
    public DateTime BirthDate { get; set; }
    public string DriverLicenseNumber { get; set; } = null!;

    /// <summary>
    /// Type of driver's license held by the courier.
    /// </summary>
    public LicenseType DriverLicenseType { get; set; }

    /// <summary>
    /// Path to the image of the courier's driver's license.
    /// </summary>
    public string DriverLicenseImagePath { get; set; } = null!;

    /// <summary>User account state.</summary>
    public UserState State { get; private set; }

    /// <summary>
    /// Empty constructor.
    /// </summary>
    public Courier() { }

    /// <summary>
    ///  Contrutoctor to initialize.
    /// </summary>
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

    /// <summary>
    /// Constructor to create a courier account.
    /// </summary>
    
    public Courier(CreateCourierDTO dto)
    {
        Identifier = dto.Identifier;
        Name = dto.Name;
        Cnpj = dto.Cnpj;
        BirthDate = dto.BirthDate;
        DriverLicenseNumber = dto.DriverLicenseNumber;
        DriverLicenseType = dto.DriverLicenseType;
        DriverLicenseImagePath = dto.DriverLicenseImagePath!;
        State = UserState.Active; // Default state
    }

    /// <summary>
    /// Constructor to update a existing courier.
    /// </summary>

    public Courier(Courier courier, EditCourierDTO dto)
    {
        Identifier = dto.Identifier;
        Name = dto.Name;
        Cnpj = courier.Cnpj;
        BirthDate = dto.BirthDate;
        DriverLicenseNumber = courier.DriverLicenseNumber;
        DriverLicenseImagePath = dto.DriverLicenseImagePath ?? null!;
        State = UserState.Active; // Default state
    }
}
