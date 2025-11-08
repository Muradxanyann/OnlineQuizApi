using Application.DTOs.OptionDTOs;

namespace Application.DTOs.QuestionDTOs;

public class QuestionResponseDto
{
    public int Id { get; set; }
    public int QuizId { get; set; }
    public string Text { get; set; } = string.Empty;
    public ICollection<OptionResponseDto> Options { get; set; } = new List<OptionResponseDto>();
}