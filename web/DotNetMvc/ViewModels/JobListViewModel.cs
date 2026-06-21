using DotNetMvc.Models;

namespace DotNetMvc.ViewModels;

public class JobListViewModel
{
    public List<Job> Jobs { get; set; } = new();
    public string? SearchTerm { get; set; }
    public JobType? SelectedJobType { get; set; }
    public string? SelectedLocation { get; set; }
    public int CurrentPage { get; set; } = 1;
    public int TotalPages { get; set; }
    public int TotalCount { get; set; }
    public List<string> AvailableLocations { get; set; } = new();
}
