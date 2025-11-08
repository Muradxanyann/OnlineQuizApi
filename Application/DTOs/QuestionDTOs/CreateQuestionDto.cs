namespace Application.DTOs;

public class CreateQuestionDto
{
    public string Text { get; set; } = string.Empty;
    public int Duration { get; set; } = 30;
    public ICollection<CreateOptionDto> Options { get; set; } = new List<CreateOptionDto>();
}