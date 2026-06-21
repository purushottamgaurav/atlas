using DotNetWebApi.Data;
using DotNetWebApi.DTOs.Game;
using Microsoft.EntityFrameworkCore;

namespace DotNetWebApi.Services;

public class LeaderboardService : ILeaderboardService
{
    private readonly QuizDbContext _context;

    public LeaderboardService(QuizDbContext context) => _context = context;

    public async Task<List<LeaderboardEntryDto>> GetGlobalTopAsync(int count = 20)
    {
        return await _context.GameScores
            .Include(s => s.GameRoom).ThenInclude(r => r!.Quiz)
            .OrderByDescending(s => s.TotalScore)
            .Take(count)
            .Select(s => new LeaderboardEntryDto
            {
                Rank = s.Rank,
                UserId = s.UserId,
                Username = s.Username,
                TotalScore = s.TotalScore,
                CorrectAnswers = s.CorrectAnswers,
                TotalQuestions = s.TotalQuestions,
                QuizTitle = s.GameRoom!.Quiz!.Title,
                CompletedAt = s.CompletedAt
            })
            .ToListAsync();
    }

    public async Task<List<LeaderboardEntryDto>> GetQuizTopAsync(int quizId, int count = 20)
    {
        return await _context.GameScores
            .Include(s => s.GameRoom).ThenInclude(r => r!.Quiz)
            .Where(s => s.GameRoom!.QuizId == quizId)
            .OrderByDescending(s => s.TotalScore)
            .Take(count)
            .Select(s => new LeaderboardEntryDto
            {
                Rank = s.Rank,
                UserId = s.UserId,
                Username = s.Username,
                TotalScore = s.TotalScore,
                CorrectAnswers = s.CorrectAnswers,
                TotalQuestions = s.TotalQuestions,
                QuizTitle = s.GameRoom!.Quiz!.Title,
                CompletedAt = s.CompletedAt
            })
            .ToListAsync();
    }

    public async Task<List<LeaderboardEntryDto>> GetUserHistoryAsync(int userId)
    {
        return await _context.GameScores
            .Include(s => s.GameRoom).ThenInclude(r => r!.Quiz)
            .Where(s => s.UserId == userId)
            .OrderByDescending(s => s.CompletedAt)
            .Select(s => new LeaderboardEntryDto
            {
                Rank = s.Rank,
                UserId = s.UserId,
                Username = s.Username,
                TotalScore = s.TotalScore,
                CorrectAnswers = s.CorrectAnswers,
                TotalQuestions = s.TotalQuestions,
                QuizTitle = s.GameRoom!.Quiz!.Title,
                CompletedAt = s.CompletedAt
            })
            .ToListAsync();
    }
}
