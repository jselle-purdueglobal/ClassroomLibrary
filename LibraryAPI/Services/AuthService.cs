using System.Security.Cryptography;
using LibraryAPI.Models;
using LibraryAPI.Repositories.Interfaces;

namespace LibraryAPI.Services;

public class AuthService(IUserRepository userRepository, IJwtService jwtService, IConfiguration configuration) : IAuthService
{
    public async Task<AuthResult?> AuthenticateAsync(string username, string password)
    {
        var user = await userRepository.GetUserAsync(username);
        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
        {
            return null;
        }

        var token = jwtService.GenerateToken(user);
        var refreshToken = GenerateRefreshToken();
        var refreshTokenExpiryTime = DateTime.UtcNow.AddDays(GetRefreshTokenExpiryDays());

        await userRepository.SaveRefreshTokenAsync(user.Username, refreshToken, refreshTokenExpiryTime);

        return new AuthResult
        {
            Token = token,
            RefreshToken = refreshToken,
            IsFirstLogin = user.IsFirstLogin
        };
    }

    public async Task<AuthResult?> RefreshTokenAsync(string refreshToken)
    {
        var user = await userRepository.GetUserByRefreshTokenAsync(refreshToken);
        if (user == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            return null;
        }

        var token = jwtService.GenerateToken(user);
        var newRefreshToken = GenerateRefreshToken();
        var refreshTokenExpiryTime = DateTime.UtcNow.AddDays(GetRefreshTokenExpiryDays());

        await userRepository.SaveRefreshTokenAsync(user.Username, newRefreshToken, refreshTokenExpiryTime);

        return new AuthResult
        {
            Token = token,
            RefreshToken = newRefreshToken,
            IsFirstLogin = user.IsFirstLogin
        };
    }

    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        var guid = Guid.NewGuid();
        var combinedBytes = new byte[randomNumber.Length + 16];
        Buffer.BlockCopy(randomNumber, 0, combinedBytes, 0, randomNumber.Length);
        Buffer.BlockCopy(guid.ToByteArray(), 0, combinedBytes, randomNumber.Length, 16);
                
        return Convert.ToBase64String(SHA256.HashData(combinedBytes));
    }

    private int GetRefreshTokenExpiryDays()
    {
        return configuration.GetValue<int>("RefreshTokenExpiryDays", 7);  // Default to 7 days if not specified
    }
    public async Task<bool> ResetPasswordAsync(string username, string newPassword)
    {
        var user = await userRepository.GetUserAsync(username);
        if (user == null)
        {
            return false;
        }

        var newPasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
        return await userRepository.UpdatePasswordAsync(username, newPasswordHash);
    }
}