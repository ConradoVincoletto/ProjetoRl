namespace ProjetoRl.ProjetoRl.Domain.Motorcycles;

/// <summary>
/// Represents an event that a motorcycle has been registered.
/// </summary>
public class BikeRegisteredEvent
{
    /// <summary>
    /// Unique identifier for the motorcycle.
    /// </summary>
    public string BikeId { get; }

    /// <summary>
    /// Identifier for the motorcycle, such as VIN or similar.
    /// </summary>
    public string Identifier { get; }

    /// <summary>
    /// License plate of the motorcycle.
    /// </summary>
    public string Plate { get; }

    /// <summary>
    ///  Constructor for BikeRegisteredEvent.
    /// </summary>
    public BikeRegisteredEvent(string bikeId, string identifier, string plate)
    {
        BikeId = bikeId;
        Identifier = identifier;
        Plate = plate;
    }
}