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
using Microsoft.Extensions.Options;

namespace ProjetoRl.ProjetoRl.API;

/// <summary>Bike service API.</summary>
[ApiController]
[Route("bikes")]
[ApiExplorerSettings(GroupName = "Bikes")]
public class BikeService : ControllerBase
{
    private readonly IBikeRepository _bikeRep;


    // private readonly IConnection _con;

    // /// <summary>Model of RabbitMQ.</summary>
    // private readonly IModel _model;

    /// <summary>
    /// Constructor for BikeService.
    /// Initializes the bike repository and sets up the RabbitMQ connection.
    /// </summary>
    /// <param name="bikeRep">Interface to contract of methods.</param>    
    public BikeService(IBikeRepository bikeRep)
    {
        _bikeRep = bikeRep;
        // var factory = new ConnectionFactory()
        // {
        //     HostName = "localhost",
        //     Port = 5672,
        //     UserName = "guest",
        //     Password = "guest"
        // };
        // _con = factory.CreateConnection();
        // _model = _con.CreateModel();

    }


    /// <summary>
    /// List all bikes with pagination and filtering options.
    /// </summary>
    /// <param name="dto">DTO to list bike.</param>    
    [HttpGet(Name = "GetAllBikes")]
    [Authorize]
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

    /// <summary>
    /// Get a bike by its ID.
    /// </summary>
    /// <param name="id">Identificaticon code Bike</param>
    /// <returns></returns>
    [HttpGet("{id}", Name = "GetBikeById")]
    [Authorize]
    [ProducesResponseType(typeof(Bike), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<Bike>> GetBikeByIdAsync([FromRoute] string id)
    {
        var bike = await _bikeRep.GetByIdAsync(id);
        if (bike == null)
            return NotFound();

        return Ok(bike);
    }


    /// <summary>
    /// Create a new bike.
    /// </summary>
    /// <param name="dto">DTO to create a new bike</param>    
    [HttpPost(Name = "CreateBike")]
    [Authorize]
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

        // PublishBikeRegisteredEvent(createdBike!);

        return CreatedAtAction("GetBikeById", new { id = bikeId }, createdBike);
    }


    /// <summary>
    /// Update an existing bike.
    /// </summary>
    /// <param name="id">Identification code bike.</param>
    /// <param name="dto">DTO to edit a existing bike.</param>    
    [HttpPut("{id}", Name = "UpdateBike")]
    [Authorize]
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

    /// <summary>
    /// Deactivate a bike. 
    /// </summary>
    /// <param name="id">Identification code bike.</param>
    /// <param name="rentalRep">Interface methods to deactivate a rental of bike.</param>
    /// <returns></returns>

    [HttpPatch("{id}/deactivate", Name = "DeactivateBike")]
    [Authorize]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> DeactivateBikeAsync([FromRoute] string id, [FromServices] IRentalRepository rentalRep)
    {
        var bike = await _bikeRep.GetByIdAsync(id);
        if (bike == null)
            return NotFound();

        var activeRental = await rentalRep.GetRentalByBikeIdAsync(bike.ID!);

        if (activeRental != null)
        {
            ModelState.AddModelError("Bike", "Cannot deactivate a bike that is currently rented.");
            return BadRequest(ModelState);
        }

        await _bikeRep.RemoveAsync(id);
        return Ok();
    }

    // private void PublishBikeRegisteredEvent(Bike bike)
    // {
    //     using var channel = _con.CreateModel();

    //     // Garante que o exchange existe
    //     channel.ExchangeDeclare(
    //         exchange: "bike.registered",
    //         type: ExchangeType.Fanout,
    //         durable: true,
    //         autoDelete: false
    //     );

    //     var message = JsonSerializer.Serialize(bike);
    //     var body = Encoding.UTF8.GetBytes(message);

    //     // Propriedades básicas - útil para persistência
    //     var props = channel.CreateBasicProperties();
    //     props.Persistent = true;

    //     channel.BasicPublish(
    //         exchange: "bike.registered",
    //         routingKey: "",
    //         basicProperties: props,
    //         body: body
    //     );

    //     Console.WriteLine($"[RabbitMQ] Evento publicado: {message}");
    // }


}
