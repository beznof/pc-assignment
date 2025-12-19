using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models;

public class Reservation
{
    [Key] public int Id { get; private set; }

    // Date range for reservation
    [Required] public DateOnly FromDate { get; set; }
    [Required] public DateOnly ToDate { get; set; }

    // Desk FK
    [Required] public int DeskId { get; private set; }

    // User FK
    [Required] public int UserId { get; private set; }

    // Navigation properties for linked Desks and Users
    [ForeignKey(nameof(DeskId))] public Desk Desk { get; private set; } = null!;
    [ForeignKey(nameof(UserId))] public User User { get; private set; } = null!;

    // Constructor for EF
    private Reservation()
    {
    }

    // Constructor for seeder & insertion
    public Reservation(DateOnly fromDate, DateOnly toDate, int deskId, int userId)
    {
        this.FromDate = fromDate;
        this.ToDate = toDate;
        this.DeskId = deskId;
        this.UserId = userId;
    }
}