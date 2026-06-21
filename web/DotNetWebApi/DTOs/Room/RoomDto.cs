using DotNetWebApi.Models;

namespace DotNetWebApi.DTOs.Room;

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
