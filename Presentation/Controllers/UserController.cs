
using Application.DTOs.UserDTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace OnlineQuizzApi.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserDto registerDto)
    {
        var registeredUser = await _userService.RegisterAsync(registerDto);
        if  (registeredUser is null)
            return BadRequest("This username already exists");
        return Ok(registeredUser);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto loginDto)
    {
        var response = await _userService.LoginAsync(loginDto);
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        if (user == null)
            return NotFound();
        
        return Ok(user);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userService.GetAllUsersAsync();
        return Ok(users);
    }
}
