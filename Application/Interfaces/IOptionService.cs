using Application.DTOs.OptionDTOs;

namespace Application.Interfaces;

public interface IOptionService
{
    Task<IEnumerable<OptionResponseDto>> GetAllAsync();
    Task<OptionResponseDto?> GetByIdAsync(int id);
    Task<int> CreateAsync(CreateOptionDto option);
    Task<bool> UpdateAsync(UpdateOptionDto option);
    Task<bool> DeleteAsync(int id);
}