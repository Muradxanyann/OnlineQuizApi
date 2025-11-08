namespace Application.DTOs;

public class QuizDetailsDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string CodeToJoin { get; set; } = string.Empty;
    public bool IsPublished { get; set; }
    public string? CategoryName { get; set; }
    public string? LevelName { get; set; }
    public ICollection<QuestionDto> Questions { get; set; } = new List<QuestionDto>();
}