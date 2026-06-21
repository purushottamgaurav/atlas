namespace DotNetBlazor.Models;

public class LoginRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public LoginRequest() { }
    public LoginRequest(string email, string password)
    {
        Email = email;
        Password = password;
    }
}

public class RegisterRequest
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public RegisterRequest() { }
    public RegisterRequest(string username, string email, string password)
    {
        Username = username;
        Email = email;
        Password = password;
    }
}

public class AuthResponse
{
    public string Token { get; set; } = string.Empty;
    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
}
