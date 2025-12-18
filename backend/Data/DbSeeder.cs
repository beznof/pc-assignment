using backend.Models;

namespace backend.Data;

public static class DbSeeder
{
  public static void Seed(AppDbContext dbContext)
  {
    // Desk seeding
    dbContext.Desks.AddRange(
      new Desk("A1"),
      new Desk("B1", true),
      new Desk("C1"),
      new Desk("A2"),
      new Desk("B2"),
      new Desk("A3", true)
    );

    // User seeding
    dbContext.Users.AddRange(
      new User("jjuozas@example.com", "Juozas", "Juozaitis"),
      new User("aauksyte@example.com", "Auksė", "Auksytė"),
      new User("ggustaitis@example.com", "Gustas", "Gustaitis")
    );

    var todaysDate = DateOnly.FromDateTime(DateTime.Today);

    // Reservation seeding
    dbContext.Reservations.AddRange(
      new Reservation(todaysDate.AddDays(-2), todaysDate.AddDays(1), 1, 1),
      new Reservation(todaysDate, todaysDate.AddDays(5), 3, 1),
      new Reservation(todaysDate.AddDays(-14), todaysDate, 4, 2),
      new Reservation(todaysDate.AddDays(-3), todaysDate.AddDays(3), 5, 2)
    );

    dbContext.SaveChanges();
  }
}