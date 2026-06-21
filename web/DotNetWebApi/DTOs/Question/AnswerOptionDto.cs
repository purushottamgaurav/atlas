namespace DotNetWebApi.DTOs.Question;

public class AnswerOptionDto
{
    public int AnswerId { get; set; }
    public string Text { get; set; } = string.Empty;
}

public class AnswerFullDto : AnswerOptionDto
{
    public bool IsCorrect { get; set; }
}
