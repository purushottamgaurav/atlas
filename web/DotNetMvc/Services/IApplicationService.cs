using DotNetMvc.Models;

namespace DotNetMvc.Services;

public interface IApplicationService
{
    Task<bool> HasAppliedAsync(int jobId, string userId);
    Task ApplyAsync(JobApplication application);
    Task<List<JobApplication>> GetApplicationsForJobAsync(int jobId);
    Task<List<JobApplication>> GetApplicationsByUserAsync(string userId);
    Task UpdateStatusAsync(int applicationId, ApplicationStatus status);
    Task<int> GetApplicationCountForJobAsync(int jobId);
}
