namespace ProjetoRl.ProjetoRl.API;

/// <summary>
/// Settings for RabbitMQ connection.
/// </summary>
public class RabbitMqSettings
{
    /// <summary>Hostname of the RabbitMQ server.</summary>
    public string HostName { get; set; } = null!;
    
    /// <summary>Port of the RabbitMQ server.</summary>
    public int Port { get; set; }

    /// <summary>Username for RabbitMQ authentication.</summary>
    public string UserName { get; set; } = null!;

    /// <summary>Password for RabbitMQ authentication.</summary>
    public string Password { get; set; } = null!;
}
