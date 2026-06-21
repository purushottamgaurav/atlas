namespace DotNetWpf.Models;

public enum RoomStatus { Waiting, Active, Completed }

public class CreateRoomRequest
{
    public int QuizId { get; set; }
    public int MaxPlayers { get; set; } = 8;
}

public class PlayerDto
{
    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public bool IsHost { get; set; }
    public DateTime JoinedAt { get; set; }
}

public class RoomDto
{
    public string Code { get; set; } = string.Empty;
    public int QuizId { get; set; }
    public string QuizTitle { get; set; } = string.Empty;
    public string HostUsername { get; set; } = string.Empty;
    public int HostUserId { get; set; }
    public RoomStatus Status { get; set; }
    public int PlayerCount { get; set; }
    public int MaxPlayers { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class RoomStateDto : RoomDto
{
    public List<PlayerDto> Players { get; set; } = new();
}
