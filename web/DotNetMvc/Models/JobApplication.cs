using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNetMvc.Models;

public class JobApplication
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ApplicationId { get; set; }

    public int JobId { get; set; }

    [Required]
    public string ApplicantUserId { get; set; } = string.Empty;

    [Required, StringLength(3000)]
    [Display(Name = "Cover Letter")]
    public string CoverLetter { get; set; } = string.Empty;

    [StringLength(1000)]
    [Display(Name = "Skills / Technologies")]
    public string? Skills { get; set; }

    [Display(Name = "Applied Date")]
    public DateTime AppliedDate { get; set; } = DateTime.UtcNow;

    public ApplicationStatus Status { get; set; } = ApplicationStatus.Pending;

    public Job? Job { get; set; }
}
