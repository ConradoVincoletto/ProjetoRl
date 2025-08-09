using Azure.Core;
using Domain.Users;
using Microsoft.AspNetCore.Mvc;
using ProjetoRl.Domain.Users.DTOs;
using ProjetoRl.ProjetoRl.API;

[ApiController]
[Route("auth")]
[ApiExplorerSettings(GroupName = "Authentication")]
public class AuthService : ControllerBase
{
    private readonly IAccessTokenRepository _accessTokenRep;
    private readonly IUserRepository _userRep;

    public AuthService(IAccessTokenRepository accessTokenRep, IUserRepository userRep)
    {
        _accessTokenRep = accessTokenRep;
        _userRep = userRep;
    }

    [HttpPost("auth", Name = "AuthenticateWithPassword")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<JWTToken>> AuthenticateWithPasswordAsync([FromBody] AuthenticateWithPasswordDto dto)
    {
        // Busca usuário pelo e-mail
        var user = await _userRep.GetByEmailAsync(dto.Email);
        if (user == null || !user.ValidatePassword(dto.Password))
            return Unauthorized();

        if (user.State == UserState.Deactivated)
            return Forbid();

        var token = new JWTToken(user);

        var ip = Request?.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? "127.0.0.1";
        var accessToken = new AccessToken(token.Token, token.Expires);

        await _accessTokenRep.SaveAsync(accessToken);

        return Ok(token);
    }
}
