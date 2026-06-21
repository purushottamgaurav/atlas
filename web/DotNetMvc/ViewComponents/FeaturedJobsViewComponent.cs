using DotNetMvc.Services;
using Microsoft.AspNetCore.Mvc;

namespace DotNetMvc.ViewComponents;

public class FeaturedJobsViewComponent : ViewComponent
{
    private readonly IJobService _jobService;

    public FeaturedJobsViewComponent(IJobService jobService)
    {
        _jobService = jobService;
    }

    public async Task<IViewComponentResult> InvokeAsync(int count = 6)
    {
        var jobs = await _jobService.GetFeaturedJobsAsync(count);
        return View(jobs);
    }
}
