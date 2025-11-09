namespace Application.DTOs.QuizDTOs;

public class QuizListDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string CodeToJoin { get; set; } = string.Empty;
    public bool IsPublished { get; set; }
    public string? CategoryName { get; set; }
    public string? LevelName { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int QuestionsCount { get; set; }
}