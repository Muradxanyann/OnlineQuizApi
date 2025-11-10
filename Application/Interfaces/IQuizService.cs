using Application.DTOs.InternalDTOs;
using Application.DTOs.Pagination;
using Application.DTOs.QuizDTOs;
using Domain;

namespace Application.Interfaces;

public interface IQuizService
{
    Task<PagedResponse<QuizListDto>> GetPagedAsync(PagedRequest request);
    Task<QuizDetailsDto?> GetByIdWithDetailsAsync(int id);
    Task<int> CreateAsync(CreateQuizDto quiz);
    Task<bool> UpdateAsync(UpdateQuizDto quiz);
    Task<bool> DeleteAsync(int id);
    
    Task SubmitQuizAsync(int quizId, int userId, QuizSubmissionDto submission);
}