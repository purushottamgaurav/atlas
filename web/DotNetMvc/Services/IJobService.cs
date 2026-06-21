using DotNetMvc.Models;

namespace DotNetMvc.Services;

public interface IJobService
{
    Task<List<Job>> GetAllJobsAsync(string? search, JobType? jobType, string? location, int page, int pageSize);
    Task<int> GetJobCountAsync(string? search, JobType? jobType, string? location);
    Task<Job?> GetByIdAsync(int id);
    Task<List<Job>> GetJobsByEmployerAsync(string userId);
    Task<Job> CreateJobAsync(Job job);
    Task UpdateJobAsync(Job job);
    Task DeleteJobAsync(int id);
    Task<List<string>> GetDistinctLocationsAsync();
    Task<List<Job>> GetFeaturedJobsAsync(int count = 6);
}
