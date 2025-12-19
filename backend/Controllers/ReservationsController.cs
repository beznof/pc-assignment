using System.Net.Mime;
using backend.DTOs.DeskReservation;
using backend.Enums.Errors;
using backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace backend.Controllers;

[ApiController]
[Route("api/reservations")]
[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
[Tags("Desk Reservations")]
public class ReservationsController: ControllerBase
{
  private readonly IReservationsService _reservationsService;

  public ReservationsController(IReservationsService reservationsService)
  {
    this._reservationsService = reservationsService;
  }

  /// <summary>
  /// Creates a reservation
  /// </summary>
  /// <param name="body">User's and desk's identifiers, reservation's date start and end</param>
  /// <returns>Created reservation</returns>
  /// <response code="201">Returns created reservation</response>
  /// <response code="400">Invalid date range or payload</response>
  /// <response code="404">User or desk not found</response>
  /// <response code="409">Reservation overlaps with existing one or desk is under maintenance</response>
  /// <response code="500">Creation failed</response>
  [HttpPost]
  [ProducesResponseType(typeof(GetCreatedReservationDto), StatusCodes.Status201Created)]
  [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
  [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
  [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
  [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
  public async Task<ActionResult<GetCreatedReservationDto>> CreateReservation ([FromBody] CreateReservationDto body)
  {
    var (reservation, error) = await _reservationsService.CreateReservation(body.UserId, body.DeskId, body.DateFrom, body.DateTo);

    switch (error)
    {
      case ReservationCreationError.DeskNotFound:
        return StatusCode(404, new ProblemDetails
        {
          Title = "Desk not found",
        });
      case ReservationCreationError.UserNotFound:
        return StatusCode(404, new ProblemDetails
        {
          Title = "User not found",
        });
      case ReservationCreationError.PastReservation:
        return StatusCode(400, new ProblemDetails
        {
          Title = "Cannot create a reservation in the past",
        });
      case ReservationCreationError.DateRangeOverlap:
        return StatusCode(409, new ProblemDetails
        {
          Title = "Selected date range overlaps with existing reservation",
        });
      case ReservationCreationError.DeskIsUnderMaintenance:
        return StatusCode(statusCode: 409, new ProblemDetails
        {
          Title = "Desk is under maintenance",
        });
      case ReservationCreationError.InvalidDateRange:
        return StatusCode(400, new ProblemDetails
        {
          Title = "Reservation must not end before its start",
        });
      case ReservationCreationError.InsertFailure:
        return StatusCode(statusCode: 500, new ProblemDetails
        {
          Title = "Creation of reservation failed",
        });
    }

    return StatusCode(201, new GetCreatedReservationDto
    {
      Id = reservation!.Id,
      FromDate = reservation.FromDate,
      ToDate = reservation.ToDate
    });
  }

  /// <summary>
  /// Deletes the user's specified reservation
  /// </summary>
  /// <param name="userId">User's identifier</param>
  /// <param name="reservationId">Reservation's identifier</param>
  /// <param name="todayOnly">Flag indicating whether the reservation should be deleted for today only</param>
  /// <response code="204">Reservation deleted scuccesfully</response>
  /// <response code="400">Reservation has already concluded or wrong payload</response>
  /// <response code="403">Reservation doesn't belong to the user</response>
  /// <response code="404">User or reservation not found</response>
  /// <response code="500">Deletion failed</response>
  [HttpDelete("{reservationId}")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
  [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
  [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
  public async Task<IActionResult> DeleteReservation ([FromQuery] int userId, [FromRoute] int reservationId, [FromQuery] bool todayOnly)
  {
    var error = await _reservationsService.CancelReservation(reservationId, userId, todayOnly);

    switch (error)
    {
      case ReservationCancellationError.UserNotFound:
        return StatusCode(404, new ProblemDetails
        {
          Title = "User not found",
        });
      case ReservationCancellationError.ReservationNotFound:
        return StatusCode(404, new ProblemDetails
        {
          Title = "Reservation not found",
        });
      case ReservationCancellationError.UserReservationMisrelation:
        return StatusCode(403, new ProblemDetails
          {
            Title = "User doesn't own the given reservation",
          });
      case ReservationCancellationError.PastReservation:
        return StatusCode(400, new ProblemDetails
        {
          Title = "Cannot cancel concluded reservations",
        });
      case ReservationCancellationError.DeleteFailure:
        return StatusCode(500, new ProblemDetails
        {
          Title = "Deletion failed",
        });
    }

    return StatusCode(204);
  }
}