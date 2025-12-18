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
    return await _usersRepository.GetProfileByIdAsync(userId);
  }

  public async Task<IEnumerable<GetUserDto>> GetAllUsers ()
  {
    return await _usersRepository.GetAllAsync();
  }
}