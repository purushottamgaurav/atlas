using DotNetWebApi.DTOs.Room;
using DotNetWebApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DotNetWebApi.Controllers;

[ApiController]
[Route("api/rooms")]
[Authorize]
public class RoomsController : ControllerBase
{
    private readonly IRoomService _roomService;

    public RoomsController(IRoomService roomService) => _roomService = roomService;

    private int GetUserId()
    {
        var str = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
        return int.Parse(str!);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRoomRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var room = await _roomService.CreateRoomAsync(request.QuizId, request.MaxPlayers, GetUserId());
        return Ok(room);
    }

    [HttpGet("active")]
    public async Task<IActionResult> GetActive() =>
        Ok(await _roomService.GetActiveRoomsAsync());

    [HttpGet("{code}")]
    public async Task<IActionResult> GetByCode(string code)
    {
        var room = await _roomService.GetRoomByCodeAsync(code);
        return room == null ? NotFound() : Ok(room);
    }
}
