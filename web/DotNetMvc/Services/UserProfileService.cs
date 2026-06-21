using DotNetMvc.Data;
using DotNetMvc.Models;
using Microsoft.EntityFrameworkCore;

namespace DotNetMvc.Services;

public class UserProfileService : IUserProfileService
{
    private readonly ApplicationDbContext _context;

    public UserProfileService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<UserProfile?> GetProfileAsync(string userId) =>
        await _context.UserProfiles.FirstOrDefaultAsync(u => u.UserId == userId);

    public async Task<UserProfile> CreateProfileAsync(UserProfile profile)
    {
        _context.UserProfiles.Add(profile);
        await _context.SaveChangesAsync();
        return profile;
    }

    public async Task<bool> ProfileExistsAsync(string userId) =>
        await _context.UserProfiles.AnyAsync(u => u.UserId == userId);
}
