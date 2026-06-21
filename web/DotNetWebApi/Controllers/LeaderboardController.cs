using DotNetWebApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DotNetWebApi.Controllers;

[ApiController]
[Route("api/leaderboard")]
[Authorize]
public class LeaderboardController : ControllerBase
{
    private readonly ILeaderboardService _leaderboard;

    public LeaderboardController(ILeaderboardService leaderboard) => _leaderboard = leaderboard;

    private int GetUserId()
    {
        var str = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
        return int.Parse(str!);
    }

    [HttpGet]
    public async Task<IActionResult> Global() =>
        Ok(await _leaderboard.GetGlobalTopAsync());

    [HttpGet("quiz/{quizId:int}")]
    public async Task<IActionResult> ByQuiz(int quizId) =>
        Ok(await _leaderboard.GetQuizTopAsync(quizId));

    [HttpGet("me")]
    public async Task<IActionResult> MyHistory() =>
        Ok(await _leaderboard.GetUserHistoryAsync(GetUserId()));
}
