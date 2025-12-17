using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Data;

public class AppDbContext: DbContext
{
  public DbSet<Desk> Desks {get; private set;}
  public DbSet<User> Users {get; private set;}

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    optionsBuilder.UseInMemoryDatabase("DesksDB");
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<Desk>()
      .HasIndex(desk => desk.Code)
      .IsUnique();

    modelBuilder.Entity<User>()
      .HasIndex(user => user.Email)
      .IsUnique();
  }
}