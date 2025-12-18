using backend.Data;
using backend.DTOs.DeskReservation;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories;

public interface IReservationsRepository
{
  Task<IEnumerable<Reservation>> GetReservationsAndUserAsync ();
  Task<Reservation?> GetReservationById (int reservationId);
  Task<Reservation?> GetReservationByDeskIdOutsideRange (int deskId, DateOnly rangeFrom, DateOnly rangeTo);
  Task<Reservation> CreateReservation(int deskId, int userId, DateOnly rangeFrom, DateOnly rangeTo);
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
}