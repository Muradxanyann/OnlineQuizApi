using System.Data;
using Application.DTOs.Pagination;
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
    public async Task<(IEnumerable<Quiz> items, int totalCount)> GetPagedAsync(PagedRequest request)
    {
        // Build WHERE conditions
        var conditions = new List<string>();
        var parameters = new DynamicParameters();

        if (!string.IsNullOrEmpty(request.SearchTerm))
        {
            conditions.Add("(q.name ILIKE @SearchTerm OR q.description ILIKE @SearchTerm)");
            parameters.Add("SearchTerm", $"%{request.SearchTerm}%");
        }

        if (request.CategoryId.HasValue)
        {
            conditions.Add("q.category_id = @CategoryId");
            parameters.Add("CategoryId", request.CategoryId.Value);
        }

        if (request.LevelId.HasValue)
        {
            conditions.Add("q.level_id = @LevelId");
            parameters.Add("LevelId", request.LevelId.Value);
        }

        if (request.IsPublished.HasValue)
        {
            conditions.Add("q.is_published = @IsPublished");
            parameters.Add("IsPublished", request.IsPublished.Value);
        }

        var whereClause = conditions.Any() ? "WHERE " + string.Join(" AND ", conditions) : "";

        // Count query
        var countSql = $"""

                                    SELECT COUNT(*) 
                                    FROM quizzes q
                                    {whereClause}
                        """;

        var totalCount = await _connection.ExecuteScalarAsync<int>(countSql, parameters, _transaction);

        // Data query
        var orderBy = GetOrderByClause(request.SortBy, request.SortDescending);
        
        var dataSql = $"""

                                   SELECT q.*, 
                                          qc.name as CategoryName,
                                          ql.name as LevelName,
                                          (SELECT COUNT(*) FROM questions WHERE quiz_id = q.id) as QuestionsCount
                                   FROM quizzes q
                                   LEFT JOIN quiz_categories qc ON q.category_id = qc.id
                                   LEFT JOIN quiz_levels ql ON q.level_id = ql.id
                                   {whereClause}
                                   {orderBy}
                                   LIMIT @PageSize OFFSET @Offset
                       """;

        parameters.Add("PageSize", request.PageSize);
        parameters.Add("Offset", (request.PageNumber - 1) * request.PageSize);

        var items = await _connection.QueryAsync<Quiz>(dataSql, parameters, _transaction);

        return (items, totalCount);
    }

    private static string GetOrderByClause(string? sortBy, bool sortDescending)
    {
        var orderBy = sortBy?.ToLower() switch
        {
            "name" => "q.name",
            "createdat" => "q.created_at",
            "updatedat" => "q.updated_at",
            "categoryname" => "qc.name",
            "levelname" => "ql.name",
            _ => "q.created_at"
        };

        var direction = sortDescending ? "DESC" : "ASC";
        return $"ORDER BY {orderBy} {direction}";
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

                var existingQuestion = qzEntry.Questions.FirstOrDefault(q => q.Id == question.Id);
                if (existingQuestion == null)
                {
                    question.Options = new List<Option>();
                    qzEntry.Questions.Add(question);
                    existingQuestion = question;
                }

                if (option != null)
                    existingQuestion.Options.Add(option);

                return qzEntry;
            },
            new { Id = id },
            _transaction
        );

        return result.FirstOrDefault();
    }
    
    public async Task<int> AddAsync(Quiz quiz)
    {
        const string sql = """
            INSERT INTO quizzes (name, description, category_id, level_id, user_id, code_to_join, is_active, is_published, created_at, updated_at)
            VALUES (@Name, @Description, @CategoryId, @LevelId, @UserId, @CodeToJoin, @IsActive, @IsPublished, @CreatedAt, @UpdatedAt)
            RETURNING id;
        """;
        return await _connection.ExecuteScalarAsync<int>(sql, quiz, _transaction);
    }

    public async Task<bool> UpdateAsync(Quiz quiz)
    {
        const string sql = """
            UPDATE quizzes
            SET name = @Name,
                description = @Description,
                category_id = @CategoryId,
                level_id = @LevelId,
                is_active = @IsActive,
                is_published = @IsPublished,
                updated_at = @UpdatedAt
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
        const string sql = "SELECT * FROM quizzes WHERE code_to_join = @Code;";
        return await _connection.QuerySingleOrDefaultAsync<Quiz>(sql, new { Code = code }, _transaction);
    }
}
