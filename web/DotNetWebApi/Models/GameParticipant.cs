using System.ComponentModel.DataAnnotations;

namespace DotNetWebApi.Models;

public class GameParticipant
{
    public int GameParticipantId { get; set; }

    public int GameRoomId { get; set; }
    public GameRoom? GameRoom { get; set; }

    public int UserId { get; set; }
    public User? User { get; set; }

    [Required, StringLength(50)]
    public string Username { get; set; } = string.Empty;

    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
}
