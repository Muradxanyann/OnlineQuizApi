namespace Domain.interfaces;

public interface IUnitOfWork : IDisposable
{
    IQuizRepository Quizzes { get; }
    IQuestionRepository Questions { get; }
    IOptionRepository Options { get; }
    IUserRepository Users { get; }
    IUserRoleRepository  UserRoles { get; }
    
    // ToDO, should add another Repositories

    void BeginTransaction();
    Task CommitAsync();
    void Rollback();
}