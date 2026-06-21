using System.ComponentModel.DataAnnotations;
using DotNetMvc.Models;

namespace DotNetMvc.ViewModels;

public class CreateJobViewModel
{
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

    [Display(Name = "Application Deadline")]
    [DataType(DataType.Date)]
    public DateTime? ApplicationDeadline { get; set; }
}
