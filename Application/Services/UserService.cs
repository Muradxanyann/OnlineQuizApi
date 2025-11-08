using System.Security.Authentication;
using Application.DTOs.UserDTOs;
using Application.Interfaces;
using AutoMapper;
using Domain;
using Domain.interfaces;

namespace Application.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IAuthService _authService;

    public UserService(IUnitOfWork unitOfWork, IMapper mapper, IAuthService authService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _authService = authService;
    }

    public async Task<UserResponseDto?> RegisterAsync(RegisterUserDto registerDto)
    {
        var existingUser = await _unitOfWork.Users.GetByUsernameAsync(registerDto.Username);
        if (existingUser != null)
        {
            return null!;
        }

        var user = _mapper.Map<User>(registerDto);

        user.PasswordHash = _authService.HashPassword(registerDto.Password);
        user.CreatedAt = DateTime.UtcNow;
        user.IsActive = true;

        user.Id = await _unitOfWork.Users.AddAsync(user);

        switch (registerDto.Username.ToLower())
        {
            case "admin":
                await _unitOfWork.UserRoles.AddUserRole(user.Id, 1);
                break;
            case "teacher":
                await _unitOfWork.UserRoles.AddUserRole(user.Id, 2);
                break;
            default:
                await _unitOfWork.UserRoles.AddUserRole(user.Id, 3);
                break;
        }
        
        await _unitOfWork.CommitAsync();

        return _mapper.Map<UserResponseDto>(user);
    }

    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto loginDto)
    {
        var user = await _unitOfWork.Users.GetByUsernameAsync(loginDto.Username);
        if (user is not { IsActive: true })
        {
            throw new AuthenticationException("Invalid credentials.");
        }

        if (!_authService.VerifyPassword(loginDto.Password, user.PasswordHash))
        {
            throw new AuthenticationException("Incorrect username or password.");
        }

        user.LastLogin = DateTime.UtcNow;
        await _unitOfWork.Users.UpdateAsync(user); 
        await _unitOfWork.CommitAsync();

        return _authService.CreateLoginResponse(user);
    }
    
    public async Task<UserResponseDto?> GetUserByIdAsync(int id)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(id);
        return _mapper.Map<UserResponseDto>(user);
    }

    public async Task<IEnumerable<UserResponseDto>> GetAllUsersAsync()
    {
        var users = await _unitOfWork.Users.GetAllAsync();
        return _mapper.Map<IEnumerable<UserResponseDto>>(users);
    }
}