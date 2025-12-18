using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories;

public interface IDesksRepository
{
  Task<IEnumerable<Desk>> GetAllAsync();
  Task<Desk?> GetByIdAsync(int deskId);
}

public class DesksRepository: IDesksRepository
{
  private readonly AppDbContext _dbContext;

  public DesksRepository(AppDbContext dbContext)
  {
    this._dbContext = dbContext;
  }

  public async Task<IEnumerable<Desk>> GetAllAsync()
  {
    return await _dbContext.Desks.ToListAsync();
  }

  public async Task<Desk?> GetByIdAsync(int deskId)
  {
    return await _dbContext.Desks.FirstOrDefaultAsync(desk => desk.Id == deskId);
  }
}