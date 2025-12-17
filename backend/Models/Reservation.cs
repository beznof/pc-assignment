using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models;

public class Reservation
{
  [Key]
  public int Id {get; private set;}

  [Required]
  public DateOnly FromDate {get; private set;}

  [Required]
  public DateOnly ToDate {get; private set;}

  [Required]
  public int DeskId {get; private set;}

  [Required]
  public int UserId {get; private set;} 

  [ForeignKey(nameof(DeskId))]
  public Desk Desk { get; private set; } = null!;

  [ForeignKey(nameof(UserId))]
  public User User { get; private set; } = null!;

  // Constructor for EF
  private Reservation() {}

  // Constructor for seeder & insertion
  public Reservation(DateOnly fromDate, DateOnly toDate, int deskId, int userId)
  {
    this.FromDate = fromDate;
    this.ToDate = toDate;
    this.DeskId = deskId;
    this.UserId = userId;
  }

}