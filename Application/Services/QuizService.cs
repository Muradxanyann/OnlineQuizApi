using Application.DTOs.InternalDTOs;
using Application.DTOs.Pagination;
using Application.DTOs.QuizDTOs;
using Application.Interfaces;
using AutoMapper;
using Domain;
using Domain.interfaces;

namespace Application.Services;

public class QuizService : IQuizService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IMessageBusPublisher _messageBusPublisher;


    public QuizService(IUnitOfWork unitOfWork, IMapper mapper,  IMessageBusPublisher messageBusPublisher)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _messageBusPublisher = messageBusPublisher;
    }

    public async Task<PagedResponse<QuizListDto>> GetPagedAsync(PagedRequest request)
    {
        var (quizzes, totalCount) = await _unitOfWork.Quizzes.GetPagedAsync(request);
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
        var quiz = await _unitOfWork.Quizzes.GetByIdWithDetailsAsync(id);
        return _mapper.Map<QuizDetailsDto>(quiz);
    }

    public async Task<int> CreateAsync(CreateQuizDto quiz)
    {
        var quizEntity = _mapper.Map<Quiz>(quiz);
        _unitOfWork.BeginTransaction();
        try
        {
            var id = await _unitOfWork.Quizzes.AddAsync(quizEntity);
            await _unitOfWork.CommitAsync();
            return id;
        }
        catch
        {
            _unitOfWork.Rollback();
            throw;
        }
    }

    public async Task<bool> UpdateAsync(UpdateQuizDto quiz)
    {
        var quizEntity = _mapper.Map<Quiz>(quiz);
        return await _unitOfWork.Quizzes.UpdateAsync(quizEntity);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _unitOfWork.Quizzes.DeleteAsync(id);
    }
    
    public async Task SubmitQuizAsync(int quizId, int userId, QuizSubmissionDto submission)
    {
        if (await _unitOfWork.Quizzes.HasUserAttemptedQuizAsync(userId, quizId))
            throw new InvalidOperationException("User already attempted this quiz.");

        var quiz = await _unitOfWork.Quizzes.GetByIdAsync(quizId);
        if (quiz == null) throw new KeyNotFoundException("Quiz not found.");

        await _unitOfWork.Quizzes.LogQuizAccessAsync(userId, quizId);

        var quizEvent = new QuizSubmittedEvent
        {
            QuizId = quizId,
            UserId = userId,
            SubmittedAt = DateTime.UtcNow,
            Answers = submission.Answers
        };

        _messageBusPublisher.Publish("quiz_submitted", quizEvent);
    }
}