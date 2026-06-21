namespace DotNetWpf.Services;

// Singleton that holds the current user's session after login.
public class SessionStore
{
    public string Token { get; set; } = string.Empty;
    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public bool IsLoggedIn => !string.IsNullOrEmpty(Token);

    public void Clear()
    {
        Token = string.Empty;
        UserId = 0;
        Username = string.Empty;
        Email = string.Empty;
    }
}
