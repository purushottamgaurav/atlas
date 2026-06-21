using DotNetWebApi.DTOs.Question;
using DotNetWebApi.DTOs.Quiz;

namespace DotNetWebApi.Services;

public interface IQuizService
{
    Task<List<QuizSummaryDto>> GetAllAsync(int userId);
    Task<QuizDto?> GetByIdAsync(int quizId);
    Task<QuizDto> CreateAsync(CreateQuizRequest request, int userId);
    Task<QuizDto?> UpdateAsync(int quizId, CreateQuizRequest request, int userId);
    Task<bool> DeleteAsync(int quizId, int userId);
    Task<QuestionDto> AddQuestionAsync(int quizId, CreateQuestionRequest request, int userId);
    Task<bool> DeleteQuestionAsync(int questionId, int userId);
}
