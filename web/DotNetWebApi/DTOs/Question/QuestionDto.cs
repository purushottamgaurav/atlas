namespace DotNetWebApi.DTOs.Question;

public class QuestionDto
{
    public int QuestionId { get; set; }
    public int QuizId { get; set; }
    public string Text { get; set; } = string.Empty;
    public int TimeLimitSeconds { get; set; }
    public int Points { get; set; }
    public int OrderNumber { get; set; }
    public List<AnswerFullDto> Answers { get; set; } = new();
}
