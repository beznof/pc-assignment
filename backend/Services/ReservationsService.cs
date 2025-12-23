using backend.Enums.Errors;
using backend.Models;
using backend.Repositories;
using Microsoft.EntityFrameworkCore;

namespace backend.Services;

public interface IReservationsService
{
    Task<(Reservation?, ReservationCreationError?)> CreateReservation(int userId, int deskId, DateOnly rangeFrom,
        DateOnly rangeTo);

    Task<ReservationCancellationError?> CancelReservation(int reservationId, int userId, bool todayOnly = false);
}

public class ReservationsService : IReservationsService
{
    private readonly IReservationsRepository _reservationsRepository;
    private readonly IUsersRepository _usersRepository;
    private readonly IDesksRepository _desksRepository;

    public ReservationsService(IReservationsRepository reservationsRepository, IUsersRepository usersRepository,
        IDesksRepository desksRepository)
    {
        this._reservationsRepository = reservationsRepository;
        this._usersRepository = usersRepository;
        this._desksRepository = desksRepository;
    }

    public async Task<(Reservation?, ReservationCreationError?)> CreateReservation(int userId, int deskId,
        DateOnly rangeFrom, DateOnly rangeTo)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        if (rangeFrom < today)
            return (null, ReservationCreationError.PastReservation);

        if (rangeTo < rangeFrom)
            return (null, ReservationCreationError.InvalidDateRange);

        var user = await _usersRepository.GetUserByIdAsync(userId);
        if (user == null)
            return (null, ReservationCreationError.UserNotFound);

        var desk = await _desksRepository.GetDeskByIdAsync(deskId);
        if (desk == null)
            return (null, ReservationCreationError.DeskNotFound);

        if (desk.IsUnderMaintenance)
            return (null, ReservationCreationError.DeskIsUnderMaintenance);

        var overlappingReservation =
            await _reservationsRepository.GetReservationByDeskIdOutsideRange(deskId, rangeFrom, rangeTo);
        if (overlappingReservation != null)
            return (null, ReservationCreationError.DateRangeOverlap);

        try
        {
            var newReservation = await _reservationsRepository.CreateReservation(deskId, userId, rangeFrom, rangeTo);
            return (newReservation, null);
        }
        catch (DbUpdateException)
        {
            return (null, ReservationCreationError.InsertFailure);
        }
    }

    public async Task<ReservationCancellationError?> CancelReservation(int reservationId, int userId,
        bool todayOnly = false)
    {
        var reservation = await _reservationsRepository.GetReservationById(reservationId);
        if (reservation == null)
            return ReservationCancellationError.ReservationNotFound;

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        if (reservation.ToDate < today)
            return ReservationCancellationError.PastReservation;

        var user = await _usersRepository.GetUserByIdAsync(userId);
        if (user == null)
            return ReservationCancellationError.UserNotFound;

        if (user.Id != reservation.UserId)
            return ReservationCancellationError.UserReservationMisrelation;

        try
        {
            if (!todayOnly || (reservation.FromDate == reservation.ToDate))
            {
                await _reservationsRepository.DeleteReservation(reservationId);
            }
            else
            {
                if (today == reservation.FromDate)
                {
                    await _reservationsRepository.UpdateReservationDateFrom(reservation.Id, today.AddDays(1));
                }
                else if (today == reservation.ToDate)
                {
                    await _reservationsRepository.UpdateReservationDateTo(reservation.Id, today.AddDays(-1));
                }
                else
                {
                    var firstRangeFrom = reservation.FromDate;
                    var firstRangeTo = today.AddDays(-1);

                    var secondRangeFrom = today.AddDays(1);
                    var secondRangeTo = reservation.ToDate;

                    await _reservationsRepository.SplitReservation(reservationId, firstRangeFrom, firstRangeTo,
                        secondRangeFrom, secondRangeTo);
                }
            }

            return null;
        }
        catch (DbUpdateException)
        {
            return ReservationCancellationError.DeleteFailure;
        }
    }
}