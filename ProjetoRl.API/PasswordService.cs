using Microsoft.AspNetCore.Mvc;
using ProjetoRl.ProjetoRl.Domain.Users;
using ProjetoRl.ProjetoRl.Domain.Users.DTOs;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace ProjetoRl.ProjetoRl.API;

/// <summary>
/// Service for managing user passwords.
/// </summary>
[ApiController]
[Route("password")]
[ApiExplorerSettings(GroupName = "Passwords")]
public class PasswordService : ControllerBase
{
    /// <summary>
    /// Sets a password for a user based on their email address.
    /// </summary>
    /// <param name="dto">DTO to set a password.</param>
    /// <param name="userRep">Interface to set a password in user account.</param>    
    [HttpPost(Name = "setPassword")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> SetPasswordAsync(
        [FromBody] SetPasswordDto dto,
        [FromServices] IUserRepository userRep
    )
    {
        var user = await userRep.GetByEmailAsync(dto.Email);
        if (user == null)
            return NotFound($"Usuário com email {dto.Email} não encontrado.");

        user.PasswordHash = HashPassword(dto.Password);

        await userRep.EditAsync(user);

        return Ok("Senha definida com sucesso.");
    }

    private string HashPassword(string password)
    {
        using var sha = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = sha.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }
}




