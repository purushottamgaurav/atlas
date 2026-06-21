using DotNetMvc.Models;

namespace DotNetMvc.ViewModels;

public class DashboardViewModel
{
    public UserRole UserRole { get; set; }

    // Employer
    public List<Job> MyJobs { get; set; } = new();
    public int TotalApplicationsReceived { get; set; }
    public int ActiveJobCount { get; set; }

    // Job Seeker
    public List<JobApplication> MyApplications { get; set; } = new();
    public int PendingCount { get; set; }
    public int AcceptedCount { get; set; }
}
