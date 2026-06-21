namespace DotNetWpf.Models;

public class AnswerOptionDto
{
    public int AnswerId { get; set; }
    public string Text { get; set; } = string.Empty;
}

public class GameQuestionDto
{
    public int QuestionId { get; set; }
    public string Text { get; set; } = string.Empty;
    public int TimeLimitSeconds { get; set; }
    public int Points { get; set; }
    public int QuestionNumber { get; set; }
    public int TotalQuestions { get; set; }
    public List<AnswerOptionDto> Answers { get; set; } = new();
}

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

public class LeaderboardEntryDto
{
    public int Rank { get; set; }
    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public int TotalScore { get; set; }
    public int CorrectAnswers { get; set; }
    public int TotalQuestions { get; set; }
}

public class GameResultDto
{
    public string RoomCode { get; set; } = string.Empty;
    public string QuizTitle { get; set; } = string.Empty;
    public List<LeaderboardEntryDto> Leaderboard { get; set; } = new();
}

public class AnswerResultDto
{
    public bool Success { get; set; }
    public int PointsEarned { get; set; }
    public string? Message { get; set; }
}
