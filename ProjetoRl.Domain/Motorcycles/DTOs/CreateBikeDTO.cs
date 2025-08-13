namespace ProjetoRl.Domain.Motorcycles.DTOs;

public class CreateBikeDTO
{
    public string Identifier { get; set; } = string.Empty;
    public int Year { get; set; }
    public string Model { get; set; } = string.Empty;
    public string Plate { get; set; } = string.Empty;
}