using System.ComponentModel.DataAnnotations;

namespace backend.Models;

public class Desk
{
  [Key]
  public int Id {get; private set; }

  [Required]
  [MaxLength(5)]
  public string Code {get; private set; } = "";

  [Required]
  public bool IsUnderMaintenance {get; private set; } = false;

  // Constructor for EF
  private Desk(){}

  // Constructor for seeder
  public Desk(string code, bool isUnderMaintenance = false)
  {
    this.Code = code;
    this.IsUnderMaintenance = isUnderMaintenance;
  }
}