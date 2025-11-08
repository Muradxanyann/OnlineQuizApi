namespace Application.DTOs;

public class UpdateQuizDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? CategoryId { get; set; }
    public int? LevelId { get; set; }
    public bool IsActive { get; set; }
    public bool IsPublished { get; set; }
}