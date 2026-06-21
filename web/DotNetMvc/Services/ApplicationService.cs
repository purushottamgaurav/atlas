using DotNetMvc.Data;
using DotNetMvc.Models;
using Microsoft.EntityFrameworkCore;

namespace DotNetMvc.Services;

public class ApplicationService : IApplicationService
{
    private readonly ApplicationDbContext _context;

    public ApplicationService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> HasAppliedAsync(int jobId, string userId) =>
        await _context.JobApplications.AnyAsync(a => a.JobId == jobId && a.ApplicantUserId == userId);

    public async Task ApplyAsync(JobApplication application)
    {
        _context.JobApplications.Add(application);
        await _context.SaveChangesAsync();
    }

    public async Task<List<JobApplication>> GetApplicationsForJobAsync(int jobId) =>
        await _context.JobApplications
            .Where(a => a.JobId == jobId)
            .OrderByDescending(a => a.AppliedDate)
            .ToListAsync();

    public async Task<List<JobApplication>> GetApplicationsByUserAsync(string userId) =>
        await _context.JobApplications
            .Include(a => a.Job)
            .Where(a => a.ApplicantUserId == userId)
            .OrderByDescending(a => a.AppliedDate)
            .ToListAsync();

    public async Task UpdateStatusAsync(int applicationId, ApplicationStatus status)
    {
        var application = await _context.JobApplications.FindAsync(applicationId);
        if (application != null)
        {
            application.Status = status;
            await _context.SaveChangesAsync();
        }
    }

    public async Task<int> GetApplicationCountForJobAsync(int jobId) =>
        await _context.JobApplications.CountAsync(a => a.JobId == jobId);
}
