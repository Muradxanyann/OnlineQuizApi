namespace Application.DTOs.QuizDTOs;

public class CreateQuizDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? CategoryId { get; set; }
    public int? LevelId { get; set; }
    public int? UserId { get; set; }
}