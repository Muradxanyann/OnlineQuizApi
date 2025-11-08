using Application.DTOs.OptionDTOs;
using Application.Interfaces;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace OnlineQuizzApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OptionsController : ControllerBase
{
    private readonly IOptionService _service;

    public OptionsController(IOptionService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var options = await _service.GetAllAsync();
        return Ok(options);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var option = await _service.GetByIdAsync(id);
        return option is not null ? Ok(option) : NotFound($"Option with id {id}not found");
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOptionDto option)
    {
        var id = await _service.CreateAsync(option);
        return CreatedAtAction(nameof(GetById), new { id }, option);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateOptionDto option)
    {
        var result = await _service.UpdateAsync(option);
        return result ? NoContent() : NotFound();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _service.DeleteAsync(id);
        return result ? NoContent() : NotFound("Option not found");
    }
}