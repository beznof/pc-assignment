namespace backend.DTOs.DeskReservation;

public class GetDeskDto
{
    public int DeskId { get; set; }
    public string DeskCode { get; set; } = "";
    public bool IsDeskUnderMaintenance { get; set; }
    public bool IsDeskReserved { get; set; }
    public bool IsReservedByCurrentUser { get; set; }
    public bool IsCancellableToday { get; set; }
    public bool IsCancellable { get; set; }
    public string? ReservedBy { get; set; }
    public int? ReservationId { get; set; }
    public DateOnly? FromRange { get; set; }
    public DateOnly? ToRange { get; set; }
}