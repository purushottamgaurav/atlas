using DotNetWebApi.DTOs.Auth;

namespace DotNetWebApi.Services;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterRequest request);
    Task<AuthResponse> LoginAsync(LoginRequest request);
    Task<UserProfileDto?> GetProfileAsync(int userId);
}
