using DotNetMvc.Data;
using DotNetMvc.Models;
using Microsoft.EntityFrameworkCore;

namespace DotNetMvc.Services;

public class JobService : IJobService
{
    private readonly ApplicationDbContext _context;
    private const int PageSize = 9;

    public JobService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Job>> GetAllJobsAsync(string? search, JobType? jobType, string? location, int page, int pageSize)
    {
        var query = BuildQuery(search, jobType, location);
        return await query
            .OrderByDescending(j => j.PostedDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> GetJobCountAsync(string? search, JobType? jobType, string? location) =>
        await BuildQuery(search, jobType, location).CountAsync();

    public async Task<Job?> GetByIdAsync(int id) =>
        await _context.Jobs
            .Include(j => j.Applications)
            .FirstOrDefaultAsync(j => j.JobId == id);

    public async Task<List<Job>> GetJobsByEmployerAsync(string userId) =>
        await _context.Jobs
            .Include(j => j.Applications)
            .Where(j => j.PostedByUserId == userId)
            .OrderByDescending(j => j.PostedDate)
            .ToListAsync();

    public async Task<Job> CreateJobAsync(Job job)
    {
        _context.Jobs.Add(job);
        await _context.SaveChangesAsync();
        return job;
    }

    public async Task UpdateJobAsync(Job job)
    {
        _context.Jobs.Update(job);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteJobAsync(int id)
    {
        var job = await _context.Jobs.FindAsync(id);
        if (job != null)
        {
            _context.Jobs.Remove(job);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<string>> GetDistinctLocationsAsync() =>
        await _context.Jobs
            .Where(j => j.IsActive)
            .Select(j => j.Location)
            .Distinct()
            .OrderBy(l => l)
            .ToListAsync();

    public async Task<List<Job>> GetFeaturedJobsAsync(int count = 6) =>
        await _context.Jobs
            .Where(j => j.IsActive)
            .OrderByDescending(j => j.PostedDate)
            .Take(count)
            .ToListAsync();

    private IQueryable<Job> BuildQuery(string? search, JobType? jobType, string? location)
    {
        var query = _context.Jobs.Where(j => j.IsActive).AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(j =>
                j.Title.Contains(search) ||
                j.Company.Contains(search) ||
                j.Description.Contains(search) ||
                j.Location.Contains(search));

        if (jobType.HasValue)
            query = query.Where(j => j.JobType == jobType.Value);

        if (!string.IsNullOrWhiteSpace(location))
            query = query.Where(j => j.Location.Contains(location));

        return query;
    }
}
