using System.Threading.Tasks;
using LibraryManager.Models;

namespace LibraryManager.Services;

public interface IAuthService
{
    Task<AuthResult> LoginAsync(string username, string password);
    Task<bool> RefreshTokenAsync();
    Task LogoutAsync();
    Task<string?> GetAccessTokenAsync();
}