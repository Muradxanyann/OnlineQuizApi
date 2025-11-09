using Application.DTOs.QuestionDTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace OnlineQuizApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuestionsController : ControllerBase
{
    private readonly IQuestionService _service;

    public QuestionsController(IQuestionService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var questions = await _service.GetAllAsync();
        return Ok(questions);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var question = await _service.GetByIdWithDetailsAsync(id);
        return question is not null ? Ok(question) : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateQuestionDto question)
    {
        var id = await _service.CreateAsync(question);
        return CreatedAtAction(nameof(GetById), new { id }, question);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateQuestionDto question)
    {
        var result = await _service.UpdateAsync(question);
        return result ? NoContent() : NotFound();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _service.DeleteAsync(id);
        return result ? NoContent() : NotFound();
    }
}