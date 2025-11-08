using Application.DTOs.OptionDTOs;
using Application.Interfaces;
using AutoMapper;
using Domain;
using Domain.interfaces;

namespace Application.Services;

public class OptionService : IOptionService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;
    
    public OptionService(IUnitOfWork unitOfWork,  IMapper mapper)
    {
        _uow = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<OptionResponseDto>> GetAllAsync()
    {
        var options =  await _uow.Options.GetAllAsync();
        return _mapper.Map<IEnumerable<OptionResponseDto>>(options);
        
    }

    public async Task<OptionResponseDto?> GetByIdAsync(int id)
    {
        var option = await _uow.Options.GetByIdAsync(id);
        return _mapper.Map<OptionResponseDto>(option);
    }

    public async Task<int> CreateAsync(CreateOptionDto createOption)
    {
        var option =  _mapper.Map<Option>(createOption);
        _uow.BeginTransaction();
        try
        {
            var id = await _uow.Options.AddAsync(option);
            await _uow.CommitAsync();
            return id;
        }
        catch
        {
            _uow.Rollback();
            throw;
        }
    }

    public async Task<bool> UpdateAsync(UpdateOptionDto createOption)
    {
        var option = _mapper.Map<Option>(createOption);
        return await _uow.Options.UpdateAsync(option);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _uow.Options.DeleteAsync(id);
    }
}