using System.Security.Cryptography;
using LibraryAPI.Models;
using LibraryAPI.Repositories.Interfaces;
using NuGet.Common;

namespace LibraryAPI.Services;

public class AuthService(IUserRepository userRepository, IJwtService jwtService, IConfiguration configuration) : IAuthService
{
    // Authenticate
    public async Task<AuthResult?> AuthenticateAsync(string username, string password)
    {
        var user = await userRepository.GetUserAsync(username);
        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
        {
            return null;
        }

        var token = jwtService.GenerateUserToken(user);
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

    // Refresh Token
    public async Task<AuthResult?> RefreshTokenAsync(string refreshToken)
    {
        var user = await userRepository.GetUserByRefreshTokenAsync(refreshToken);
        if (user == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            return null;
        }

        var token = jwtService.GenerateUserToken(user);
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

    // Generate Refresh Token
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

    // Get Refresh Token Expiration
    private int GetRefreshTokenExpiryDays()
    {
        return configuration.GetValue("RefreshTokenExpiryDays", 7);  // Default to 7 days if not specified
    }
    
    // Reset Password
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
    
    // Link Library
    public async Task<string?> LinkLibrary(string libraryCode)
    {
        return await jwtService.GenerateLibraryToken(libraryCode);
    }
}