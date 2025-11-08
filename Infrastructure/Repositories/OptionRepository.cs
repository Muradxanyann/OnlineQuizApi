using System.Data;
using Application.Interfaces;
using Dapper;
using Domain;
using Domain.interfaces;

namespace Infrastructure.Repositories;

public class OptionRepository : IOptionRepository
{
    private readonly IDbConnection _connection;
    private readonly IDbTransaction? _transaction;

    public OptionRepository(IConnectionFactory connectionFactory, IDbTransaction? transaction)
    {
        _connection = connectionFactory.CreateConnection();
        _transaction = transaction;
    }

    public async Task<Option?> GetByIdAsync(int id)
    {
        const string sql = "SELECT * FROM options WHERE id = @Id;";
        return await _connection.QuerySingleOrDefaultAsync<Option>(sql, new { Id = id }, _transaction);
    }

    public async Task<IEnumerable<Option>> GetAllAsync()
    {
        const string sql = "SELECT * FROM options;";
        return await _connection.QueryAsync<Option>(sql, transaction: _transaction);
    }

    public async Task<int> AddAsync(Option option)
    {
        const string sql = """
                               INSERT INTO options (question_id, text, is_correct)
                               VALUES (@QuestionId, @Text, @IsCorrect)
                               RETURNING id;
                           """;
        return await _connection.ExecuteScalarAsync<int>(sql, option, _transaction);
    }

    public async Task<bool> UpdateAsync(Option option)
    {
        const string sql = """
                               UPDATE options
                               SET text = @Text,
                                   is_correct = @IsCorrect
                               WHERE id = @Id;
                           """;
        var rows = await _connection.ExecuteAsync(sql, option, _transaction);
        return rows > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        const string sql = "DELETE FROM options WHERE id = @Id;";
        var rows = await _connection.ExecuteAsync(sql, new { Id = id }, _transaction);
        return rows > 0;
    }
}