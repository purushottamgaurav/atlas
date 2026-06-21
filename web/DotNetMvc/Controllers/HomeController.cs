using DotNetMvc.Models;
using DotNetMvc.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DotNetMvc.Controllers;

public class HomeController : BaseController
{
    private readonly IUserProfileService _profileService;
    private readonly ILogger<HomeController> _logger;

    public HomeController(IUserProfileService profileService, ILogger<HomeController> logger)
    {
        _profileService = profileService;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        var userId = GetCurrentUserId();
        if (!await _profileService.ProfileExistsAsync(userId))
            return RedirectToAction("Setup", "Profile");

        ViewData["Title"] = "Home";
        return View();
    }

    public IActionResult About()
    {
        ViewData["Title"] = "About JobBoard";
        return View();
    }

    [AllowAnonymous]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
