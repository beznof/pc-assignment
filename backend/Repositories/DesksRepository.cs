using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories;

public interface IDesksRepository
{
    Task<Desk?> GetDeskByIdAsync(int deskId);
    Task<IEnumerable<Desk>> GetAllDesksAsync();
}

public class DesksRepository : IDesksRepository
{
    private readonly AppDbContext _dbContext;

    public DesksRepository(AppDbContext dbContext)
    {
        this._dbContext = dbContext;
    }

    public async Task<Desk?> GetDeskByIdAsync(int deskId)
    {
        return await _dbContext.Desks.FirstOrDefaultAsync(desk => desk.Id == deskId);
    }

    public async Task<IEnumerable<Desk>> GetAllDesksAsync()
    {
        return await _dbContext.Desks
            .OrderBy(desk => desk.Code)
            .ToListAsync();
    }
}