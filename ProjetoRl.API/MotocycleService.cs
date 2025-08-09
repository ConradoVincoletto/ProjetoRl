using Microsoft.AspNetCore.Mvc;
using ProjetoRl.Domain.Motorcycles.DTOs;
using ProjetoRl.ProjetoRl.Domain.Motorcycles;
using System.Net;

namespace ProjetoRl.API.Controllers;

[ApiController]
[Route("bikes")]
[ApiExplorerSettings(GroupName = "Bikes")]
public class MotocycleService : ControllerBase
{
    private readonly IBikeRepository _bikeRep;

    public MotocycleService(IBikeRepository bikeRep)
    {
        _bikeRep = bikeRep;
    }

    [HttpGet(Name = "GetAllBikes")]
    [ProducesResponseType(typeof(IEnumerable<Bike>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<Bike>>> GetAllBikesAsync()
    {
        var bikes = await _bikeRep.GetAllAsync();
        return Ok(bikes);
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

        bike.Update(dto);
        await _bikeRep.UpdateAsync(bike);

        return NoContent();
    }

    [HttpPatch("{id}/deactivate", Name = "DeactivateBike")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> DeactivateBikeAsync([FromRoute] string id)
    {
        var bike = await _bikeRep.GetByIdAsync(id);
        if (bike == null)
            return NotFound();

        bike.Deactivate();
        await _bikeRep.UpdateAsync(bike);

        return NoContent();
    }

    [HttpPatch("{id}/activate", Name = "ReactivateBike")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> ReactivateBikeAsync([FromRoute] string id)
    {
        var bike = await _bikeRep.GetByIdAsync(id);
        if (bike == null)
            return NotFound();

        bike.Activate();
        await _bikeRep.UpdateAsync(bike);

        return NoContent();
    }
}
