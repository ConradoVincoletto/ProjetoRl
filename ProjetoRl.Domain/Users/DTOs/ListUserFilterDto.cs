using Domain.Users;

namespace ProjetoRl.ProjetoRl.Domain.Users.DTOs;

/// <summary>DTO containing the filter parameters received in the endpoint that retrieves the list of platform users.</summary>
public class    ListUsersFilterDto
{
    /// <summary>User's email address.</summary>
    public string? Email { get; set; }

    /// <summary>User's first name.</summary>
    public string? FirstName { get; set; }

    /// <summary>User's last name.</summary>
    public string? LastName { get; set; }

    /// <summary>User states.</summary>
    public IEnumerable<UserState>? States { get; set; }

    /// <summary>User states.</summary>
    public IEnumerable<Role>? Roles { get; set; }

    /// <summary>Current search page.</summary>
    public uint PageIndex { get; set; } = 1;

    /// <summary>Number of records returned per page.</summary>
    public uint PageSize { get; set; } = 50;
}