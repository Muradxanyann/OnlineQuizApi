using Application.DTOs.QuizDTOs;

namespace Application.Interfaces;

public interface IQuizService
{
    Task<IEnumerable<QuizDetailsDto?>> GetAllAsync();
    Task<QuizDetailsDto?> GetByIdWithDetailsAsync(int id);
    Task<int> CreateAsync(CreateQuizDto quiz);
    Task<bool> UpdateAsync(UpdateQuizDto quiz);
    Task<bool> DeleteAsync(int id);
}