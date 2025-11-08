namespace Domain.interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(int id);
    Task<User?> GetByIdWithDetailsAsync(int id);
    Task<IEnumerable<User>> GetAllAsync();
    
    Task<int> AddAsync(User quiz);
    Task<bool> UpdateAsync(User quiz);
    Task<bool> DeleteAsync(int id);
    
    Task<User?> GetByUsernameAsync(string username);
    //void SetTransaction(System.Data.IDbTransaction? transaction);
}