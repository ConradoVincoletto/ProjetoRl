using Domain.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjetoRl.ProjetoRl.Domain.Users;
using ProjetoRl.ProjetoRl.Domain.Users.DTOs;
using System.Net;

namespace ProjetoRl.ProjetoRl.API;

/// <summary>
/// Service to management couriers.
/// </summary>
[ApiController]
[Route("couriers")]
[ApiExplorerSettings(GroupName = "Couriers")]
public class CourierService : ControllerBase
{
    private readonly ICourierRepository _courierRep;

    /// <summary>
    /// Constructor Service.
    /// </summary>
    /// <param name="courierRep">Interface method Courier.</param>
    public CourierService(ICourierRepository courierRep)
    {
        _courierRep = courierRep;
    }

    /// <summary>
    /// List Couriers and pagination.
    /// </summary>
    [HttpGet(Name = "ListCouriers")]
    [Authorize]
    public async Task<IActionResult> GetCouriers([FromQuery] CourierListFilterDTO dto)
    {
        var result = await _courierRep.ListAsync(dto.Identifier,
                                                 dto.Name,
                                                 dto.Cnpj,
                                                 dto.DriverLicenseNumber,
                                                 dto.States,
                                                 dto.PageIndex,
                                                 dto.PageSize);

        return Ok(result);
    }

    /// <summary>
    /// Get Courier by Id.
    /// </summary>
    /// <param name="id">Identification code Courirer.</param>    
    [HttpGet("{id}", Name = "GetCourierById")]
    [ProducesResponseType(typeof(Courier), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [Authorize]
    public async Task<ActionResult<Courier>> GetCourierByIdAsync([FromRoute] string id)
    {
        var courier = await _courierRep.GetByIdAsync(id);
        if (courier == null)
            return NotFound();

        return Ok(courier);
    }

    /// <summary>
    /// Create a new Courier.
    /// </summary>
    /// <param name="dto">DTO to create a new courier.</param>        
    [HttpPost(Name = "CreateCourier")]
    [Authorize]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<Courier>> CreateCourierAsync([FromForm] CreateCourierDTO dto)
    {

        if (await CheckCNPJInUseAsync(null, dto.Cnpj))
        {
            ModelState.AddModelError("CNPJ", "Cnpj já está em uso.");
            return BadRequest(ModelState);
        }

        if (await CheckCNHInUseAsync(null, dto.DriverLicenseNumber))
        {
            ModelState.AddModelError("CNH", "CNH já está em uso.");
            return BadRequest(ModelState);
        }

        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        if (dto.DriverLicenseImage != null)
        {
            dto.DriverLicenseImagePath = await SaveDriverLicenseImageAsync(dto.Identifier, dto.DriverLicenseImage);
        }

        var courier = new Courier(dto);
        var courierId = await _courierRep.CreateAsync(courier);

        var createdCourier = await _courierRep.GetByIdAsync(courierId);
        return CreatedAtAction("GetCourierById", new { id = courierId }, createdCourier);
    }

    /// <summary>
    /// Update a existing Courier.
    /// </summary>
    /// <param name="id">Identification code Courier.</param>
    /// <param name="dto">DTO to edit a existing courier.</param>        
    [HttpPut("{id}", Name = "UpdateCourier")]
    [Authorize]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UpdateCourierAsync([FromRoute] string id, [FromForm] EditCourierDTO dto)
    {
        // Get existing courier to update
        {
            var courierUser = await _courierRep.GetByIdAsync(id);
            if (courierUser == null)
                return NotFound();

            if (dto.DriverLicenseImage != null)
                dto.DriverLicenseImagePath = await SaveDriverLicenseImageAsync(courierUser.Identifier, dto.DriverLicenseImage, courierUser.DriverLicenseImagePath);
            else
                dto.DriverLicenseImagePath = courierUser.DriverLicenseImagePath;

            var updateCourier = new Courier(courierUser, dto);
            await _courierRep.EditAsync(updateCourier);

            return Ok();
        }
    }
    /// <summary>
    /// Deactivate a existing Courier.
    /// </summary>
    /// <param name="id">Identification code Courier.</param>    
    [HttpPatch("{id}/deactivate", Name = "DeactivateCourier")]
    [Authorize]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> DeactivateCourierAsync([FromRoute] string id)
    {
        var courierUser = await _courierRep.GetByIdAsync(id);
        if (courierUser == null)
            return NotFound();

        await _courierRep.DeactivateCourierAccountAsync(id);

        return Ok();
    }

    /// <summary>
    /// Reactivate a existing Courier.
    /// </summary>
    /// <param name="id">Identification code Courier.</param>    
    [HttpPatch("{id}/activate", Name = "ReactivateCourier")]
    [Authorize]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> ReactivateCourierAsync([FromRoute] string id)
    {
        var courierUser = await _courierRep.GetByIdAsync(id);
        if (courierUser == null)
            return NotFound();

        await _courierRep.ActivateCourirerAccountAsync(id);

        return Ok();
    }

    /// <summary>
    /// Check if CNPJ is already in use.
    /// </summary>
    /// <param name="id">Identification code Courier.</param>
    /// <param name="cNPJ">Documento of courier to check if exist.</param>    
    private async Task<bool> CheckCNPJInUseAsync(string? id, string cNPJ)
    {
        var cnpj = (await _courierRep.ListAsync(null, null, cNPJ, null, null, 1, 10)).Data
            .SingleOrDefault(c => c.Cnpj.Equals(cNPJ, StringComparison.InvariantCultureIgnoreCase));

        if (id != null)
            return cnpj != null && cnpj!.ID != id;

        return cnpj != null;
    }

    private async Task<bool> CheckCNHInUseAsync(string? id, string cNH)
    {
        var cnh = (await _courierRep.ListAsync(null, null, null, null, null, 1, 10)).Data
            .SingleOrDefault(c => c.Cnpj.Equals(cNH, StringComparison.InvariantCultureIgnoreCase));

        if (id != null)
            return cnh != null && cnh!.ID != id;

        return cnh != null;
    }

    private async Task<string> SaveDriverLicenseImageAsync(string identifier, IFormFile? driverLicenseImage, string? oldImagePath = null)
    {
        if (driverLicenseImage == null)
            return oldImagePath ?? string.Empty;

        try
        {
            // Detecta a extensão pelo ContentType do arquivo
            string extension = Path.GetExtension(driverLicenseImage.FileName);
            if (string.IsNullOrEmpty(extension))
            {
                if (driverLicenseImage.ContentType == "image/png")
                    extension = ".png";
                else if (driverLicenseImage.ContentType == "image/bmp")
                    extension = ".bmp";
                else
                    extension = ".jpg"; // padrão
            }

            // Gera nome único
            var fileName = $"CNH_{identifier}_{DateTime.Now:yyyyMMddHHmmss}{extension}";

            // Caminho físico
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "cnh");
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var filePath = Path.Combine(folderPath, fileName);

            // Copia o arquivo recebido para o disco
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await driverLicenseImage.CopyToAsync(stream);
            }

            // Remove imagem antiga se existir
            if (!string.IsNullOrEmpty(oldImagePath))
            {
                var oldFileFullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", oldImagePath);
                if (System.IO.File.Exists(oldFileFullPath))
                    System.IO.File.Delete(oldFileFullPath);
            }

            // Retorna caminho relativo
            return Path.Combine("images", "cnh", fileName).Replace("\\", "/");
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Erro ao salvar imagem da CNH: {ex.Message}");
        }
    }


}
