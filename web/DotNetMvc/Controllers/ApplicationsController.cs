using DotNetMvc.Models;
using DotNetMvc.Services;
using DotNetMvc.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DotNetMvc.Controllers;

public class ApplicationsController : BaseController
{
    private readonly IApplicationService _applicationService;
    private readonly IJobService _jobService;
    private readonly IUserProfileService _profileService;

    public ApplicationsController(IApplicationService applicationService, IJobService jobService, IUserProfileService profileService)
    {
        _applicationService = applicationService;
        _jobService = jobService;
        _profileService = profileService;
    }

    [HttpPost("applications/apply")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Apply(ApplicationViewModel model)
    {
        var userId = GetCurrentUserId();
        var profile = await _profileService.GetProfileAsync(userId);

        if (profile?.Role != UserRole.JobSeeker)
        {
            TempData["ErrorMessage"] = "Only job seekers can apply for jobs.";
            return RedirectToAction("Details", "Jobs", new { id = model.JobId });
        }

        if (await _applicationService.HasAppliedAsync(model.JobId, userId))
        {
            TempData["ErrorMessage"] = "You have already applied for this job.";
            return RedirectToAction("Details", "Jobs", new { id = model.JobId });
        }

        if (!ModelState.IsValid)
        {
            var job = await _jobService.GetByIdAsync(model.JobId);
            if (job == null) return NotFound();

            var vm = new JobDetailsViewModel
            {
                Job = job,
                ApplicationForm = model,
                IsAlreadyApplied = false,
                IsOwner = job.PostedByUserId == userId,
                ApplicationCount = await _applicationService.GetApplicationCountForJobAsync(model.JobId)
            };
            return View("~/Views/Jobs/Details.cshtml", vm);
        }

        var application = new JobApplication
        {
            JobId = model.JobId,
            ApplicantUserId = userId,
            CoverLetter = model.CoverLetter,
            Skills = model.Skills,
            AppliedDate = DateTime.UtcNow,
            Status = ApplicationStatus.Pending
        };

        await _applicationService.ApplyAsync(application);
        TempData["SuccessMessage"] = "Your application was submitted successfully!";
        return RedirectToAction("MyApplications");
    }

    [HttpGet("applications/mine")]
    public async Task<IActionResult> MyApplications()
    {
        var userId = GetCurrentUserId();
        var profile = await _profileService.GetProfileAsync(userId);

        if (profile?.Role != UserRole.JobSeeker)
        {
            TempData["ErrorMessage"] = "This section is for job seekers.";
            return RedirectToAction("Index", "Home");
        }

        var applications = await _applicationService.GetApplicationsByUserAsync(userId);
        ViewData["Title"] = "My Applications";
        return View(applications);
    }

    [HttpGet("applications/review/{jobId:int}")]
    public async Task<IActionResult> ReviewApplications(int jobId)
    {
        var userId = GetCurrentUserId();
        var job = await _jobService.GetByIdAsync(jobId);

        if (job == null) return NotFound();

        if (job.PostedByUserId != userId)
        {
            TempData["ErrorMessage"] = "You can only review applications for your own job postings.";
            return RedirectToAction("MyJobs", "Jobs");
        }

        var applications = await _applicationService.GetApplicationsForJobAsync(jobId);
        ViewBag.Job = job;
        ViewData["Title"] = $"Applications — {job.Title}";
        return View(applications);
    }

    [HttpPost("applications/status")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateStatus(int applicationId, ApplicationStatus status, int jobId)
    {
        var userId = GetCurrentUserId();
        var job = await _jobService.GetByIdAsync(jobId);

        if (job == null) return NotFound();
        if (job.PostedByUserId != userId) return Forbid();

        await _applicationService.UpdateStatusAsync(applicationId, status);
        TempData["SuccessMessage"] = $"Application status updated to {status}.";
        return RedirectToAction(nameof(ReviewApplications), new { jobId });
    }
}
