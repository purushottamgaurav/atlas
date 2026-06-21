using System.ComponentModel.DataAnnotations;

namespace DotNetMvc.Models;

public class UserProfile
{
    public int UserProfileId { get; set; }

    [Required]
    public string UserId { get; set; } = string.Empty;

    [Required, StringLength(100)]
    [Display(Name = "Display Name")]
    public string DisplayName { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public UserRole Role { get; set; }

    [StringLength(200)]
    [Display(Name = "Company Name")]
    public string? CompanyName { get; set; }
}
