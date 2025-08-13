
using ProjetoRl.Domain.Motorcycles.DTOs;

namespace ProjetoRl.ProjetoRl.Domain.Motorcycles;

public class Bike
{
    public string? ID { get; set; }
    public string Identifier { get; set; } = null!;
    public int Year { get; set; }
    public string Model { get; set; } = null!;
    public string Plate { get; set; } = null!;

    public Bike() { }

    public Bike(string? iD, string identifier, int year, string model, string plate)
    {
        ID = iD;
        Identifier = identifier;
        Year = year;
        Model = model;
        Plate = plate;
    }

    public Bike(CreateBikeDTO dto)
    {
        Identifier = dto.Identifier;
        Year = dto.Year;
        Model = dto.Model;
        Plate = dto.Plate;
    }

    public Bike(UpdateBikeDTO dto)
    {
        Plate = dto.Plate;
    }
}
