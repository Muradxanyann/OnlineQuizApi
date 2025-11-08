using Application.DTOs.OptionDTOs;

namespace Application.DTOs.QuestionDTOs;

public class QuestionDto
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public int Duration { get; set; }
    public ICollection<OptionResponseDto> Options { get; set; } = new List<OptionResponseDto>();
}