using System.Net.Mime;
using backend.DTOs.DeskReservation;
using backend.Services;
using Microsoft.AspNetCore.Mvc;
using backend.Enums.Errors;

namespace backend.Controllers;

[ApiController]
[Route("api/desks")]
[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
[Tags("Desks")]
public class DeskController : ControllerBase
{
    private readonly IDesksService _desksService;

    public DeskController(IDesksService desksService)
    {
        this._desksService = desksService;
    }

    /// <summary>
    /// Retrieves the list of desks and corresponding current reservations (if they exist) from user's perspective
    /// </summary>
    /// <param name="userId">User's identifier</param>
    /// <param name="rangeFrom">Date range from</param>
    /// <param name="rangeTo">Date range to</param>
    /// <returns>List of desks and reservations</returns>
    /// <response code="200">Returns desks and their reservations</response>
    /// <response code="400">Invalid date range or wrong payload</response>
    /// <response code="404">User not found</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<GetDeskDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<GetDeskDto>>> GetDesksAndReservations([FromQuery] int userId,
        [FromQuery] DateOnly rangeFrom, [FromQuery] DateOnly rangeTo)
    {
        var (desksAndReservations, error) = await _desksService.GetDesksAndReservations(rangeFrom, rangeTo, userId);

        switch (error)
        {
            case DeskRetrievalError.UserNotFound:
                return StatusCode(404, new ProblemDetails
                {
                    Title = "User not found",
                });
            case DeskRetrievalError.InvalidDateRange:
                return StatusCode(400, new ProblemDetails
                {
                    Title = "Reservation must not end before its start",
                });
        }

        return StatusCode(200, desksAndReservations);
    }
}