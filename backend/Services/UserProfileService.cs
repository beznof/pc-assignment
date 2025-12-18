using backend.DTOs.DeskReservation;
using backend.DTOs.UserProfile;
using backend.Repositories;

namespace backend.Services;

public interface IUserProfileService
{
  Task<GetUserProfileDto?> GetUserProfile (int userId);
  Task<IEnumerable<GetUserDto>> GetAllUsers ();
}

public class UserProfileService: IUserProfileService
{
  private readonly IUserRepository _userRepository;

  public UserProfileService(IUserRepository userRepository)
  {
    this._userRepository = userRepository;
  }

  public async Task<GetUserProfileDto?> GetUserProfile (int userId)
  {
    return await _userRepository.GetProfileByIdAsync(userId);
  }

  public async Task<IEnumerable<GetUserDto>> GetAllUsers ()
  {
    return await _userRepository.GetAllAsync();
  }
}