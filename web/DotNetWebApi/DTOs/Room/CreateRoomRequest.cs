using System.ComponentModel.DataAnnotations;

namespace DotNetWebApi.DTOs.Room;

public class CreateRoomRequest
{
    [Required]
    public int QuizId { get; set; }

    [Range(2, 50)]
    public int MaxPlayers { get; set; } = 10;
}
