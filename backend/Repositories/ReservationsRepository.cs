using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories;

public interface IReservationsRepository
{
  Task<IEnumerable<Reservation>> GetReservationsAndUserAsync ();
  Task<Reservation?> GetReservationById (int reservationId);
  Task<Reservation?> GetReservationByDeskIdOutsideRange (int deskId, DateOnly rangeFrom, DateOnly rangeTo);
  Task<Reservation> CreateReservation(int deskId, int userId, DateOnly rangeFrom, DateOnly rangeTo);
  Task DeleteReservation(int reservationId);
  Task UpdateReservationDateFrom(int reservationId, DateOnly fromDate);
  Task UpdateReservationDateTo(int reservationId, DateOnly toDate);
  Task SplitReservation (int reservationId, DateOnly firstRangeFrom, DateOnly firstRangeTo, DateOnly secondRangeFrom, DateOnly secondRangeTo);
}

public class ReservationsRepository: IReservationsRepository
{
  private readonly AppDbContext _dbContext;

  public ReservationsRepository(AppDbContext dbContext)
  {
    this._dbContext = dbContext;
  }

  public async Task<IEnumerable<Reservation>> GetReservationsAndUserAsync()
  {
    return await _dbContext.Reservations
      .Include(reservation => reservation.User)
      .ToListAsync();
  }

  public async Task<Reservation?> GetReservationById (int reservationId) 
  {
    return await _dbContext.Reservations
      .FirstOrDefaultAsync(reservation => reservation.Id == reservationId);
  }
  
  public async Task<Reservation?> GetReservationByDeskIdOutsideRange (int deskId, DateOnly rangeFrom, DateOnly rangeTo)
  {
    return await _dbContext.Reservations
      .FirstOrDefaultAsync(reservation => reservation.DeskId == deskId &&
        reservation.FromDate <= rangeTo && reservation.ToDate >= rangeFrom
      );
  }

  public async Task<Reservation> CreateReservation(int deskId, int userId, DateOnly rangeFrom, DateOnly rangeTo)
  {
    var newReservation = new Reservation(rangeFrom, rangeTo, deskId, userId);

    await _dbContext.Reservations.AddAsync(newReservation);
    await _dbContext.SaveChangesAsync();

    return newReservation;
  }

  public async Task DeleteReservation(int reservationId)
  {
    var reservation = await this.GetReservationById(reservationId);

    if (reservation == null) return;

    _dbContext.Reservations.Remove(reservation);
    await _dbContext.SaveChangesAsync();
  }

  public async Task UpdateReservationDateFrom(int reservationId, DateOnly fromDate)
  {
    var reservation = await this.GetReservationById(reservationId);

    if (reservation == null) return;

    reservation.FromDate = fromDate;

    await _dbContext.SaveChangesAsync();
  }

  public async Task UpdateReservationDateTo(int reservationId, DateOnly toDate)
  {
    var reservation = await this.GetReservationById(reservationId);

    if (reservation == null) return;

    reservation.ToDate = toDate;

    await _dbContext.SaveChangesAsync();
  }

  public async Task SplitReservation (int reservationId, DateOnly firstRangeFrom, DateOnly firstRangeTo, DateOnly secondRangeFrom, DateOnly secondRangeTo)
  {
    await using var transaction = await _dbContext.Database.BeginTransactionAsync();

    var reservation = await this.GetReservationById(reservationId);

    await this.CreateReservation(reservation!.DeskId, reservation.UserId, firstRangeFrom, firstRangeTo);
    await this.CreateReservation(reservation!.DeskId, reservation.UserId, secondRangeFrom, secondRangeTo);

    await this.DeleteReservation(reservationId);

    await transaction.CommitAsync();
  }
}