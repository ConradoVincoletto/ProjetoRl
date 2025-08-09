namespace ProjetoRl.ProjetoRl.API.Config;

/// <summary>Authentication settings.</summary>
public static class AuthConfig
{
    /// <summary>Key used for encrypting JWT tokens.</summary>
    public static string JWTKey { get; } = "uXRSlnUMSvljGXDcR6ewJsUqn232p1fG";

    //TODO: I prefer to stay here this jwt key, because it is a test project.
}