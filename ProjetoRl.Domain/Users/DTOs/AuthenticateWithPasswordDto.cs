namespace ProjetoRl.ProjetoRl.Domain.Users.DTOs;

/// <summary>DTO used in the authentication process on the local database.</summary>
public class AuthenticateWithPasswordDto
{
    /// <summary>Admin's email address.</summary>
    public string Email { get; set; } = null!;

    /// <summary>Password provided for authentication.</summary>
    public string Password { get; set; } = null!;
}