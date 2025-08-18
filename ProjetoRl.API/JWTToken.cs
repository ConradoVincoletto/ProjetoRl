using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ProjetoRl.ProjetoRl.API.Config;
using ProjetoRl.ProjetoRl.Domain.Users;

namespace ProjetoRl.ProjetoRl.API;

/// <summary>
/// JWT Token class for handling JSON Web Tokens.
/// </summary>
public class JWTToken
{
    /// <summary>
    /// Gets the JWT token string.
    /// </summary>
    public string Token { get; private set; }

    /// <summary>
    /// Gets the expiration date of the JWT token.
    /// </summary>
    public DateTime Expires { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="JWTToken"/> class with the specified user.
    /// </summary>
    /// <param name="user">user account.</param>
    internal JWTToken(User user)
    {
        Expires = DateTime.UtcNow.AddMinutes(10);

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(AuthConfig.JWTKey);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.ID!.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            }),
            Expires = Expires,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        Token = tokenHandler.WriteToken(token);
    }
}
