using DotNetMvc.Models;

namespace DotNetMvc.Services;

public interface IUserProfileService
{
    Task<UserProfile?> GetProfileAsync(string userId);
    Task<UserProfile> CreateProfileAsync(UserProfile profile);
    Task<bool> ProfileExistsAsync(string userId);
}
