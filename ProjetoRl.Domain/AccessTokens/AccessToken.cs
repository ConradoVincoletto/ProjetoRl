namespace ProjetoRl.ProjetoRl.Domain.AccessTokens;

/// <summary>Entity that represents a user's token.</summary>
public class AccessToken
{
    /// <summary>Identification code of the token.</summary>
    public string ID { get; private set; } = null!;

    /// <summary>JWT token code for user access control.</summary>
    public string JWTToken { get; set; } = null!;

    /// <summary>Identification code of the user to whom the token refers.</summary>
    public string UserId { get; set; }

    /// <summary>IP that generated the token.</summary>
    public string? IP { get; set; } = null!;

    /// <summary>Flag indicating if the token has been updated.</summary>
    public bool Refreshed { get; set; }

    /// <summary>Token creation date.</summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>Token expiration date.</summary>
    public DateTime Expires { get; set; }

    /// <summary>Date the token was changed.</summary>
    public DateTime? RefreshedAt { get; set; }

    /// <summary>Indicates if the access token has been revoked.</summary>
    public bool Canceled { get; set; }

    /// <summary>Constructor for creating a new access token.</summary>
    /// <param name="jWTToken">JWT token.</param>
    /// <param name="refreshToken">Refresh token.</param>
    /// <param name="userId">User's identification code.</param>
    /// <param name="iP">IP address used by the user to generate the token.</param>
    /// <param name="expires">Token expiration date.</param>
    /// <param name="type">Type of passport.</param>
    public AccessToken(string jWTToken, string userId, string? iP, DateTime expires)
    {
        JWTToken = jWTToken;       
        UserId = userId;
        IP = iP;
        CreatedAt = DateTime.Now;
        Expires = expires;
        Refreshed = false;
        Canceled = false;
    }

    /// <summary>Constructor for instantiating an existing token.</summary>
    /// <param name="iD">Identification code of the token.</param>
    /// <param name="jWTToken">JWT token.</param>
    /// <param name="refreshToken">Refresh token.</param>
    /// <param name="userId">User's identification code.</param>
    /// <param name="iP">IP address used by the user to generate the token.</param>
    /// <param name="refreshed">Flag indicating whether the token has been updated.</param>
    /// <param name="createdAt">Token creation date.</param>
    /// <param name="expires">Token expiration date.</param>
    /// <param name="refreshedAt">Date the token was changed.</param>
    /// <param name="canceled">Indicates if the access token has been revoked.</param>
    /// <param name="type">Type of passport.</param>
    public AccessToken(string iD, string jWTToken, string userId, string? iP, bool refreshed,
        DateTime createdAt, DateTime expires, DateTime? refreshedAt, bool canceled)
    {
        ID = iD;        
        JWTToken = jWTToken;        
        UserId = userId;
        IP = iP;
        Refreshed = refreshed;
        CreatedAt = createdAt;
        Expires = expires;
        RefreshedAt = refreshedAt;
        Canceled = canceled;
    }

    /// <summary>Type of passport.</summary>
    /// <param name="jWTToken">JWT token.</param>
    /// <param name="userId">Identification code of the user.</param>
    /// <param name="type">Type of passport.</param>
    public AccessToken(string jWTToken, string userId)
    {
        JWTToken = jWTToken;        
        UserId = userId;
    }
}