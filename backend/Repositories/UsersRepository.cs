using backend.Data;
using backend.DTOs.DeskReservation;
using backend.DTOs.UserProfile;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories;

public interface IUsersRepository
{
  Task<IEnumerable<GetUserDto>> GetAllAsync();
  Task<GetUserProfileDto?> GetProfileByIdAsync(int userId);
}

public class UsersRepository: IUsersRepository
{
  private readonly AppDbContext _dbContext;

  public UsersRepository(AppDbContext dbContext)
  {
    this._dbContext = dbContext;
  }

  public async Task<IEnumerable<GetUserDto>> GetAllAsync()
  {
    return await _dbContext.Users
      .Select(user => new GetUserDto
      {
        Id = user.Id,
        Email = user.Email,
        Name = user.Name,
        Surname = user.Surname
      })
      .ToListAsync();
  }

  public async Task<GetUserProfileDto?> GetProfileByIdAsync(int userId)
  {
    var today = DateOnly.FromDateTime(DateTime.Today);

    return await _dbContext.Users
      .Where(user => user.Id == userId)
      .Select(user => new GetUserProfileDto
      {
        Name = user.Name,
        Surname = user.Surname,
        Email = user.Email,
        OngoingReservations = user.Reservations
          .Where(reservation => reservation.ToDate >= today)
          .Select(reservation => new GetUserReservationDto
          {
            Code = reservation.Desk.Code,
            FromDate = reservation.FromDate,
            ToDate = reservation.ToDate
          })
          .ToList(),
        PastReservations = user.Reservations
          .Where(reservation => reservation.ToDate < today)
          .Select(reservation => new GetUserReservationDto
          {
            Code = reservation.Desk.Code,
            FromDate = reservation.FromDate,
            ToDate = reservation.ToDate
          })
          .ToList()
      })
      .FirstOrDefaultAsync();
  }
}

