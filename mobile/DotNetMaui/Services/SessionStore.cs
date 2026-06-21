namespace DotNetMaui.Services;

public class SessionStore
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
