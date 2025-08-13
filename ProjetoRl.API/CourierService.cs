using Domain.Users;
using Microsoft.AspNetCore.Mvc;
using ProjetoRl.ProjetoRl.Domain.Users;
using ProjetoRl.ProjetoRl.Domain.Users.DTOs;
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

    /// <summary>
    /// Lista couriers com filtros opcionais e paginação
    /// </summary>
    [HttpGet(Name = "ListCouriers")]
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

        if (!string.IsNullOrEmpty(dto.DriverLicenseImageBase64))
            dto.DriverLicenseImagePath = await SaveDriverLicenseImageAsync(dto.Identifier, dto.DriverLicenseImageBase64);

        var courier = new Courier(dto);
        var courierId = await _courierRep.CreateAsync(courier);

        var createdCourier = await _courierRep.GetByIdAsync(courierId);
        return CreatedAtAction(nameof(GetCourierByIdAsync), new { id = courierId }, createdCourier);
    }


    [HttpPut("{id}", Name = "UpdateCourier")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> UpdateCourierAsync([FromRoute] string id, [FromBody] EditCourierDTO dto)
    {
        var courierUser = await _courierRep.GetByIdAsync(id);
        if (courierUser == null)
            return NotFound();

        if (!string.IsNullOrEmpty(dto.DriverLicenseImageBase64))
            dto.DriverLicenseImagePath = await SaveDriverLicenseImageAsync(courierUser.Identifier, dto.DriverLicenseImageBase64, courierUser.DriverLicenseImagePath);
        else
            dto.DriverLicenseImagePath = courierUser.DriverLicenseImagePath;

        var updateCourier = new Courier(courierUser, dto);
        await _courierRep.EditAsync(updateCourier);

        return Ok();
    }

    [HttpPatch("{id}/deactivate", Name = "DeactivateCourier")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> DeactivateCourierAsync([FromRoute] string id)
    {
        var courierUser = await _courierRep.GetByIdAsync(id);
        if (courierUser == null)
            return NotFound();

        var editCourierUser = _courierRep.DeactivateCourierAccountAsync(id);

        return Ok(editCourierUser);
    }

    [HttpPatch("{id}/activate", Name = "ReactivateCourier")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> ReactivateCourierAsync([FromRoute] string id)
    {
        var courierUser = await _courierRep.GetByIdAsync(id);
        if (courierUser == null)
            return NotFound();

        var reactiveCourierUser = _courierRep.ActivateCourirerAccountAsync(id);

        return Ok(reactiveCourierUser);
    }

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

    private async Task<string> SaveDriverLicenseImageAsync(string identifier, string base64Image, string? oldImagePath = null)
    {
        if (string.IsNullOrEmpty(base64Image))
            return oldImagePath ?? string.Empty;

        try
        {
            string extension = ".jpg"; // padrão
            string base64Data = base64Image;

            // Detecta tipo pelo cabeçalho
            if (base64Data.StartsWith("data:image/png"))
                extension = ".png";
            else if (base64Data.StartsWith("data:image/bmp"))
                extension = ".bmp";
            else if (base64Data.StartsWith("data:image/jpeg"))
                extension = ".jpg";

            // Remove o prefixo "data:image/png;base64,"
            var base64WithoutPrefix = base64Data.Contains(",")
                ? base64Data.Split(',')[1]
                : base64Data;

            var imageBytes = Convert.FromBase64String(base64WithoutPrefix);

            // Gera nome único
            var fileName = $"CNH_{identifier}_{DateTime.Now:yyyyMMddHHmmss}{extension}";

            // Caminho físico
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "cnh");
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var filePath = Path.Combine(folderPath, fileName);

            // Salva no disco
            await System.IO.File.WriteAllBytesAsync(filePath, imageBytes);

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
