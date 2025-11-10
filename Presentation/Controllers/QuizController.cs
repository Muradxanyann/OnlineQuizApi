using System.Security.Claims;
using Application.DTOs.InternalDTOs;
using Application.DTOs.Pagination;
using Application.DTOs.QuizDTOs;
using Application.Interfaces;
using Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OnlineQuizApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuizzesController : ControllerBase
{
    private readonly IQuizService _quizService;

    public QuizzesController(IQuizService service)
    {
        _quizService = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetPaged([FromQuery] PagedRequest request)
    {
        var result = await _quizService.GetPagedAsync(request);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var quiz = await _quizService.GetByIdWithDetailsAsync(id);
        return quiz is not null ? Ok(quiz) : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateQuizDto quiz)
    {
        var id = await _quizService.CreateAsync(quiz);
        return CreatedAtAction(nameof(GetById), new { id }, quiz);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateQuizDto quiz)
    {
        var result = await _quizService.UpdateAsync(quiz);
        return result ? NoContent() : NotFound();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _quizService.DeleteAsync(id);
        return result ? NoContent() : NotFound();
    }
    
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPost("{quizId}/submit")]
    public async Task<IActionResult> SubmitQuiz(int quizId, [FromBody] QuizSubmissionDto submission)
    {
        var userId = int.Parse(User.FindFirst("nameid")?.Value ?? "0");
        await _quizService.SubmitQuizAsync(quizId, userId, submission);
        return Ok(new { Message = "Quiz submission received and is being processed." });
    }
}