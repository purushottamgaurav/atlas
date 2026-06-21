using System.ComponentModel.DataAnnotations;

namespace DotNetWebApi.Models;

public enum RoomStatus { Waiting, Active, Completed }

public class GameRoom
{
    public int GameRoomId { get; set; }

    [Required, StringLength(10)]
    public string Code { get; set; } = string.Empty;

    public int QuizId { get; set; }
    public Quiz? Quiz { get; set; }

    public int HostUserId { get; set; }

    public RoomStatus Status { get; set; } = RoomStatus.Waiting;

    public int MaxPlayers { get; set; } = 10;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<GameParticipant> Participants { get; set; } = new List<GameParticipant>();
    public ICollection<GameScore> Scores { get; set; } = new List<GameScore>();
}
