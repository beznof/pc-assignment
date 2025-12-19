namespace backend.Enums.Errors;

public enum ReservationCreationError
{
    DeskNotFound,
    UserNotFound,
    DateRangeOverlap,
    InsertFailure,
    InvalidDateRange,
    DeskIsUnderMaintenance,
    PastReservation
}