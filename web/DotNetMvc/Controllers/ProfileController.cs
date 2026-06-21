using DotNetMvc.Models;
using DotNetMvc.Services;
using DotNetMvc.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DotNetMvc.Controllers;

[Route("profile")]
public class ProfileController : BaseController
{
    private readonly IUserProfileService _profileService;

    public ProfileController(IUserProfileService profileService)
    {
        _profileService = profileService;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        var profile = await _profileService.GetProfileAsync(GetCurrentUserId());
        if (profile == null)
            return RedirectToAction(nameof(Setup));

        return View(profile);
    }

    [HttpGet("setup")]
    public async Task<IActionResult> Setup()
    {
        var userId = GetCurrentUserId();
        if (await _profileService.ProfileExistsAsync(userId))
            return RedirectToAction(nameof(Index));

        var vm = new ProfileSetupViewModel
        {
            DisplayName = GetCurrentUserName()
        };
        return View(vm);
    }

    [HttpPost("setup")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Setup(ProfileSetupViewModel model)
    {
        if (model.Role == UserRole.Employer && string.IsNullOrWhiteSpace(model.CompanyName))
            ModelState.AddModelError("CompanyName", "Company name is required for employers.");

        if (!ModelState.IsValid)
            return View(model);

        var profile = new UserProfile
        {
            UserId = GetCurrentUserId(),
            DisplayName = model.DisplayName,
            Email = GetCurrentUserEmail(),
            Role = model.Role,
            CompanyName = model.CompanyName
        };

        await _profileService.CreateProfileAsync(profile);
        TempData["SuccessMessage"] = $"Welcome, {profile.DisplayName}! Your profile has been set up.";
        return RedirectToAction("Index", "Home");
    }
}
