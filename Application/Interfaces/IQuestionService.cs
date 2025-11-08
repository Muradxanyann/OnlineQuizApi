using Application.DTOs.QuestionDTOs;

namespace Application.Interfaces;


public interface IQuestionService
{
    Task<IEnumerable<QuestionResponseDto>> GetAllAsync();
    Task<QuestionResponseDto?> GetByIdWithDetailsAsync(int id);
    Task<int> CreateAsync(CreateQuestionDto question);
    Task<bool> UpdateAsync(UpdateQuestionDto question);
    Task<bool> DeleteAsync(int id);
}