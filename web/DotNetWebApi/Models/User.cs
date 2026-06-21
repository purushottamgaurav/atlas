using System.ComponentModel.DataAnnotations;

namespace DotNetWebApi.Models;

public class User
{
    public int UserId { get; set; }

    [Required, StringLength(50)]
    public string Username { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(200)]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();
    public ICollection<GameParticipant> Participations { get; set; } = new List<GameParticipant>();
}
