using Domain.Rentals;
using Microsoft.AspNetCore.Mvc;
using ProjetoRl.Domain.Rentals.DTOs;
using ProjetoRl.ProjetoRl.Domain.Motorcycles;
using ProjetoRl.ProjetoRl.Domain.Rentals;
using ProjetoRl.ProjetoRl.Domain.Users;
using System.Net;

namespace ProjetoRl.ProjetoRl.API;

[ApiController]
[Route("rentals")]
[ApiExplorerSettings(GroupName = "Rentals")]
public class RentalService : ControllerBase
{
    private readonly IRentalRepository _rentalRep;

    public RentalService(IRentalRepository rentalRep)
    {
        _rentalRep = rentalRep;
    }


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
        return CreatedAtAction(nameof(GetRentalByIdAsync), new { id = rentalId }, createdRental);
    }

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

    private readonly Dictionary<int, decimal> _planCosts = new()
    {
        { 7, 30m },
        { 15, 28m },
        { 30, 22m },
        { 45, 20m },
        { 50, 18m }
    };
}
