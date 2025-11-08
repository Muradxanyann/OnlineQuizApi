namespace Domain.interfaces;

public interface IQuestionRepository
{
    Task<Question?> GetByIdAsync(int id);
    Task<Question?> GetByIdWithDetailsAsync(int id);
    Task<IEnumerable<Question>> GetAllAsync();
    
    Task<int> AddAsync(Question quiz);
    Task<bool> UpdateAsync(Question quiz);
    Task<bool> DeleteAsync(int id);
}