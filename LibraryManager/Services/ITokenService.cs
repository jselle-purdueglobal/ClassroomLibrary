using System.Threading.Tasks;

namespace LibraryManager.Services;

public interface ITokenService
{
    Task SaveTokensAsync(string accessToken, string refreshToken);
    Task<string?> GetAccessTokenAsync();
    Task<string> GetRefreshTokenAsync();
    Task<bool> IsTokenValidAsync();
    Task ClearTokensAsync();
}