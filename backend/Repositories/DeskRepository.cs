using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories;

public interface IDeskRepository
{
  Task<IEnumerable<Desk>> GetAllAsync();
  Task<Desk?> GetByIdAsync(int deskId);
}

public class DeskRepository: IDeskRepository
{
  private readonly AppDbContext _dbContext;

  public DeskRepository(AppDbContext dbContext)
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