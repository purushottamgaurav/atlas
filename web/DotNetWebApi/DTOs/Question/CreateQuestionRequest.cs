using System.ComponentModel.DataAnnotations;

namespace DotNetWebApi.DTOs.Question;

public class CreateAnswerRequest
{
    [Required, StringLength(500)]
    public string Text { get; set; } = string.Empty;
    public bool IsCorrect { get; set; }
}

public class CreateQuestionRequest
{
    [Required, StringLength(1000)]
    public string Text { get; set; } = string.Empty;

    [Range(5, 120)]
    public int TimeLimitSeconds { get; set; } = 30;

    [Range(10, 1000)]
    public int Points { get; set; } = 100;

    public int OrderNumber { get; set; }

    [Required, MinLength(2, ErrorMessage = "At least 2 answers required.")]
    public List<CreateAnswerRequest> Answers { get; set; } = new();
}
