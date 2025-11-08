using Application.DTOs.QuestionDTOs;

namespace Application.DTOs.QuizDTOs;

public class QuizDetailsDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string CodeToJoin { get; set; } = string.Empty;
    public bool IsPublished { get; set; }
    public string? CategoryName { get; set; }
    public string? LevelName { get; set; }
    public ICollection<QuestionResponseDto> Questions { get; set; } = new List<QuestionResponseDto>();
}