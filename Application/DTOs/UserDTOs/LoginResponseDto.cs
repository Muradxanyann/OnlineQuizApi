namespace Application.DTOs.UserDTOs;

public class LoginResponseDto
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty; // JWT Token
}