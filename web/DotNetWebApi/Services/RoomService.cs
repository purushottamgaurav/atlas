using DotNetWebApi.Data;
using DotNetWebApi.DTOs.Room;
using DotNetWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DotNetWebApi.Services;

public class RoomService : IRoomService
{
    private readonly QuizDbContext _context;

    public RoomService(QuizDbContext context) => _context = context;

    public async Task<RoomStateDto> CreateRoomAsync(int quizId, int maxPlayers, int hostUserId)
    {
        var quiz = await _context.Quizzes.FindAsync(quizId)
            ?? throw new InvalidOperationException("Quiz not found.");

        var host = await _context.Users.FindAsync(hostUserId)
            ?? throw new InvalidOperationException("User not found.");

        var code = await GenerateUniqueCodeAsync();

        var room = new GameRoom
        {
            Code = code,
            QuizId = quizId,
            HostUserId = hostUserId,
            MaxPlayers = maxPlayers,
            Status = RoomStatus.Waiting
        };

        _context.GameRooms.Add(room);
        await _context.SaveChangesAsync();

        return new RoomStateDto
        {
            Code = code,
            QuizId = quizId,
            QuizTitle = quiz.Title,
            HostUsername = host.Username,
            HostUserId = hostUserId,
            Status = RoomStatus.Waiting,
            PlayerCount = 0,
            MaxPlayers = maxPlayers,
            CreatedAt = room.CreatedAt,
            Players = new List<PlayerDto>()
        };
    }

    public async Task<RoomStateDto?> GetRoomByCodeAsync(string code)
    {
        var room = await _context.GameRooms
            .Include(r => r.Quiz)
            .Include(r => r.Participants).ThenInclude(p => p.User)
            .FirstOrDefaultAsync(r => r.Code == code.ToUpper());

        if (room == null) return null;

        var host = await _context.Users.FindAsync(room.HostUserId);

        return new RoomStateDto
        {
            Code = room.Code,
            QuizId = room.QuizId,
            QuizTitle = room.Quiz?.Title ?? string.Empty,
            HostUsername = host?.Username ?? string.Empty,
            HostUserId = room.HostUserId,
            Status = room.Status,
            PlayerCount = room.Participants.Count,
            MaxPlayers = room.MaxPlayers,
            CreatedAt = room.CreatedAt,
            Players = room.Participants.Select(p => new PlayerDto
            {
                UserId = p.UserId,
                Username = p.Username,
                IsHost = p.UserId == room.HostUserId,
                JoinedAt = p.JoinedAt
            }).ToList()
        };
    }

    public async Task<List<RoomDto>> GetActiveRoomsAsync()
    {
        return await _context.GameRooms
            .Include(r => r.Quiz)
            .Include(r => r.Participants)
            .Where(r => r.Status == RoomStatus.Waiting)
            .OrderByDescending(r => r.CreatedAt)
            .Select(r => new RoomDto
            {
                Code = r.Code,
                QuizId = r.QuizId,
                QuizTitle = r.Quiz!.Title,
                HostUsername = string.Empty,
                HostUserId = r.HostUserId,
                Status = r.Status,
                PlayerCount = r.Participants.Count,
                MaxPlayers = r.MaxPlayers,
                CreatedAt = r.CreatedAt
            })
            .ToListAsync();
    }

    private async Task<string> GenerateUniqueCodeAsync()
    {
        const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
        var random = new Random();
        string code;
        do
        {
            code = new string(Enumerable.Repeat(chars, 6).Select(s => s[random.Next(s.Length)]).ToArray());
        }
        while (await _context.GameRooms.AnyAsync(r => r.Code == code));

        return code;
    }
}
