using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DotNetMvc.Controllers;

public abstract class BaseController : Controller
{
    protected string GetCurrentUserId() =>
        User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new InvalidOperationException("User is not authenticated.");

    protected string GetCurrentUserName() =>
        User.FindFirstValue("name")
            ?? User.Identity?.Name
            ?? "Unknown";

    protected string GetCurrentUserEmail() =>
        User.FindFirstValue(ClaimTypes.Email)
            ?? User.FindFirstValue("preferred_username")
            ?? string.Empty;
}
