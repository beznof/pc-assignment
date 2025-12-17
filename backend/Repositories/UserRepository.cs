using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories;

public interface IUserRepository
{
  Task<IEnumerable<User>> GetAllAsync();
  Task<User?> GetByIdAsync(int userId);
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
    return await _dbContext.Users.FirstOrDefaultAsync();
  }
}

