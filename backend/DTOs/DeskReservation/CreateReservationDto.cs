
namespace backend.DTOs.DeskReservation;

public class CreateReservationDto
{
  public int UserId {get; set;}
  public int DeskId {get; set;}
  public DateOnly DateFrom {get; set;}
  public DateOnly DateTo {get; set;}
}