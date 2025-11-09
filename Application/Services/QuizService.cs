using Application.DTOs.Pagination;
using Application.DTOs.QuizDTOs;
using Application.Interfaces;
using AutoMapper;
using Domain;
using Domain.interfaces;

namespace Application.Services;

public class QuizService : IQuizService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public QuizService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _uow = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PagedResponse<QuizListDto>> GetPagedAsync(PagedRequest request)
    {
        var (quizzes, totalCount) = await _uow.Quizzes.GetPagedAsync(request);
        var quizDtos = _mapper.Map<IEnumerable<QuizListDto>>(quizzes);

        return new PagedResponse<QuizListDto>(
            quizDtos, 
            totalCount, 
            request.PageNumber, 
            request.PageSize
        );
    }

    public async Task<QuizDetailsDto?> GetByIdWithDetailsAsync(int id)
    {
        var quiz = await _uow.Quizzes.GetByIdWithDetailsAsync(id);
        return _mapper.Map<QuizDetailsDto>(quiz);
    }

    public async Task<int> CreateAsync(CreateQuizDto quiz)
    {
        var quizEntity = _mapper.Map<Quiz>(quiz);
        _uow.BeginTransaction();
        try
        {
            var id = await _uow.Quizzes.AddAsync(quizEntity);
            await _uow.CommitAsync();
            return id;
        }
        catch
        {
            _uow.Rollback();
            throw;
        }
    }

    public async Task<bool> UpdateAsync(UpdateQuizDto quiz)
    {
        var quizEntity = _mapper.Map<Quiz>(quiz);
        return await _uow.Quizzes.UpdateAsync(quizEntity);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _uow.Quizzes.DeleteAsync(id);
    }
}