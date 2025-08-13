
namespace ProjetoRl.ProjetoRl.Domain.Motorcycles.DTOs;

public class ListBikeDTO
{
    public string? Identifier { get; set; }
    public int? Year { get; set; }
    public string? Model { get; set; }
    public string? Plate { get; set; }
    public uint PageIndex { get; set; } = 1;
    public uint PageSize { get; set; } = 50;
}