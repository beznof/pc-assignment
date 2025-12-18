using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories;

public interface IUsersRepository
{
  Task<User?> GetUserByIdAsync(int userId);
  Task<IEnumerable<User>> GetAllUsersAsync();
  Task<User?> GetUserAndReservationsByIdAsync(int userId);
}

public class UsersRepository: IUsersRepository
{
  private readonly AppDbContext _dbContext;

  public UsersRepository(AppDbContext dbContext)
  {
    this._dbContext = dbContext;
  }

  public async Task<User?> GetUserByIdAsync(int userId)
  {
    return await _dbContext.Users.FirstOrDefaultAsync(user => user.Id == userId);
  }

  public async Task<IEnumerable<User>> GetAllUsersAsync()
  {
    return await _dbContext.Users.ToListAsync();
  }

  public async Task<User?> GetUserAndReservationsByIdAsync(int userId)
  {
    return await _dbContext.Users
      .Where(user => user.Id == userId)
      .Include(user => user.Reservations)
      .FirstOrDefaultAsync();
  }
}

