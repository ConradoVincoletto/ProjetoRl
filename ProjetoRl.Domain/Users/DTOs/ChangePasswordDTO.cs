namespace ProjetoRl.ProjetoRl.Domain.Users.DTOs;

/// <summary>DTO used in the password change process for an authenticated user.</summary>
public class ChangePasswordDTO
{
    /// <summary>User's current access password.</summary>
    public string CurrentPassword { get; set; } = null!;

    /// <summary>User's new access password.</summary>
    public string NewPassword { get; set; } = null!;

    /// <summary>Confirmation of the user's new access password.</summary>
    public string PasswordConfirmation { get; set; } = null!;
}