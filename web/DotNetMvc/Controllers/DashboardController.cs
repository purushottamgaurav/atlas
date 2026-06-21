using DotNetMvc.Models;
using DotNetMvc.Services;
using DotNetMvc.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DotNetMvc.Controllers;

[Route("dashboard")]
public class DashboardController : BaseController
{
    private readonly IUserProfileService _profileService;
    private readonly IJobService _jobService;
    private readonly IApplicationService _applicationService;

    public DashboardController(IUserProfileService profileService, IJobService jobService, IApplicationService applicationService)
    {
        _profileService = profileService;
        _jobService = jobService;
        _applicationService = applicationService;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        var userId = GetCurrentUserId();
        var profile = await _profileService.GetProfileAsync(userId);

        if (profile == null)
            return RedirectToAction("Setup", "Profile");

        var vm = new DashboardViewModel { UserRole = profile.Role };

        if (profile.Role == UserRole.Employer)
        {
            vm.MyJobs = await _jobService.GetJobsByEmployerAsync(userId);
            vm.ActiveJobCount = vm.MyJobs.Count(j => j.IsActive);
            vm.TotalApplicationsReceived = vm.MyJobs.Sum(j => j.Applications.Count);
        }
        else
        {
            vm.MyApplications = await _applicationService.GetApplicationsByUserAsync(userId);
            vm.PendingCount = vm.MyApplications.Count(a => a.Status == ApplicationStatus.Pending);
            vm.AcceptedCount = vm.MyApplications.Count(a => a.Status == ApplicationStatus.Accepted);
        }

        ViewData["Title"] = "Dashboard";
        return View(vm);
    }
}
