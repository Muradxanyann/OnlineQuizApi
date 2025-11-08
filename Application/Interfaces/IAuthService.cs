using Application.DTOs.UserDTOs;
using Domain;

namespace Application.Interfaces;

public interface IAuthService
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string storedHash);
    LoginResponseDto CreateLoginResponse(User user);
}