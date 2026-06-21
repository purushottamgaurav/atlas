using DotNetMvc.Models;
using DotNetMvc.Services;
using DotNetMvc.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DotNetMvc.Controllers;

[Route("jobs")]
public class JobsController : BaseController
{
    private readonly IJobService _jobService;
    private readonly IApplicationService _applicationService;
    private readonly IUserProfileService _profileService;
    private const int PageSize = 9;

    public JobsController(IJobService jobService, IApplicationService applicationService, IUserProfileService profileService)
    {
        _jobService = jobService;
        _applicationService = applicationService;
        _profileService = profileService;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index(string? search, JobType? jobType, string? location, int page = 1)
    {
        var total = await _jobService.GetJobCountAsync(search, jobType, location);
        var jobs = await _jobService.GetAllJobsAsync(search, jobType, location, page, PageSize);
        var locations = await _jobService.GetDistinctLocationsAsync();

        var vm = new JobListViewModel
        {
            Jobs = jobs,
            SearchTerm = search,
            SelectedJobType = jobType,
            SelectedLocation = location,
            CurrentPage = page,
            TotalPages = (int)Math.Ceiling(total / (double)PageSize),
            TotalCount = total,
            AvailableLocations = locations
        };

        ViewData["Title"] = "Browse Jobs";
        return View(vm);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Details(int id)
    {
        var job = await _jobService.GetByIdAsync(id);
        if (job == null) return NotFound();

        var userId = GetCurrentUserId();
        var profile = await _profileService.GetProfileAsync(userId);

        var vm = new JobDetailsViewModel
        {
            Job = job,
            IsAlreadyApplied = await _applicationService.HasAppliedAsync(id, userId),
            IsOwner = job.PostedByUserId == userId,
            ApplicationCount = await _applicationService.GetApplicationCountForJobAsync(id),
            ApplicationForm = new ApplicationViewModel
            {
                JobId = id,
                JobTitle = job.Title,
                ApplicantName = GetCurrentUserName(),
                Email = GetCurrentUserEmail()
            }
        };

        ViewData["Title"] = job.Title;
        return View(vm);
    }

    [HttpGet("create")]
    public async Task<IActionResult> Create()
    {
        var profile = await _profileService.GetProfileAsync(GetCurrentUserId());
        if (profile?.Role != UserRole.Employer)
        {
            TempData["ErrorMessage"] = "Only employers can post jobs. Please update your profile role.";
            return RedirectToAction("Index");
        }

        var vm = new CreateJobViewModel
        {
            Company = profile.CompanyName ?? string.Empty
        };

        ViewData["Title"] = "Post a Job";
        return View(vm);
    }

    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateJobViewModel model)
    {
        var profile = await _profileService.GetProfileAsync(GetCurrentUserId());
        if (profile?.Role != UserRole.Employer)
            return RedirectToAction("Index");

        if (!ModelState.IsValid)
            return View(model);

        var job = new Job
        {
            Title = model.Title,
            Description = model.Description,
            Requirements = model.Requirements,
            Company = model.Company,
            Location = model.Location,
            SalaryRange = model.SalaryRange,
            JobType = model.JobType,
            ApplicationDeadline = model.ApplicationDeadline,
            PostedByUserId = GetCurrentUserId(),
            PostedDate = DateTime.UtcNow,
            IsActive = true
        };

        await _jobService.CreateJobAsync(job);
        TempData["SuccessMessage"] = $"Job \"{job.Title}\" posted successfully!";
        return RedirectToAction(nameof(MyJobs));
    }

    [HttpGet("edit/{id:int}")]
    public async Task<IActionResult> Edit(int id)
    {
        var job = await _jobService.GetByIdAsync(id);
        if (job == null) return NotFound();

        if (job.PostedByUserId != GetCurrentUserId())
        {
            TempData["ErrorMessage"] = "You can only edit your own job postings.";
            return RedirectToAction(nameof(MyJobs));
        }

        var vm = new EditJobViewModel
        {
            JobId = job.JobId,
            Title = job.Title,
            Description = job.Description,
            Requirements = job.Requirements,
            Company = job.Company,
            Location = job.Location,
            SalaryRange = job.SalaryRange,
            JobType = job.JobType,
            ApplicationDeadline = job.ApplicationDeadline,
            IsActive = job.IsActive
        };

        ViewData["Title"] = "Edit Job";
        return View(vm);
    }

    [HttpPost("edit/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, EditJobViewModel model)
    {
        var job = await _jobService.GetByIdAsync(id);
        if (job == null) return NotFound();

        if (job.PostedByUserId != GetCurrentUserId())
            return Forbid();

        if (!ModelState.IsValid)
            return View(model);

        job.Title = model.Title;
        job.Description = model.Description;
        job.Requirements = model.Requirements;
        job.Company = model.Company;
        job.Location = model.Location;
        job.SalaryRange = model.SalaryRange;
        job.JobType = model.JobType;
        job.ApplicationDeadline = model.ApplicationDeadline;
        job.IsActive = model.IsActive;

        await _jobService.UpdateJobAsync(job);
        TempData["SuccessMessage"] = "Job updated successfully!";
        return RedirectToAction(nameof(MyJobs));
    }

    [HttpGet("delete/{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var job = await _jobService.GetByIdAsync(id);
        if (job == null) return NotFound();

        if (job.PostedByUserId != GetCurrentUserId())
        {
            TempData["ErrorMessage"] = "You can only delete your own job postings.";
            return RedirectToAction(nameof(MyJobs));
        }

        ViewData["Title"] = "Delete Job";
        return View(job);
    }

    [HttpPost("delete/{id:int}")]
    [ValidateAntiForgeryToken]
    [ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var job = await _jobService.GetByIdAsync(id);
        if (job == null) return NotFound();

        if (job.PostedByUserId != GetCurrentUserId())
            return Forbid();

        await _jobService.DeleteJobAsync(id);
        TempData["SuccessMessage"] = $"Job \"{job.Title}\" deleted.";
        return RedirectToAction(nameof(MyJobs));
    }

    [HttpGet("mine")]
    public async Task<IActionResult> MyJobs()
    {
        var profile = await _profileService.GetProfileAsync(GetCurrentUserId());
        if (profile?.Role != UserRole.Employer)
        {
            TempData["ErrorMessage"] = "Only employers can view their job postings.";
            return RedirectToAction("Index");
        }

        var jobs = await _jobService.GetJobsByEmployerAsync(GetCurrentUserId());
        ViewData["Title"] = "My Job Postings";
        return View(jobs);
    }
}
