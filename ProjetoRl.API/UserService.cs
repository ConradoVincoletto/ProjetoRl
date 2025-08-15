using Domain.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjetoRl.ProjetoRl.Commom;
using ProjetoRl.ProjetoRl.Domain.Users;
using ProjetoRl.ProjetoRl.Domain.Users.DTOs;
using System.Net;
using System.Security.Claims;

namespace ProjetoRl.ProjetoRl.API;

/// <summary>Services that provide access to user account data and operations.</summary>
[ApiController]
[Route("users")]
[ApiExplorerSettings(GroupName = "Users")]
public class UserService : ControllerBase
{
    /// <summary>Repository for storing users.</summary>
    private readonly IUserRepository _userRep;

    /// <summary>Constructor with dependency injection.</summary>
    /// <param name="userRep">Repository for storing users.</param>        
    /// <param name="config">Application configuration object.</param>        
    public UserService(IUserRepository userRep, IConfiguration config)
    {
        _userRep = userRep;
    }

    /// <summary>Retrieves the list of all users registered on the platform that meet the filter criteria.</summary>
    /// <param name="dto">DTO containing the filter parameters.</param>
    /// <returns>List of users registered on the platform that meet the filter criteria.</returns>
    [HttpGet(Name = "ListUsers")]
    // [Authorize(Roles = "Administrator")]
    public async Task<ActionResult<PagedResult<User>>> ListUsersAsync([FromQuery] ListUsersFilterDto dto)
    {
        if (!User.IsInRole("Administrator"))
            return Forbid();

        return await _userRep.ListAsync(dto.Email, dto.FirstName, dto.LastName, dto.States, dto.Roles, dto.PageIndex, dto.PageSize);
    }

    /// <summary>Gets user information based on their identification code.</summary>
    /// <param name="id">User identification code.</param>
    /// <returns>Entity containing user information.</returns>
    [HttpGet("{id}", Name = "GetUserById")]
    [Authorize]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    public async Task<ActionResult<User>> GetUserByIdAsync([FromRoute(Name = "id")] string id)
    {

        var loggedUserId = User?.Claims.First(c => c.Type == ClaimTypes.Sid).Value;
        if (User != null && !User.IsInRole("Administrator") && loggedUserId != id)
            return Forbid();


        var user = await _userRep.GetByIdAsync(id);
        if (user == null)
            return NotFound();

        return user;
    }


    /// <summary>Creates a new user account using local authentication by setting a password.</summary>
    /// <param name="dto">DTO containing the necessary information for the account registration process with password setup.</param>
    [HttpPost(Name = "CreateUser")]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    public async Task<ActionResult<User>> CreateUserAccountAsync([FromBody] CreateUserAccountDto dto)
    {
        await ValidateIfAnEmailIsInUseAsync(dto.Email);
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // Create an user and a passport.
        var user = new User(dto);
        var userId = await _userRep.CreateAsync(user);

        user = await _userRep.GetByIdAsync(userId);

        return CreatedAtAction("GetUserById", new { id = user!.ID }, user);
    }

    /// <summary>Edit the basic information of a user account.</summary>
    /// <param name="id">Identification code of the user whose information will be edited.</param>
    /// <param name="dto">DTO containing the updated user information.</param>        
    [Authorize]
    [HttpPut("{id}", Name = "EditUserProfile")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult> EditUserProfileAsync([FromRoute] string id, [FromBody] EditUserAccountDTO dto)
    {
        var loggedUserId = User?.Claims.First(c => c.Type == ClaimTypes.Sid).Value;

        // Verifica se o usuário tem permissão para editar
        if (User != null && !User.IsInRole("Administrator") && loggedUserId != id)
            return Forbid();

        var user = await _userRep.GetByIdAsync(id);
        if (user == null)
        {
            ModelState.AddModelError("id", string.Format("User not found.", id));
            return NotFound(ModelState);
        }

        // Verifica se o e-mail já está em uso
        await ValidateIfAnEmailIsInUseAsync(dto.Email!, id);
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // Valida a role se o usuário for Administrador
        if (User?.IsInRole("Administrator") == true && dto.Roles == null)
        {
            ModelState.AddModelError("role", string.Format("Role is null.", id));
            return BadRequest(ModelState); // Alterado para BadRequest
        }

        // Persiste as alterações
        await _userRep.EditAsync(user);

        return Ok(user);
    }


    /// <summary>Deletes a user.</summary>
    /// <param name="id">Identification code of the user to be deleted.</param>
    [HttpDelete("{id}", Name = "RemoveUser")]
    [Authorize(Roles = "Administrator")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult> RemoveAsync([FromRoute] string id)
    {
        //check exists user in database
        var user = await _userRep.GetByIdAsync(id);
        if (user == null)
            return NotFound();

        // Check if the user is attempting to self-exclude.
        var loggedUserId = User?.Claims.First(c => c.Type == ClaimTypes.Sid).Value;
        if (loggedUserId == id)
            return Forbid();

        await _userRep.RemoveAsync(id);

        return Ok();
    }

    /// <summary>Activate user account.</summary>
    /// <param name="id">Identification code of the user to be activated.</param>
    [HttpPut("activate/{id}", Name = "ActivateUser")]
    [Authorize(Roles = "Administrator")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult> ActivateAccountAsync([FromRoute] string id)
    {
        var user = await _userRep.GetByIdAsync(id);
        if (user == null)
            return NotFound();

        if (user.State == UserState.Active)
            return StatusCode(429, new { Message = string.Format("User all ready  activeted.", user.FirstName, user.ID) });

        await _userRep.ActivateAccountAsync(id);

        return Ok(id);
    }

    /// <summary>Deactivate a user account based on identification code.</summary>
    /// <param name="id">Identification code of the user to be deactivated.</param>
    [HttpDelete("deactivate/{id}", Name = "DeactivateUserAccount")]
    [Authorize(Roles = "Administrator")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.TooManyRequests)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult> DeactivateUserAccountAsync([FromRoute] string id)
    {
        var user = await _userRep.GetByIdAsync(id);
        if (user == null)
            return NotFound();

        if (user.State == UserState.Deactivated)
            return StatusCode(429, new { Message = string.Format("User all ready deactiveted.", user.FirstName, user.ID) });

        await _userRep.DeactivateAccountAsync(id, false);

        return Ok();
    }

    /// <summary>Validates if the email address can be assigned to a specific user.</summary>
    /// <param name="email">Email address to be validated.</param>
    /// <param name="userId">
    /// Identification code of the user for whom the validation is to be performed.
    /// This parameter should be used for editing the email of an existing user.
    /// </param>
    private async Task ValidateIfAnEmailIsInUseAsync(string email, string? userId = null)
    {
        // Checks if the email address is already being used by the user.
        var user = await _userRep.GetByEmailAsync(email);
        if (user != null && userId != user.ID)
            ModelState.AddModelError("Email", string.Format("User all ready exists with this e-mail.", email));
    }
}

