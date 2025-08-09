using Domain.Users;
using Microsoft.AspNetCore.Mvc;
using ProjetoRl.Domain.Users.DTOs;
using System.Net;

namespace ProjetoRl.ProjetoRl.API;

[ApiController]
[Route("couriers")]
[ApiExplorerSettings(GroupName = "Couriers")]
public class CourierService : ControllerBase
{
    private readonly ICourierRepository _courierRep;

    public CourierService(ICourierRepository courierRep)
    {
        _courierRep = courierRep;
    }

    [HttpGet(Name = "GetAllCouriers")]
    [ProducesResponseType(typeof(IEnumerable<Courier>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<Courier>>> GetAllCouriersAsync()
    {
        var couriers = await _courierRep.GetAllAsync();
        return Ok(couriers);
    }

    [HttpGet("{id}", Name = "GetCourierById")]
    [ProducesResponseType(typeof(Courier), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<Courier>> GetCourierByIdAsync([FromRoute] string id)
    {
        var courier = await _courierRep.GetByIdAsync(id);
        if (courier == null)
            return NotFound();

        return Ok(courier);
    }

    [HttpPost(Name = "CreateCourier")]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<Courier>> CreateCourierAsync([FromBody] CreateCourierDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var courier = new Courier(dto);
        var courierId = await _courierRep.CreateAsync(courier);

        var createdCourier = await _courierRep.GetByIdAsync(courierId);
        return CreatedAtAction(nameof(GetCourierByIdAsync), new { id = courierId }, createdCourier);
    }

    [HttpPut("{id}", Name = "UpdateCourier")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> UpdateCourierAsync([FromRoute] string id, [FromBody] UpdateCourierDTO dto)
    {
        var existing = await _courierRep.GetByIdAsync(id);
        if (existing == null)
            return NotFound();

        existing.Update(dto);
        await _courierRep.UpdateAsync(existing);

        return NoContent();
    }

    [HttpPatch("{id}/deactivate", Name = "DeactivateCourier")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> DeactivateCourierAsync([FromRoute] string id)
    {
        var courier = await _courierRep.GetByIdAsync(id);
        if (courier == null)
            return NotFound();

        courier.Deactivate();
        await _courierRep.UpdateAsync(courier);

        return NoContent();
    }

    [HttpPatch("{id}/activate", Name = "ReactivateCourier")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> ReactivateCourierAsync([FromRoute] string id)
    {
        var courier = await _courierRep.GetByIdAsync(id);
        if (courier == null)
            return NotFound();

        courier.Activate();
        await _courierRep.UpdateAsync(courier);

        return NoContent();
    }
}
