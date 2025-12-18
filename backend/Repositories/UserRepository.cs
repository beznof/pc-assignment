using backend.Data;
using backend.DTOs.UserProfile;
using backend.Models;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories;

public interface IUserRepository
{
  Task<IEnumerable<User>> GetAllAsync();
  Task<User?> GetByIdAsync(int userId);
  Task<GetUserProfileDto?> GetProfileByIdAsync(int userId);
}

public class UserRepository: IUserRepository
{
  private readonly AppDbContext _dbContext;

  public UserRepository(AppDbContext dbContext)
  {
    this._dbContext = dbContext;
  }

  public async Task<IEnumerable<User>> GetAllAsync()
  {
    return await _dbContext.Users.ToListAsync();
  }

  public async Task<User?> GetByIdAsync(int userId)
  {
    return await _dbContext.Users.FirstOrDefaultAsync(user => user.Id == userId);
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

