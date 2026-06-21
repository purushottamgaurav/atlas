namespace DotNetBlazor.Services;
using DotNetBlazor.Models;

public class SessionService
{
    public string Token { get; set; } = string.Empty;
    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string CurrentRoomCode { get; set; } = string.Empty;
    public bool IsRoomHost { get; set; }
    public GameResultDto? LastGameResult { get; set; }
    public GameQuestionDto? CurrentQuestion { get; set; }

    public bool IsLoggedIn => !string.IsNullOrEmpty(Token);

    public void SetUser(AuthResponse r)
    {
        Token = r.Token;
        UserId = r.UserId;
        Username = r.Username;
        Email = r.Email;
    }

    public void Clear()
    {
        Token = string.Empty;
        UserId = 0;
        Username = string.Empty;
        Email = string.Empty;
        CurrentRoomCode = string.Empty;
        IsRoomHost = false;
        LastGameResult = null;
        CurrentQuestion = null;
    }
}
