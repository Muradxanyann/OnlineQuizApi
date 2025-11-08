using Application.DTOs.QuizDTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace OnlineQuizzApi.Controllers;

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
    public async Task<IActionResult> GetAll()
    {
        var quizzes = await _quizService.GetAllAsync();
        return Ok(quizzes);
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
}