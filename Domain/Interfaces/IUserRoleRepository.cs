namespace Domain.interfaces;

public interface IUserRoleRepository
{
    Task<int> AddUserRole(int userId, int roleId);
}