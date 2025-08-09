using Domain.Rentals;
using Microsoft.AspNetCore.Mvc;
using ProjetoRl.Domain.Rentals.DTOs;
using System.Net;

namespace ProjetoRl.API.Controllers;

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

    [HttpGet(Name = "GetAllRentals")]
    [ProducesResponseType(typeof(IEnumerable<Rental>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<Rental>>> GetAllRentalsAsync()
    {
        var rentals = await _rentalRep.GetAllAsync();
        return Ok(rentals);
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

    [HttpPost(Name = "CreateRental")]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<Rental>> CreateRentalAsync([FromBody] CreateRentalDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var rental = new Rental(dto);
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

        rental.Update(dto);
        await _rentalRep.UpdateAsync(rental);

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
        await _rentalRep.UpdateAsync(rental);

        return NoContent();
    }
}
