namespace backend.DTOs.UserProfile;

public class GetUserProfileDto
{
    public string Name { get; set; } = "";
    public string Surname { get; set; } = "";
    public string Email { get; set; } = "";
    public List<GetUserReservationDto> OngoingReservations { get; set; } = new List<GetUserReservationDto>();
    public List<GetUserReservationDto> PastReservations { get; set; } = new List<GetUserReservationDto>();
}