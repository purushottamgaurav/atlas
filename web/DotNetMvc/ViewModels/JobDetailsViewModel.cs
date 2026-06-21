using DotNetMvc.Models;

namespace DotNetMvc.ViewModels;

public class JobDetailsViewModel
{
    public Job Job { get; set; } = null!;
    public ApplicationViewModel ApplicationForm { get; set; } = new();
    public bool IsAlreadyApplied { get; set; }
    public bool IsOwner { get; set; }
    public int ApplicationCount { get; set; }
}
