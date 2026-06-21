using System.ComponentModel.DataAnnotations;

namespace DotNetWebApi.Models;

public class Quiz
{
    public int QuizId { get; set; }

    [Required, StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }

    [StringLength(100)]
    public string? Category { get; set; }

    public bool IsPublic { get; set; } = true;

    public int CreatedByUserId { get; set; }
    public User? CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Question> Questions { get; set; } = new List<Question>();
    public ICollection<GameRoom> GameRooms { get; set; } = new List<GameRoom>();
}
