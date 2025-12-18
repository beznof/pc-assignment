using backend.DTOs.DeskReservation;
using backend.DTOs.UserProfile;
using backend.Repositories;

namespace backend.Services;

public interface IUsersService
{
  Task<GetUserProfileDto?> GetUserProfile (int userId);
  Task<IEnumerable<GetUserDto>> GetAllUsers ();
}

public class UsersService: IUsersService
{
  private readonly IUsersRepository _usersRepository;

  public UsersService(IUsersRepository usersRepository)
  {
    this._usersRepository = usersRepository;
  }

  public async Task<GetUserProfileDto?> GetUserProfile (int userId)
  {
    var user =  await _usersRepository.GetUserAndReservationsByIdAsync(userId);

    if (user == null)
    {
      return null;
    }

    var today = DateOnly.FromDateTime(DateTime.Today);

    return new GetUserProfileDto
    {
      Name = user.Name,
      Surname = user.Surname,
      Email = user.Email,
      OngoingReservations = user.Reservations
      .Where(reservation => reservation.ToDate >= today)
        .Select(reservation => new GetUserReservationDto
        {
          Code = reservation.Desk.Code,
          FromDate = reservation.FromDate,
          ToDate = reservation.ToDate
        })
        .ToList(),
      PastReservations = user.Reservations
        .Where(reservation => reservation.ToDate < today)
        .Select(reservation => new GetUserReservationDto
        {
          Code = reservation.Desk.Code,
          FromDate = reservation.FromDate,
          ToDate = reservation.ToDate
        })
        .ToList()
    };
  }

  public async Task<IEnumerable<GetUserDto>> GetAllUsers ()
  {
    var users = await _usersRepository.GetAllUsersAsync();

    return users
      .Select(user => new GetUserDto
      {
        Id = user.Id,
        Email = user.Email,
        Name = user.Name,
        Surname = user.Surname
      })
      .ToList();
  }
}