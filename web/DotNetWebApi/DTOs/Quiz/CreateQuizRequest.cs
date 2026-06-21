using System.ComponentModel.DataAnnotations;

namespace DotNetWebApi.DTOs.Quiz;

public class CreateQuizRequest
{
    [Required, StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }

    [StringLength(100)]
    public string? Category { get; set; }

    public bool IsPublic { get; set; } = true;
}
