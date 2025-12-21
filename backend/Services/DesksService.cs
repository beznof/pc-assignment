using backend.DTOs.DeskReservation;
using backend.Enums.Errors;
using backend.Repositories;

namespace backend.Services;

public interface IDesksService
{
    Task<(IEnumerable<GetDeskDto>?, DeskRetrievalError?)> GetDesksAndReservations(DateOnly rangeFrom, DateOnly rangeTo,
        int userId);
}

public class DesksService : IDesksService
{
    private readonly IDesksRepository _desksRepository;
    private readonly IReservationsRepository _reservationsRepository;
    private readonly IUsersRepository _usersRepository;

    public DesksService(IDesksRepository desksRepository, IReservationsRepository reservationsRepository,
        IUsersRepository usersRepository)
    {
        this._desksRepository = desksRepository;
        this._reservationsRepository = reservationsRepository;
        this._usersRepository = usersRepository;
    }

    public async Task<(IEnumerable<GetDeskDto>?, DeskRetrievalError?)> GetDesksAndReservations(DateOnly rangeFrom,
        DateOnly rangeTo, int userId)
    {
        if (rangeFrom > rangeTo)
            return (null, DeskRetrievalError.InvalidDateRange);

        var user = await _usersRepository.GetUserByIdAsync(userId);
        if (user == null)
            return (null, DeskRetrievalError.UserNotFound);

        var today = DateOnly.FromDateTime(DateTime.Today);

        var desks = await _desksRepository.GetAllDesksAsync();
        var reservations = await _reservationsRepository.GetReservationsAndUserAsync();

        var dtos = desks
            .Select(desk =>
            {
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
                    IsCancellableToday = existingReservation != null
                        ? existingReservation.FromDate <= today && existingReservation.ToDate >= today
                        : false,
                    ReservedBy = existingReservation != null
                        ? $"{reservationOwnerUser?.Name} {reservationOwnerUser?.Surname}"
                        : null,
                    ReservationId = existingReservation != null ? existingReservation.Id : null,
                    FromRange = existingReservation != null ? existingReservation.FromDate : null,
                    ToRange = existingReservation != null ? existingReservation.ToDate : null
                };
            })
            .ToList();

        return (dtos, null);
    }
}