namespace ProjetoRl.ProjetoRl.Domain.Users;
public enum UserState
{
    /// <summary>The user account is inactive.</summary>
    Deactivated = 0,

    /// <summary>The user account is active.</summary>
    Active = 1,

    /// <summary>The user's account has been temporarily deactivated by the user.</summary>
    DeactivatedByUser = 2,

    /// <summary>The user account has been deleted.</summary>
    Removed = 3,

    /// <summary>The user account is pending activation.</summary>
    Pending = 4
}