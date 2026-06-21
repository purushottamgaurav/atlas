using DotNetWebApi.DTOs.Question;
using DotNetWebApi.DTOs.Quiz;
using DotNetWebApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DotNetWebApi.Controllers;

[ApiController]
[Route("api/quizzes")]
[Authorize]
public class QuizzesController : ControllerBase
{
    private readonly IQuizService _quizService;

    public QuizzesController(IQuizService quizService) => _quizService = quizService;

    private int GetUserId()
    {
        var str = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
        return int.Parse(str!);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await _quizService.GetAllAsync(GetUserId()));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var quiz = await _quizService.GetByIdAsync(id);
        return quiz == null ? NotFound() : Ok(quiz);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateQuizRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var quiz = await _quizService.CreateAsync(request, GetUserId());
        return CreatedAtAction(nameof(GetById), new { id = quiz.QuizId }, quiz);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] CreateQuizRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var quiz = await _quizService.UpdateAsync(id, request, GetUserId());
        return quiz == null ? NotFound() : Ok(quiz);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _quizService.DeleteAsync(id, GetUserId());
        return success ? NoContent() : NotFound();
    }

    [HttpPost("{id:int}/questions")]
    public async Task<IActionResult> AddQuestion(int id, [FromBody] CreateQuestionRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var question = await _quizService.AddQuestionAsync(id, request, GetUserId());
        return Ok(question);
    }

    [HttpDelete("questions/{questionId:int}")]
    public async Task<IActionResult> DeleteQuestion(int questionId)
    {
        var success = await _quizService.DeleteQuestionAsync(questionId, GetUserId());
        return success ? NoContent() : NotFound();
    }
}
