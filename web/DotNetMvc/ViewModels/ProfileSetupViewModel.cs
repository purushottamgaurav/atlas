using System.ComponentModel.DataAnnotations;
using DotNetMvc.Models;

namespace DotNetMvc.ViewModels;

public class ProfileSetupViewModel
{
    [Required, StringLength(100)]
    [Display(Name = "Display Name")]
    public string DisplayName { get; set; } = string.Empty;

    [Required]
    [Display(Name = "I am a...")]
    public UserRole Role { get; set; }

    [StringLength(200)]
    [Display(Name = "Company Name")]
    public string? CompanyName { get; set; }
}
