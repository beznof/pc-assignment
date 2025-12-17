using System.ComponentModel.DataAnnotations;

namespace backend.Models;

public class Desk
{
  [Key]
  public int Id {get; private set; }

  [Required]
  [MaxLength(5)]
  public string Code {get; private set; } = "";

  private Desk(){}

  public Desk(string code)
  {
    this.Code = code;
  }
}