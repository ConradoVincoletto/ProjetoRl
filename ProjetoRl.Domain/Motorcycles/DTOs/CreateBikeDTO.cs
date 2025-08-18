namespace ProjetoRl.Domain.Motorcycles.DTOs;

/// <summary>
/// // Data Transfer Object for creating a new motorcycle.
/// </summary>
public class CreateBikeDTO
{
    public string Identifier { get; set; } = string.Empty;
    public int Year { get; set; }
    public string Model { get; set; } = string.Empty;
    public string Plate { get; set; } = string.Empty;
}