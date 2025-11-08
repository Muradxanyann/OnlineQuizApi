using System.Data;
using Dapper;
using Domain;
using Domain.interfaces;
using Application.Interfaces;

namespace Infrastructure.Repositories;

public class QuestionRepository : IQuestionRepository
{
    private readonly IDbConnection _connection;
    private readonly IDbTransaction? _transaction;
    
    public QuestionRepository(IConnectionFactory connectionFactory, IDbTransaction? transaction)
    {
        _connection = connectionFactory.CreateConnection();
        _transaction = transaction;
    }

    public async Task<IEnumerable<Question>> GetAllAsync()
    {
        const string sql = "SELECT * FROM questions;";
        return await _connection.QueryAsync<Question>(sql, transaction: _transaction);
    }
    public async Task<Question?> GetByIdAsync(int id)
    {
        const string sql = "SELECT * FROM questions WHERE id = @Id;";
        return await _connection.QuerySingleOrDefaultAsync<Question>(sql, new { Id = id }, _transaction);
    }

    public async Task<Question?> GetByIdWithDetailsAsync(int id)
    {
        const string sql = """
                               SELECT q.*, o.*
                               FROM questions q
                               LEFT JOIN options o ON q.id = o.question_id
                               WHERE q.id = @Id;
                           """;

        var questionDict = new Dictionary<int, Question>();

        var result = await _connection.QueryAsync<Question, Option, Question>(
            sql,
            (question, option) =>
            {
                if (!questionDict.TryGetValue(question.Id, out var qEntry))
                {
                    qEntry = question;
                    qEntry.Options = new List<Option>();
                    questionDict.Add(qEntry.Id, qEntry);
                }

                qEntry.Options.Add(option);

                return qEntry;
            },
            new { Id = id },
            _transaction
        );

        return result.FirstOrDefault();
    }

    

    public async Task<int> AddAsync(Question question)
    {
        const string sql = """
                               INSERT INTO questions (quiz_id, text)
                               VALUES (@QuizId, @Text)
                               RETURNING id;
                           """;
        return await _connection.ExecuteScalarAsync<int>(sql, question, _transaction);
    }

    public async Task<bool> UpdateAsync(Question question)
    {
        const string sql = """
                               UPDATE questions
                               SET text = @Text
                               WHERE id = @Id;
                           """;
        var rows = await _connection.ExecuteAsync(sql, question, _transaction);
        return rows > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        const string sql = "DELETE FROM questions WHERE id = @Id;";
        var rows = await _connection.ExecuteAsync(sql, new { Id = id }, _transaction);
        return rows > 0;
    }
}