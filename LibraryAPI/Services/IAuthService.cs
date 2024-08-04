using LibraryAPI.Models;

namespace LibraryAPI.Services;

public interface IAuthService
{
    Task<AuthResult?> AuthenticateAsync(string username, string password);
    Task<AuthResult?> RefreshTokenAsync(string refreshToken);
    Task<bool> ResetPasswordAsync(string username, string newPassword);
    Task<string?> LinkLibrary(string libraryCode);
}