using System.Data;
using Application.Interfaces;
using Domain.interfaces;
using Infrastructure.Repositories;

namespace Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly IConnectionFactory _connection;
    private IDbTransaction? _transaction;
    private bool _disposed;
    
    private IQuizRepository? _quizRepository;
    private IQuestionRepository? _questionRepository;
    private IOptionRepository? _optionRepository;
    private IUserRepository? _userRepository;
    private IUserRoleRepository? _userRoleRepository;

    public UnitOfWork(IConnectionFactory connectionFactory)
    {
        _connection = connectionFactory;
    }

    public IQuizRepository Quizzes => _quizRepository ??= new QuizRepository(_connection, _transaction);
    public IQuestionRepository Questions => _questionRepository ??= new QuestionRepository(_connection, _transaction);
    public IOptionRepository Options => _optionRepository ??= new OptionRepository(_connection, _transaction);
    public IUserRepository Users => _userRepository ??= new UserRepository(_connection, _transaction);
    
    public IUserRoleRepository UserRoles => _userRoleRepository ??= new UserRoleRepository(_connection, _transaction);

    public void BeginTransaction()
    {
        if (_transaction != null)
            throw new InvalidOperationException("Transaction already started.");

        var connection = _connection.CreateConnection();
        if (connection.State != ConnectionState.Open)
            connection.Open();

        _transaction = connection.BeginTransaction();
    }

    public async Task CommitAsync()
    {
        try
        {
            _transaction?.Commit();
        }
        catch
        {
            _transaction?.Rollback();
            throw;
        }
        finally
        {
            _transaction?.Dispose();
            _transaction = null;
        }

        await Task.CompletedTask;
    }

    public void Rollback()
    {
        try
        {
            _transaction?.Rollback();
        }
        finally
        {
            _transaction?.Dispose();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) return;
        if (disposing)
        {
            _transaction?.Dispose();
        }
        _disposed = true;
    }
}