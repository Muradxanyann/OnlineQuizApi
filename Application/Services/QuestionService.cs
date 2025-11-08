using Application.DTOs.QuestionDTOs;
using Application.Interfaces;
using AutoMapper;
using Domain;
using Domain.interfaces;

namespace Application.Services;

public class QuestionService : IQuestionService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;


    public QuestionService(IUnitOfWork unitOfWork,  IMapper mapper)
    {
        _uow = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<QuestionResponseDto>> GetAllAsync()
    {
        var questions = await _uow.Questions.GetAllAsync();
        return _mapper.Map<IEnumerable<QuestionResponseDto>>(questions);
    }

    public async Task<QuestionResponseDto?> GetByIdWithDetailsAsync(int id)
    {
        var question =  await _uow.Questions.GetByIdWithDetailsAsync(id);
        return _mapper.Map<QuestionResponseDto>(question);
    }

    public async Task<int> CreateAsync(CreateQuestionDto createQuestion)
    {
        var question = _mapper.Map<Question>(createQuestion);
        _uow.BeginTransaction();
        try
        {
            var id = await _uow.Questions.AddAsync(question);
            await _uow.CommitAsync();
            return id;
        }
        catch
        {
            _uow.Rollback();
            throw;
        }
    }

    public async Task<bool> UpdateAsync(UpdateQuestionDto updateQuestion)
    {
        var question = _mapper.Map<Question>(updateQuestion);
        return await _uow.Questions.UpdateAsync(question);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _uow.Questions.DeleteAsync(id);
    }
}