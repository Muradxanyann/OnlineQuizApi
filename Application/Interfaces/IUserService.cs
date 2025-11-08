using Application.DTOs.UserDTOs;

namespace Application.Interfaces;

public interface IUserService
{
    Task<UserResponseDto?> GetUserByIdAsync(int id);
    Task<IEnumerable<UserResponseDto>> GetAllUsersAsync();
        
    Task<UserResponseDto?> RegisterAsync(RegisterUserDto registerDto);
    Task<LoginResponseDto> LoginAsync(LoginRequestDto loginDto);
}