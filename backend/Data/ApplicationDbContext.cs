using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Data;

public class AppDbContext : DbContext
{
    public DbSet<Desk> Desks { get; private set; }
    public DbSet<User> Users { get; private set; }
    public DbSet<Reservation> Reservations { get; private set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase("DesksDB");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Unique constraint for Desk's code
        modelBuilder.Entity<Desk>()
            .HasIndex(desk => desk.Code)
            .IsUnique();

        // Unique constraint for User's email
        modelBuilder.Entity<User>()
            .HasIndex(user => user.Email)
            .IsUnique();
    }
}