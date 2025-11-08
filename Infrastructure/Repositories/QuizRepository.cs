using System.Data;
using Dapper;
using Domain;
using Domain.interfaces;
using Application.Interfaces;

namespace Infrastructure.Repositories;

public class QuizRepository : IQuizRepository
{
    private readonly IDbConnection _connection;
    private readonly IDbTransaction? _transaction;

    public QuizRepository(IConnectionFactory connectionFactory, IDbTransaction? transaction)
    {
        _connection = connectionFactory.CreateConnection();
        _transaction = transaction;
    }

    public async Task<Quiz?> GetByIdAsync(int id)
    {
        const string sql = "SELECT * FROM quizzes WHERE id = @Id;";
        return await _connection.QuerySingleOrDefaultAsync<Quiz>(sql, new { Id = id }, _transaction);
    }

    public async Task<Quiz?> GetByIdWithDetailsAsync(int id)
    {
        const string sql = """

                                       SELECT qz.*, qs.*, op.*
                                       FROM quizzes qz
                                       LEFT JOIN questions qs ON qz.id = qs.quiz_id
                                       LEFT JOIN options op ON qs.id = op.question_id
                                       WHERE qz.id = @Id;
                           """;

        var quizDict = new Dictionary<int, Quiz>();

        var result = await _connection.QueryAsync<Quiz, Question, Option, Quiz>(
            sql,
            (quiz, question, option) =>
            {
                if (!quizDict.TryGetValue(quiz.Id, out var qzEntry))
                {
                    qzEntry = quiz;
                    qzEntry.Questions = new List<Question>();
                    quizDict.Add(qzEntry.Id, qzEntry);
                }

                {
                    var existingQuestion = qzEntry.Questions.FirstOrDefault(q => q.Id == question.Id);
                    if (existingQuestion == null)
                    {
                        question.Options = new List<Option>();
                        qzEntry.Questions.Add(question);
                        existingQuestion = question;
                    }

                    existingQuestion.Options.Add(option);
                }

                return qzEntry;
            },
            new { Id = id },
            _transaction
        );

        return result.FirstOrDefault();
    }

    public async Task<IEnumerable<Quiz>> GetAllAsync()
    {
        const string sql = "SELECT * FROM quizzes;";
        return await _connection.QueryAsync<Quiz>(sql, transaction: _transaction);
    }

    public async Task<int> AddAsync(Quiz quiz)
    {
        const string sql = """

                                       INSERT INTO quizzes (title, description, topic, difficulty, created_at)
                                       VALUES (@Title, @Description, @Topic, @Difficulty, @CreatedAt)
                                       RETURNING id;
                           """;
        return await _connection.ExecuteScalarAsync<int>(sql, quiz, _transaction);
    }

    public async Task<bool> UpdateAsync(Quiz quiz)
    {
        const string sql = """

                                       UPDATE quizzes
                                       SET title = @Title,
                                           description = @Description,
                                           topic = @Topic,
                                           difficulty = @Difficulty
                                       WHERE id = @Id;
                           """;
        var rows = await _connection.ExecuteAsync(sql, quiz, _transaction);
        return rows > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        const string sql = "DELETE FROM quizzes WHERE id = @Id;";
        var rows = await _connection.ExecuteAsync(sql, new { Id = id }, _transaction);
        return rows > 0;
    }

    public async Task<Quiz?> FindByCodeAsync(string code)
    {
        const string sql = "SELECT * FROM quizzes WHERE code = @Code;";
        return await _connection.QuerySingleOrDefaultAsync<Quiz>(sql, new { Code = code }, _transaction);
    }
}