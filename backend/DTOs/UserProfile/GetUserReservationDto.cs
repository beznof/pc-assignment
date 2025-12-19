namespace backend.DTOs.UserProfile;

public class GetUserReservationDto
{
    public string Code { get; set; } = "";
    public DateOnly FromDate { get; set; }
    public DateOnly ToDate { get; set; }
}