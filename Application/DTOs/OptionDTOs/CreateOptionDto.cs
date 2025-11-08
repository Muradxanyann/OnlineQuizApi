namespace Application.DTOs.OptionDTOs;

public class CreateOptionDto
{
    public int QuestionId { get; set; }
    public string Text { get; set; } = string.Empty;
    public bool IsCorrect { get; set; }
}