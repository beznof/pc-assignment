using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models;

public class Desk
{
  [Key]
  public int Id {get; private set; }

  [Required]
  [MaxLength(5)]
  public string Code {get; private set; } = "";

  // Indicator for whether desk is under maintenance (unavailable)
  [Required]
  public bool IsUnderMaintenance {get; private set; } = false;

  // Navigation property for linked Reservations
  [InverseProperty("Desk")]
  public List<Reservation> Reservations {get; private set;} = new List<Reservation>();

  // Constructor for EF
  private Desk(){}

  // Constructor for seeder
  public Desk(string code, bool isUnderMaintenance = false)
  {
    this.Code = code;
    this.IsUnderMaintenance = isUnderMaintenance;
  }
}