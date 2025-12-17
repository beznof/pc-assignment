using backend.Models;

namespace backend.Data;

public static class DbSeeder
{
  public static void Seed(AppDbContext dbContext)
  {
    dbContext.Desks.AddRange(
      new Desk("A1"),
      new Desk("B1"),
      new Desk("C1"),
      new Desk("A2"),
      new Desk("B2"),
      new Desk("A3")
    );

    dbContext.SaveChanges();
  }
}