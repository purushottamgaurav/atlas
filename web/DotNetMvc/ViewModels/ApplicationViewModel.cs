using System.ComponentModel.DataAnnotations;

namespace DotNetMvc.ViewModels;

public class ApplicationViewModel
{
    public int JobId { get; set; }
    public string? JobTitle { get; set; }

    [Required]
    [StringLength(3000, MinimumLength = 50, ErrorMessage = "Cover letter must be between 50 and 3000 characters.")]
    [Display(Name = "Cover Letter")]
    public string CoverLetter { get; set; } = string.Empty;

    [StringLength(1000)]
    [Display(Name = "Skills / Technologies")]
    public string? Skills { get; set; }

    public string? ApplicantName { get; set; }
    public string? Email { get; set; }
}
