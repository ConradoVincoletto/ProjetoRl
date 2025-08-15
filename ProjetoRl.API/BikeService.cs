using Microsoft.AspNetCore.Mvc;
using ProjetoRl.Domain.Motorcycles.DTOs;
using RabbitMQ.Client;
using ProjetoRl.ProjetoRl.Domain.Motorcycles;
using ProjetoRl.ProjetoRl.Domain.Motorcycles.DTOs;
using System.Net;
using System.Text;
using System.Text.Json;
using ProjetoRl.ProjetoRl.Commom;
using Microsoft.AspNetCore.Authorization;
using ProjetoRl.ProjetoRl.Domain.Rentals;

namespace ProjetoRl.ProjetoRl.API;

[ApiController]
[Route("bikes")]
[ApiExplorerSettings(GroupName = "Bikes")]
public class BikeService : ControllerBase
{
    private readonly IBikeRepository _bikeRep;
    /// <summary>Factory of connections with the rabbitMQ.</summary>
    private readonly ConnectionFactory _connectionFactory;

    /// <summary>Connection with RabbitMQ.</summary>
    private readonly IConnection _con;

    /// <summary>Model of RabbitMQ.</summary>
    private readonly IModel _model;

    public BikeService(IBikeRepository bikeRep)
    {
        _bikeRep = bikeRep;
        _connectionFactory = new ConnectionFactory()
        {
            HostName = "localhost",
            UserName = "guest",
            Password = "guest"
        };
        _con = _connectionFactory.CreateConnection();
        _model = _con.CreateModel();
    }

    [HttpGet(Name = "GetAllBikes")]
    [Authorize(Roles = "Administrator")]
    [ProducesResponseType(typeof(IEnumerable<Bike>), (int)HttpStatusCode.OK)]
    public async Task<PagedResult<Bike>> ListBikeAsync([FromQuery] ListBikeDTO dto)
    {
        return await _bikeRep.ListAsync(dto.Identifier,
                                        dto.Year,
                                        dto.Model,
                                        dto.Plate,
                                        dto.PageIndex,
                                        dto.PageSize);
    }

    [HttpGet("{id}", Name = "GetBikeById")]
    [ProducesResponseType(typeof(Bike), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<Bike>> GetBikeByIdAsync([FromRoute] string id)
    {
        var bike = await _bikeRep.GetByIdAsync(id);
        if (bike == null)
            return NotFound();

        return Ok(bike);
    }


    [HttpPost(Name = "CreateBike")]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<Bike>> CreateBikeAsync([FromBody] CreateBikeDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var existingBike = await _bikeRep.GetByPlateAsync(dto.Plate);

        if (existingBike != null)
            return BadRequest("A bike with this plate already exists.");

        var bike = new Bike(dto);
        var bikeId = await _bikeRep.CreateAsync(bike);



        var createdBike = await _bikeRep.GetByIdAsync(bikeId);
        return CreatedAtAction(nameof(GetBikeByIdAsync), new { id = bikeId }, createdBike);
    }

    [HttpPut("{id}", Name = "UpdateBike")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> UpdateBikeAsync([FromRoute] string id, [FromBody] UpdateBikeDTO dto)
    {
        var bike = await _bikeRep.GetByIdAsync(id);
        if (bike == null)
            return NotFound();

        var editBike = new Bike(dto);
        await _bikeRep.EditAsync(editBike);

        return Ok();
    }

    [HttpPatch("{id}/deactivate", Name = "DeactivateBike")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> DeactivateBikeAsync([FromRoute] string id, [FromServices] IRentalRepository rentalRep)
    {
        var bike = await _bikeRep.GetByIdAsync(id);
        if (bike == null)
            return NotFound();
        
        var activeRental = await rentalRep.GetRentalByBikeIdAsync(bike.ID!);

        if(activeRental != null)
        {
            ModelState.AddModelError("Bike", "Cannot deactivate a bike that is currently rented.");
            return BadRequest(ModelState);
        }

        var deactiveBike = _bikeRep.RemoveAsync(id);
        return Ok(deactiveBike);
    }

    private void PublishBikeRegisteredEvent(Bike bike)
    {
        using var channel = _con.CreateModel();

        // Cria um exchange tipo Fanout
        channel.ExchangeDeclare(exchange: "bike.registered", type: ExchangeType.Fanout, durable: true);

        var message = JsonSerializer.Serialize(bike);
        var body = Encoding.UTF8.GetBytes(message);

        channel.BasicPublish(
            exchange: "bike.registered",
            routingKey: "",
            basicProperties: null,
            body: body
        );

        Console.WriteLine($"[RabbitMQ] Evento publicado: {message}");
    }

}
