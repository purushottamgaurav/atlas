using System.ComponentModel.DataAnnotations;

namespace DotNetMvc.Models;

public class Job
{
    public int JobId { get; set; }

    [Required, StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required, StringLength(5000)]
    [Display(Name = "Job Description")]
    public string Description { get; set; } = string.Empty;

    [StringLength(3000)]
    public string? Requirements { get; set; }

    [Required, StringLength(200)]
    public string Company { get; set; } = string.Empty;

    [Required, StringLength(200)]
    public string Location { get; set; } = string.Empty;

    [StringLength(100)]
    [Display(Name = "Salary Range")]
    public string? SalaryRange { get; set; }

    [Required]
    [Display(Name = "Job Type")]
    public JobType JobType { get; set; }

    [Display(Name = "Posted Date")]
    public DateTime PostedDate { get; set; } = DateTime.UtcNow;

    [Display(Name = "Application Deadline")]
    [DataType(DataType.Date)]
    public DateTime? ApplicationDeadline { get; set; }

    [Display(Name = "Active")]
    public bool IsActive { get; set; } = true;

    [Required]
    public string PostedByUserId { get; set; } = string.Empty;

    public ICollection<JobApplication> Applications { get; set; } = new List<JobApplication>();
}
