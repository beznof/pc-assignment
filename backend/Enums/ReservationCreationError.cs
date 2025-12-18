
namespace backend.Enums;

public enum ReservationCreationError
{
  DeskNotFound,
  UserNotFound,
  DateRangeOverlap,
  InsertFailure,
  InvalidDateRange
}