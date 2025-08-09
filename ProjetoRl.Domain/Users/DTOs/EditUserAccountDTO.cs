using Domain.Users;

namespace ProjetoRl.Domain.Users.DTOs;

/// <summary>DTO used on user profile edition process.</summary>
public class EditUserProfileDto
{
    /// <summary>User's first name.</summary>
    public string FirstName { get; set; } = null!;

    /// <summary>User's last name.</summary>
    public string LastName { get; set; } = null!;

    /// <summary>User's email address.</summary>
    public string Email { get; set; } = null!;

    /// <summary>User's phone number.</summary>
    public string? Cellphone { get; set; } = null;

    /// <summary>Access profiles assigned to the user.</summary>
    public IEnumerable<Role>? Roles { get; set; }
}
