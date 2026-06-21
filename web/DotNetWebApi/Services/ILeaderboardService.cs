using DotNetWebApi.DTOs.Game;

namespace DotNetWebApi.Services;

public interface ILeaderboardService
{
    Task<List<LeaderboardEntryDto>> GetGlobalTopAsync(int count = 20);
    Task<List<LeaderboardEntryDto>> GetQuizTopAsync(int quizId, int count = 20);
    Task<List<LeaderboardEntryDto>> GetUserHistoryAsync(int userId);
}
