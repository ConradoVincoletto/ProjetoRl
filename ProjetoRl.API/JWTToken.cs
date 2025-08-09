using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ProjetoRl.ProjetoRl.API.Config;
using ProjetoRl.ProjetoRl.Domain.Users;

namespace ProjetoRl.ProjetoRl.API;

public class JWTToken
{
    public string Token { get; private set; }
    public DateTime Expires { get; private set; }

    internal JWTToken(User user)
    {
        Expires = DateTime.UtcNow.AddMinutes(10);

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(AuthConfig.JWTKey);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.ID.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            }),
            Expires = Expires,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        Token = tokenHandler.WriteToken(token);
    }
}
