namespace Application.DTOs.InternalDTOs;

public class QuizSubmissionDto
{
    public List<UserAnswerDto> Answers { get; set; } = new List<UserAnswerDto>();
}