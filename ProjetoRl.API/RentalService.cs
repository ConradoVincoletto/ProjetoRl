using Domain.Rentals;
using Microsoft.AspNetCore.Mvc;
using ProjetoRl.Domain.Rentals.DTOs;
using ProjetoRl.ProjetoRl.Domain.Motorcycles;
using ProjetoRl.ProjetoRl.Domain.Rentals;
using ProjetoRl.ProjetoRl.Domain.Users;
using System.Net;

namespace ProjetoRl.ProjetoRl.API;

/// <summary>
/// Rental Service API
/// </summary>
[ApiController]
[Route("api/[controller]")]
[ApiExplorerSettings(GroupName = "Rentals")]
public class RentalService : ControllerBase
{
    private readonly IRentalRepository _rentalRep;

    /// <summary>
    /// Constructor to rental service
    /// </summary>
    /// <param name="rentalRep">Interface method</param>
    public RentalService(IRentalRepository rentalRep)
    {
        _rentalRep = rentalRep;        
    }

    /// <summary>
    /// Get rental by id
    /// </summary>
    /// <param name="id">Identification code Rental.</param>    
    [HttpGet("{id}", Name = "GetRentalById")]
    [ProducesResponseType(typeof(Rental), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<Rental>> GetRentalByIdAsync([FromRoute] string id)
    {
        var rental = await _rentalRep.GetByIdAsync(id);
        if (rental == null)
            return NotFound();

        return Ok(rental);
    }
    
    /// <summary>
    /// Get rental by bike id
    /// </summary>
    /// <param name="id">Identication code bike.</param>
    /// <returns></returns>
    [HttpGet("bike/{id}", Name = "GetRentalByBikeId")]
    [ProducesResponseType(typeof(Rental), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<Rental>> GetRentalByBikeIdAsync([FromRoute] string id)
    {
        var rental = await _rentalRep.GetRentalByBikeIdAsync(id);
        if (rental == null)
            return NotFound();

        return Ok(rental);
    }
    
    /// <summary>
    /// Create a new rental
    /// </summary>
    /// <param name="dto">DTO to create a new rental for bike.</param>
    /// <param name="courierRepository">Courier Methods to call in create rental.</param>
    /// <param name="bikeRepository">Bike methods tp call in create a new rental.</param>    
    [HttpPost(Name = "CreateRental")]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<Rental>> CreateRentalAsync([FromBody] CreateRentalDTO dto,
                                                              [FromServices] ICourierRepository courierRepository,
                                                              [FromServices] IBikeRepository bikeRepository)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var courier = await courierRepository.GetByIdAsync(dto.CourierId);
        if (courier == null)
        {
            ModelState.AddModelError(nameof(dto.CourierId), "Entregador não encontrado.");
            return BadRequest(ModelState);
        }

        // Valida categoria CNH
        if (courier.DriverLicenseType != LicenseType.A)
        {
            ModelState.AddModelError(nameof(dto.CourierId), "Entregador não habilitado na categoria A.");
            return BadRequest(ModelState);
        }

        var bike = await bikeRepository.GetByIdAsync(dto.BikeId);

        if (bike == null)
        {
            ModelState.AddModelError(nameof(dto.BikeId), "Moto não encontrada.");
            return BadRequest(ModelState);
        }

        // Valida plano
        if (!_planCosts.ContainsKey(dto.PlanDays))
        {
            ModelState.AddModelError(nameof(dto.PlanDays), "Plano inválido. Planos disponíveis: 7, 15, 30, 45 ou 50 dias.");
            return BadRequest(ModelState);
        }

        // ✅ Define custo diário e datas
        dto.DailyCost = _planCosts[dto.PlanDays];
        dto.StartDate = DateTime.UtcNow.Date.AddDays(1); // começa no próximo dia
        var expectedEndDate = dto.StartDate.AddDays(dto.PlanDays);

        var rental = new Rental(dto)
        {
            ExpectedEndDate = expectedEndDate
        };

        var rentalId = await _rentalRep.CreateAsync(rental);

        var createdRental = await _rentalRep.GetByIdAsync(rentalId);
        return CreatedAtAction("GetRentalById", new { id = rentalId }, createdRental);
    }

    /// <summary>
    /// Update an existing rental
    /// </summary>
    /// <param name="id">Identification code Rental.</param>
    /// <param name="dto">DTO to update a Rental.</param>    
    [HttpPut("{id}", Name = "UpdateRental")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> UpdateRentalAsync([FromRoute] string id, [FromBody] UpdateRentalDTO dto)
    {
        var rental = await _rentalRep.GetByIdAsync(id);
        if (rental == null)
            return NotFound();
        
        var updateRental = new Rental(dto);
        
        await _rentalRep.EditAsync(updateRental);

        return NoContent();
    }

    /// <summary>
    /// Finalize an existing rental
    /// </summary>
    /// <param name="id">Identification code Rental.</param>
    /// <param name="dto">DTO to finalize a rental bike.</param>
    [HttpPatch("{id}/finalize", Name = "FinalizeRental")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> FinalizeRentalAsync([FromRoute] string id, [FromBody] FinalizeRentalDTO dto)
    {
        var rental = await _rentalRep.GetByIdAsync(id);
        if (rental == null)
            return NotFound();       

        rental.FinalizeRental(dto.ActualEndDate);
        await _rentalRep.EditAsync(rental);

        return NoContent();
    }

    /// <summary>
    /// Dictionary to hold plan costs
    /// </summary>
    private readonly Dictionary<int, decimal> _planCosts = new()
    {
        { 7, 30m },
        { 15, 28m },
        { 30, 22m },
        { 45, 20m },
        { 50, 18m }
    };
}
