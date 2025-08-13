using Domain.Users;

namespace ProjetoRl.ProjetoRl.Domain.Users.DTOs;

/// <summary>DTO used in the user registration process with password authentication.</summary>
public class CreateUserAccountDto
{
    /// <summary>User's first name.</summary>
    public string FirstName { get; set; } = null!;

    /// <summary>User's last name.</summary>
    public string LastName { get; set; } = null!;

    /// <summary>User's email address.</summary>
    public string Email { get; set; } = null!;

    /// <summary>User's phone number.</summary>
    public string? Cellphone { get; set; } = null;  

    /// <summary>Roles of user logged.</summary>
    public IEnumerable<Role> Roles {get; set;} = null!;          
}