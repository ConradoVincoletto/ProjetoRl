using Domain.Users;
using ProjetoRl.Domain.Users.DTOs;

namespace ProjetoRl.ProjetoRl.Domain.Users;

public class User
{
    /// <summary>User identification code.</summary>
    public string ID { get; private set; } = null!;

    /// <summary>User's first name.</summary>
    public string FirstName { get; private set; } = null!;

    /// <summary>User's last name.</summary>
    public string LastName { get; private set; } = null!;

    /// <summary>User's email address.</summary>
    public string Email { get; private set; } = null!;

    /// <summary>User's cellphone number.</summary>
    public string? Cellphone { get; private set; }

    /// <summary>Profiles assigned to the user's account.</summary>
    public IEnumerable<Role> Roles { get; private set; } = null!;

    /// <summary>User account state.</summary>
    public UserState State { get; private set; }

    /// <summary>Constructor used in the registration process with local password setup.</summary>
    /// <param name="dto">DTO for the user registration process with local password setup.</param>
    public User(CreateUserAccountDto dto)
    {
        FirstName = dto.FirstName;
        LastName = dto.LastName;
        Email = dto.Email;
        Cellphone = dto.Cellphone;
        Roles = dto.Roles;
        State = UserState.Pending; 
    }

    /// <summary>Standard constructor.</summary>
    public User() { }

    /// <summary>Constructor with parameters for initialization.</summary>
    /// <param name="iD">User identification code.</param>
    /// <param name="firstName">User's first name.</param>
    /// <param name="lastName">User's last name.</param>
    /// <param name="email">User's email address.</param>
    /// <param name="cellphone">User's cellphone number.</param>
    /// <param name="roles">Profiles assigned to the user's account.</param>
    /// <param name="state">User account state.</param>    
    public User(string iD, string firstName, string lastName, string email, string? cellphone, IEnumerable<Role> roles, UserState state)
    {
        ID = iD;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Cellphone = cellphone;
        Roles = roles;
        State = state;
    }
    // Other properties and methods related to the User entity can be added here.
}