
using ProjetoRl.Domain.Motorcycles.DTOs;

namespace ProjetoRl.ProjetoRl.Domain.Motorcycles;

/// <summary>
/// Represents a motorcycle entity.
/// </summary>
public class Bike
{
    /// <summary>
    /// Unique identifier for the motorcycle.
    /// </summary>
    public string? ID { get; set; }

    /// <summary>
    /// Identifier for the motorcycle, such as VIN or similar.
    /// </summary>
    public string Identifier { get; set; } = null!;

    /// <summary>
    /// Year of manufacture of the motorcycle.
    /// </summary>
    public int Year { get; set; }

    /// <summary>
    /// Model of the motorcycle.
    /// </summary>
    public string Model { get; set; } = null!;

    /// <summary>
    /// License plate of the motorcycle.
    /// </summary>
    public string Plate { get; set; } = null!;

    /// <summary>
    /// Default constructor for Bike class.
    /// </summary>
    public Bike() { }

    /// <summary>
    /// Constructor for Bike class with parameters.
    /// </summary> 
    public Bike(string? iD, string identifier, int year, string model, string plate)
    {
        ID = iD;
        Identifier = identifier;
        Year = year;
        Model = model;
        Plate = plate;
    }

    /// <summary>
    /// Constructor for Bike class using CreateBikeDTO.
    /// </summary>
    public Bike(CreateBikeDTO dto)
    {
        Identifier = dto.Identifier;
        Year = dto.Year;
        Model = dto.Model;
        Plate = dto.Plate;
    }

    /// <summary>
    /// Constructor for Bike class using UpdateBikeDTO.
    /// </summary>
    /// <param name="dto"></param>
    public Bike(UpdateBikeDTO dto)
    {
        Plate = dto.Plate;
    }
}
