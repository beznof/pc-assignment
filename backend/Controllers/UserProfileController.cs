using System.ComponentModel;
using System.Net.Mime;
using backend.DTOs.UserProfile;
using backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[ApiController]
[Route("api/profiles")]
[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
[Tags("Profile")]
public class UserProfileController : ControllerBase
{
  private readonly UsersService _usersService;

  public UserProfileController(UsersService usersService)
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
  [HttpGet("{userId}")]
  [ProducesResponseType(typeof(GetUserProfileDto), StatusCodes.Status200OK)]
  [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
  [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
  public async Task<ActionResult<GetUserProfileDto>> GetProfile([FromRoute] int userId)
  {
    var userProfile = await _usersService.GetUserProfile(userId);

    if(userProfile == null)
    {
      return StatusCode(404, new ProblemDetails
      {
        Title = "User not found",
        Status = StatusCodes.Status404NotFound,
      });
    }

    return userProfile;
  }
}
