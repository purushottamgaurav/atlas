using System.ComponentModel.DataAnnotations;

namespace DotNetWebApi.Models;

public class Answer
{
    public int AnswerId { get; set; }

    public int QuestionId { get; set; }
    public Question? Question { get; set; }

    [Required, StringLength(500)]
    public string Text { get; set; } = string.Empty;

    public bool IsCorrect { get; set; }
}
