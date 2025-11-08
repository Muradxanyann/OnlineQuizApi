namespace Domain.interfaces;

public interface IOptionRepository
{
    Task<Option?> GetByIdAsync(int id);
    Task<IEnumerable<Option>> GetAllAsync();
    Task<int> AddAsync(Option quiz);
    Task<bool> UpdateAsync(Option quiz);
    Task<bool> DeleteAsync(int id);
}