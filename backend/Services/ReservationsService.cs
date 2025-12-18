using backend.Enums;
using backend.Models;
using backend.Repositories;
using Microsoft.EntityFrameworkCore;

namespace backend.Services;

public interface IReservationsService
{
  Task<(Reservation?, ReservationCreationError?)> CreateReservation (int userId, int deskId, DateOnly rangeFrom, DateOnly rangeTo);
  Task<ReservationCancellationError?> CancelReservation (int reservationId, bool todayOnly);
}

public class ReservationsService: IReservationsService
{
  private readonly IReservationsRepository _reservationsRepository;
  private readonly IUsersRepository _usersRepository;
  private readonly IDesksRepository _desksRepository;

  public ReservationsService(IReservationsRepository reservationsRepository, IUsersRepository usersRepository, IDesksRepository desksRepository)
  {
    this._reservationsRepository = reservationsRepository;
    this._usersRepository = usersRepository;
    this._desksRepository = desksRepository;
  }

  public async Task<(Reservation?, ReservationCreationError?)> CreateReservation (int userId, int deskId, DateOnly rangeFrom, DateOnly rangeTo)
  {
    if (rangeTo > rangeFrom) 
      return (null, ReservationCreationError.InvalidDateRange);

    var user = await _usersRepository.GetUserByIdAsync(userId);
    if (user == null) 
      return (null, ReservationCreationError.UserNotFound);

    var desk = await _desksRepository.GetDeskByIdAsync(deskId);
    if (desk == null) 
      return (null, ReservationCreationError.DeskNotFound);
    
    var overlappingReservation = await _reservationsRepository.GetReservationByDeskIdOutsideRange(deskId, rangeFrom, rangeTo);
    if (overlappingReservation != null) 
      return (null, ReservationCreationError.DateRangeOverlap);

    try
    {
      var newReservation = await _reservationsRepository.CreateReservation(userId, deskId, rangeFrom, rangeTo);
      return (newReservation, null);
    } catch (DbUpdateException)
    {
      return (null, ReservationCreationError.InsertFailure);
    }
  }

  // public async Task<ReservationCancellationError?> CancelReservation (int reservationId, int userId, bool todayOnly = false)
  // {
  //   var reservation = await _reservationsRepository.GetReservationById(reservationId);
  //   if (reservation == null) 
  //     return ReservationCancellationError.ReservationNotFound;

  //   var user = await _usersRepository.GetUserByIdAsync(userId);
  //   if (user == null) 
  //     return ReservationCancellationError.UserNotFound;

  //   if (user.Id != reservation.UserId) 
  //     return ReservationCancellationError.UserReservationMisrelation;
  // }
}