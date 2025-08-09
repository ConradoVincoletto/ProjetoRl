
using ProjetoRl.Domain.Motorcycles.DTOs;

namespace ProjetoRl.ProjetoRl.Domain.Motorcycles;

public class Bike
{
    public string ID { get; set; } = string.Empty;
    public string Identifier { get; set; } = string.Empty;
    public int Year { get; set; }
    public string Model { get; set; } = string.Empty;
    public string Plate { get; set; } = string.Empty;

    public Bike() { }

    public Bike(string iD, string identifier, int year, string model, string plate)
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
}
