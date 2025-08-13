namespace ProjetoRl.ProjetoRl.Domain.Motorcycles;

public class BikeRegisteredEvent
{
    public string BikeId { get; }
    public string Identifier { get; }
    public string Plate { get; }

    public BikeRegisteredEvent(string bikeId, string identifier, string plate)
    {
        BikeId = bikeId;
        Identifier = identifier;
        Plate = plate;
    }
}