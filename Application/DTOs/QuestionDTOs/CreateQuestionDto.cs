using Application.DTOs.OptionDTOs;

namespace Application.DTOs.QuestionDTOs;

public class CreateQuestionDto
{
    public int QuizId { get; set; }
    public string Text { get; set; } = string.Empty;
    public int Duration { get; set; } = 30;
    public ICollection<CreateOptionDto> Options { get; set; } = new List<CreateOptionDto>();
}