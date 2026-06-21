using System.ComponentModel.DataAnnotations;

namespace DotNetWebApi.Models;

public class GameScore
{
    public int GameScoreId { get; set; }

    public int GameRoomId { get; set; }
    public GameRoom? GameRoom { get; set; }

    public int UserId { get; set; }

    [Required, StringLength(50)]
    public string Username { get; set; } = string.Empty;

    public int TotalScore { get; set; }

    public int CorrectAnswers { get; set; }

    public int TotalQuestions { get; set; }

    public int Rank { get; set; }

    public DateTime CompletedAt { get; set; } = DateTime.UtcNow;
}
