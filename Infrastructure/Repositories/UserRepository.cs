using System.Data;
using Application.Interfaces;
using Dapper;
using Domain;
using Domain.interfaces;

namespace Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IDbConnection _connection;
    private IDbTransaction? _transaction;

    public UserRepository(IConnectionFactory connectionFactory, IDbTransaction? transaction)
    {
        _connection = connectionFactory.CreateConnection();
        _transaction = transaction;
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        const string sql = "SELECT * FROM Users WHERE id = @Id";
        var command = new CommandDefinition(
            sql,
            new { Id = id },
            _transaction
        );
        return await _connection.QuerySingleOrDefaultAsync<User>(command);
    }

    public Task<User?> GetByIdWithDetailsAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        const string sql = "SELECT * FROM Users";
        var command = new CommandDefinition(
            sql,
            _transaction
        );
        return await _connection.QueryAsync<User>(command);
    }
    
    public async Task<User?> GetByUsernameAsync(string username)
    {
        const string sql = "SELECT * FROM Users WHERE username = @Username";
        var command = new CommandDefinition(
            sql,
            new { Username = username },
            _transaction
        );
        return await _connection.QuerySingleOrDefaultAsync<User>(command);
    }

    public async Task<int> AddAsync(User user)
    {
        const string sql = """
                               INSERT INTO Users (name, username, email, phone_number, password_hash, created_at)
                               VALUES (@Name, @Username, @Email, @PhoneNumber, @PasswordHash, @CreatedAt)
                               RETURNING id;
                           """;

        var command = new CommandDefinition(
            sql,
            new
            {
                user.Name,
                user.Username,
                user.Email,
                user.PhoneNumber,
                user.PasswordHash,
                user.CreatedAt
            },
            _transaction
        );
        return await _connection.ExecuteScalarAsync<int>(command);
    }

    public async Task<bool> UpdateAsync(User user)
    {
        const string sql = """
                               UPDATE Users 
                               SET username = @Username, 
                                   email = @Email,
                                   password_hash = @PasswordHash
                               WHERE id = @Id;
                           """;

        var affected = await _connection.ExecuteAsync(sql, user, _transaction);
        return affected > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        const string sql = "DELETE FROM Users WHERE id = @Id";
        var affected = await _connection.ExecuteAsync(sql, new { Id = id }, _transaction);
        return affected > 0;
    }
    
    
}