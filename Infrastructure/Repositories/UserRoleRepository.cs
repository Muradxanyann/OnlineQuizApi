using System.Data;
using Application.Interfaces;
using Dapper;
using Domain.interfaces;

namespace Infrastructure.Repositories;

public class UserRoleRepository :  IUserRoleRepository
{
    private readonly IDbConnection _connection;
    private IDbTransaction? _transaction;

    public UserRoleRepository(IConnectionFactory connectionFactory, IDbTransaction? transaction)
    {
        _connection = connectionFactory.CreateConnection();
        _transaction = transaction;
    }
    public Task<int> AddUserRole(int userId, int roleId)
    {
        const string sql = "INSERT INTO UserRoles (user_Id, role_id) VALUES (@userId, @roleId) RETURNING id";
        var command = new CommandDefinition(
            sql,
            new {userId,  roleId},
            _transaction
        );
        return _connection.ExecuteScalarAsync<int>(command);
    }
}