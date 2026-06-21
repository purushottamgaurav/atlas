using System.ComponentModel.DataAnnotations;

namespace DotNetWebApi.Models;

public class Question
{
    public int QuestionId { get; set; }

    public int QuizId { get; set; }
    public Quiz? Quiz { get; set; }

    [Required, StringLength(1000)]
    public string Text { get; set; } = string.Empty;

    public int TimeLimitSeconds { get; set; } = 30;

    public int Points { get; set; } = 100;

    public int OrderNumber { get; set; }

    public ICollection<Answer> Answers { get; set; } = new List<Answer>();
}
