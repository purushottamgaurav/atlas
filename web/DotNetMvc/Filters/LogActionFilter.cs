using DotNetMvc.Models;
using DotNetMvc.Services;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace DotNetMvc.Filters;

public class LogActionFilter : IActionFilter
{
    private readonly ILogger<LogActionFilter> _logger;
    private readonly IUserProfileService _profileService;

    public LogActionFilter(ILogger<LogActionFilter> logger, IUserProfileService profileService)
    {
        _logger = logger;
        _profileService = profileService;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        var controller = context.RouteData.Values["controller"];
        var action = context.RouteData.Values["action"];
        var user = context.HttpContext.User.Identity?.Name ?? "Anonymous";

        _logger.LogInformation("[Filter] START {Controller}/{Action} | User: {User}", controller, action, user);

        // Set ViewBag.IsEmployer for every request so the Layout nav renders correctly
        var userId = context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId != null && context.Controller is Microsoft.AspNetCore.Mvc.Controller ctrl)
        {
            var profile = _profileService.GetProfileAsync(userId).GetAwaiter().GetResult();
            ctrl.ViewBag.IsEmployer = profile?.Role == UserRole.Employer;
            ctrl.ViewBag.UserDisplayName = profile?.DisplayName ?? user;
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        var controller = context.RouteData.Values["controller"];
        var action = context.RouteData.Values["action"];
        _logger.LogInformation("[Filter] END {Controller}/{Action}", controller, action);
    }
}
