using LibraryAPI.Models;

namespace LibraryAPI.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User?> GetUserAsync(string username);
    Task<User?> GetUserByRefreshTokenAsync(string refreshToken);
    Task SaveRefreshTokenAsync(string username, string refreshToken, DateTime expiryTime);
    Task<bool> UpdatePasswordAsync(string username, string newPasswordHash);
}