using DotNetWebApi.Data;
using DotNetWebApi.DTOs.Question;
using DotNetWebApi.DTOs.Quiz;
using DotNetWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DotNetWebApi.Services;

public class QuizService : IQuizService
{
    private readonly QuizDbContext _context;

    public QuizService(QuizDbContext context) => _context = context;

    public async Task<List<QuizSummaryDto>> GetAllAsync(int userId)
    {
        return await _context.Quizzes
            .Include(q => q.CreatedBy)
            .Include(q => q.Questions)
            .Where(q => q.IsPublic || q.CreatedByUserId == userId)
            .OrderByDescending(q => q.CreatedAt)
            .Select(q => new QuizSummaryDto
            {
                QuizId = q.QuizId,
                Title = q.Title,
                Description = q.Description,
                Category = q.Category,
                IsPublic = q.IsPublic,
                CreatedBy = q.CreatedBy!.Username,
                QuestionCount = q.Questions.Count,
                CreatedAt = q.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<QuizDto?> GetByIdAsync(int quizId)
    {
        var quiz = await _context.Quizzes
            .Include(q => q.CreatedBy)
            .Include(q => q.Questions)
                .ThenInclude(q => q.Answers)
            .FirstOrDefaultAsync(q => q.QuizId == quizId);

        if (quiz == null) return null;

        return MapToDto(quiz);
    }

    public async Task<QuizDto> CreateAsync(CreateQuizRequest request, int userId)
    {
        var quiz = new Quiz
        {
            Title = request.Title,
            Description = request.Description,
            Category = request.Category,
            IsPublic = request.IsPublic,
            CreatedByUserId = userId
        };

        _context.Quizzes.Add(quiz);
        await _context.SaveChangesAsync();

        return (await GetByIdAsync(quiz.QuizId))!;
    }

    public async Task<QuizDto?> UpdateAsync(int quizId, CreateQuizRequest request, int userId)
    {
        var quiz = await _context.Quizzes.FindAsync(quizId);
        if (quiz == null || quiz.CreatedByUserId != userId) return null;

        quiz.Title = request.Title;
        quiz.Description = request.Description;
        quiz.Category = request.Category;
        quiz.IsPublic = request.IsPublic;

        await _context.SaveChangesAsync();
        return (await GetByIdAsync(quizId))!;
    }

    public async Task<bool> DeleteAsync(int quizId, int userId)
    {
        var quiz = await _context.Quizzes.FindAsync(quizId);
        if (quiz == null || quiz.CreatedByUserId != userId) return false;

        _context.Quizzes.Remove(quiz);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<QuestionDto> AddQuestionAsync(int quizId, CreateQuestionRequest request, int userId)
    {
        var quiz = await _context.Quizzes.FindAsync(quizId)
            ?? throw new InvalidOperationException("Quiz not found.");

        if (quiz.CreatedByUserId != userId)
            throw new UnauthorizedAccessException("Only the quiz owner can add questions.");

        var question = new Question
        {
            QuizId = quizId,
            Text = request.Text,
            TimeLimitSeconds = request.TimeLimitSeconds,
            Points = request.Points,
            OrderNumber = request.OrderNumber,
            Answers = request.Answers.Select(a => new Answer
            {
                Text = a.Text,
                IsCorrect = a.IsCorrect
            }).ToList()
        };

        _context.Questions.Add(question);
        await _context.SaveChangesAsync();

        return MapQuestionToDto(question);
    }

    public async Task<bool> DeleteQuestionAsync(int questionId, int userId)
    {
        var question = await _context.Questions
            .Include(q => q.Quiz)
            .FirstOrDefaultAsync(q => q.QuestionId == questionId);

        if (question == null || question.Quiz?.CreatedByUserId != userId) return false;

        _context.Questions.Remove(question);
        await _context.SaveChangesAsync();
        return true;
    }

    private static QuizDto MapToDto(Quiz quiz) => new()
    {
        QuizId = quiz.QuizId,
        Title = quiz.Title,
        Description = quiz.Description,
        Category = quiz.Category,
        IsPublic = quiz.IsPublic,
        CreatedBy = quiz.CreatedBy?.Username ?? string.Empty,
        CreatedAt = quiz.CreatedAt,
        Questions = quiz.Questions
            .OrderBy(q => q.OrderNumber)
            .Select(MapQuestionToDto)
            .ToList()
    };

    private static QuestionDto MapQuestionToDto(Question q) => new()
    {
        QuestionId = q.QuestionId,
        QuizId = q.QuizId,
        Text = q.Text,
        TimeLimitSeconds = q.TimeLimitSeconds,
        Points = q.Points,
        OrderNumber = q.OrderNumber,
        Answers = q.Answers.Select(a => new AnswerFullDto
        {
            AnswerId = a.AnswerId,
            Text = a.Text,
            IsCorrect = a.IsCorrect
        }).ToList()
    };
}
