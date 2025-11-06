using ShopSim.DTOs;
using ShopSim.Models;

namespace ShopSim.Services;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(UserRegisterDto registerDto);
    Task<AuthResponseDto> LoginAsync(UserLoginDto loginDto);
    Task<UserReadDto> GetUserByIdAsync(int userId);
    Task<bool> UserExistsAsync(string email);
}

public interface IJwtService
{
    string GenerateToken(User user);
    int? GetUserIdFromToken(string token);
    bool ValidateToken(string token);
}
