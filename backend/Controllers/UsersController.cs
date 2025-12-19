using System.Net.Mime;
using backend.DTOs.DeskReservation;
using backend.DTOs.UserProfile;
using backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[ApiController]
[Route("api/users")]
[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
[Tags("Users")]
public class UsersController : ControllerBase
{
    private readonly IUsersService _usersService;

    public UsersController(IUsersService usersService)
    {
        this._usersService = usersService;
    }

    /// <summary>
    /// Retrieves user's profile data
    /// </summary>
    /// <param name="userId">User's identifier</param>
    /// <returns>User's information, current and past reservations</returns>
    /// <response code="200">Returns user's profile data</response>
    /// <response code="404">User not found</response>
    [HttpGet("profile/{userId}")]
    [ProducesResponseType(typeof(GetUserProfileDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<GetUserProfileDto>> GetProfile([FromRoute] int userId)
    {
        var userProfile = await _usersService.GetUserProfile(userId);

        if (userProfile == null)
        {
            return StatusCode(404, new ProblemDetails
            {
                Title = "User not found",
            });
        }

        return StatusCode(200, userProfile);
    }

    /// <summary>
    /// Retrieves the list of users
    /// </summary>
    /// <returns>List of users</returns>
    /// <response code="200">Returns the list of users</response>
    [HttpGet("all")]
    [ProducesResponseType(typeof(IEnumerable<GetUserDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<GetUserDto>>> GetAllUsers()
    {
        var users = await _usersService.GetAllUsers();

        return StatusCode(200, users);
    }
}