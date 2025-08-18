namespace ProjetoRl.Domain.Motorcycles.DTOs;


/// <summary>
/// Data Transfer Object for updating a motorcycle's plate.
/// </summary>
public class UpdateBikeDTO
{
    public string Plate { get; set; } = string.Empty;
}
