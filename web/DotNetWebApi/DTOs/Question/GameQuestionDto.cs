namespace DotNetWebApi.DTOs.Question;

// Sent to clients during active gameplay — never includes IsCorrect
public class GameQuestionDto
{
    public int QuestionId { get; set; }
    public string Text { get; set; } = string.Empty;
    public int TimeLimitSeconds { get; set; }
    public int Points { get; set; }
    public int OrderNumber { get; set; }
    public int QuestionNumber { get; set; }
    public int TotalQuestions { get; set; }
    public List<AnswerOptionDto> Answers { get; set; } = new();
}
