namespace DotNetWebApi.DTOs.Game;

public class LeaderboardEntryDto
{
    public int Rank { get; set; }
    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public int TotalScore { get; set; }
    public int CorrectAnswers { get; set; }
    public int TotalQuestions { get; set; }
    public string? QuizTitle { get; set; }
    public DateTime CompletedAt { get; set; }
}
