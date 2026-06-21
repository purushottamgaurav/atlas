namespace DotNetWebApi.DTOs.Game;

public class PlayerScoreDto
{
    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public int RoundPoints { get; set; }
    public int TotalScore { get; set; }
    public bool AnsweredCorrectly { get; set; }
}

public class QuestionResultDto
{
    public int QuestionId { get; set; }
    public int CorrectAnswerId { get; set; }
    public string CorrectAnswerText { get; set; } = string.Empty;
    public List<PlayerScoreDto> PlayerScores { get; set; } = new();
}

public class GameResultDto
{
    public string RoomCode { get; set; } = string.Empty;
    public string QuizTitle { get; set; } = string.Empty;
    public List<LeaderboardEntryDto> Leaderboard { get; set; } = new();
}
