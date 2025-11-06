namespace Domain.interfaces;

public interface IQuizRepository
{
    Task<Quiz?> GetByIdAsync(int id);
    Task<Quiz?> GetByIdWithDetailsAsync(int id);
    Task<IEnumerable<Quiz>> GetAllAsync();
    
    Task<int> AddAsync(Quiz quiz);
    Task<bool> UpdateAsync(Quiz quiz);
    Task<bool> DeleteAsync(int id);
    Task<Quiz?> FindByCodeAsync(string code);
}