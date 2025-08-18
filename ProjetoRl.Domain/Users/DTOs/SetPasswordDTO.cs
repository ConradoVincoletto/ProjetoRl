   namespace ProjetoRl.ProjetoRl.Domain.Users.DTOs; 
   
   /// <summary>
   /// Data Transfer Object for setting a user's password.
   /// </summary>
    public class SetPasswordDto
{
    /// <summary>
    /// The email address of the user for whom the password is being set.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// The new password to be set for the user.
    /// </summary>
    public string Password { get; set; } = string.Empty;
}