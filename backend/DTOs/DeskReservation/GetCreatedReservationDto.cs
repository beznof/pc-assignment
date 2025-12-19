namespace backend.DTOs.DeskReservation;

public class GetCreatedReservationDto
{
    public int Id { get; set; }
    public DateOnly FromDate { get; set; }
    public DateOnly ToDate { get; set; }
}