namespace Application.DTOs;

public class InternalQuizDetailsDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<InternalQuestionDto> Questions { get; set; } = new List<InternalQuestionDto>();
}