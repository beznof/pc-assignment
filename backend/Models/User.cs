using System.ComponentModel.DataAnnotations;

namespace backend.Models;

public class User
{
  [Key]
  public int Id {get; private set;}

  [Required]
  [MaxLength(100)]
  public string Email {get; private set;} = "";

  [Required]
  [MaxLength(30)]
  public string Name {get; private set;} = "";

  [Required]
  [MaxLength(30)]
  public string Surname {get; private set;} = "";

  // Constructor for EF
  private User(){}

  // Constructor for seeder
  public User(string email, string name, string surname)
  {
    this.Email = email;
    this.Name = name;
    this.Surname = surname;
  }
}