using DotNetWebApi.DTOs.Room;

namespace DotNetWebApi.Services;

public interface IRoomService
{
    Task<RoomStateDto> CreateRoomAsync(int quizId, int maxPlayers, int hostUserId);
    Task<RoomStateDto?> GetRoomByCodeAsync(string code);
    Task<List<RoomDto>> GetActiveRoomsAsync();
}
