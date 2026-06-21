namespace DotNetWebApi.DTOs.Room;

public class PlayerDto
{
    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public bool IsHost { get; set; }
    public DateTime JoinedAt { get; set; }
}
