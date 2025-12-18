using backend.DTOs.DeskReservation;
using backend.Models;
using backend.Repositories;

namespace backend.Services;

public interface IDesksService
{
  Task<IEnumerable<GetDeskDto>> GetDesksAndReservations (DateOnly rangeFrom, DateOnly rangeTo, int userId);
}

public class DesksService: IDesksService
{
  private readonly IDesksRepository _desksRepository;
  private readonly IReservationsRepository _reservationsRepository;

  public DesksService (IDesksRepository desksRepository, IReservationsRepository reservationsRepository)
  {
    this._desksRepository = desksRepository;
    this._reservationsRepository = reservationsRepository;
  }

  public async Task<IEnumerable<GetDeskDto>> GetDesksAndReservations (DateOnly rangeFrom, DateOnly rangeTo, int userId)
  {
    var desks = await _desksRepository.GetAllDesksAsync();
    var reservations = await _reservationsRepository.GetReservationsAndUserAsync();

    return desks
      .Select(desk => {
        var existingReservation = reservations
          .FirstOrDefault(reservation => 
            reservation.DeskId == desk.Id &&
            reservation.FromDate <= rangeTo && reservation.ToDate >= rangeFrom
          );

        var reservationOwnerUser = existingReservation != null ? existingReservation.User : null;
        
        return new GetDeskDto
          {
            DeskId = desk.Id,
            DeskCode = desk.Code,
            IsDeskUnderMaintenance = desk.IsUnderMaintenance,
            IsDeskReserved = existingReservation != null,
            IsReservedByCurrentUser = reservationOwnerUser != null ? reservationOwnerUser.Id == userId : false,
            ReservedBy = existingReservation != null ? $"{reservationOwnerUser?.Name} {reservationOwnerUser?.Surname} ({reservationOwnerUser?.Email})" : null,
            ReservationId = existingReservation != null ? existingReservation.Id : null
          };
      })
      .ToList();
  }
}