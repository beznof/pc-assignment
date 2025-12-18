using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories;

public interface IReservationsRepository
{
  Task<IEnumerable<Reservation>> GetAllAsync();
  Task<Reservation?> GetByIdAsync(int reservationId);
}

public class ReservationsRepository: IReservationsRepository
{
  private readonly AppDbContext _dbContext;

  public ReservationsRepository(AppDbContext dbContext)
  {
    this._dbContext = dbContext;
  }

  public async Task<IEnumerable<Reservation>> GetAllAsync()
  {
    return await _dbContext.Reservations.ToListAsync();
  }

  public async Task<Reservation?> GetByIdAsync(int reservationId)
  {
    return await _dbContext.Reservations.FirstOrDefaultAsync(reservation => reservation.Id == reservationId);
  }
}