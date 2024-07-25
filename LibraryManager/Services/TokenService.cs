using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;

namespace LibraryManager.Services;

public class TokenService(IDataProtectionProvider dataProtectionProvider) : ITokenService
{
    private const string RefreshTokenFileName = "refresh_token.bin";
    private string? _accessToken;

    public Task SaveTokensAsync(string accessToken, string refreshToken)
    {
        _accessToken = accessToken;
        var protector = dataProtectionProvider.CreateProtector("RefreshTokenProtection");
        var encryptedRefreshToken = protector.Protect(refreshToken);
        File.WriteAllText(RefreshTokenFileName, encryptedRefreshToken);
        return Task.CompletedTask;
    }

    public Task<string?> GetAccessTokenAsync()
    {
        return Task.FromResult(_accessToken);
    }

    public Task<string> GetRefreshTokenAsync()
    {
        if (!File.Exists(RefreshTokenFileName))
        {
            return Task.FromResult(string.Empty);
        }

        var protector = dataProtectionProvider.CreateProtector("RefreshTokenProtection");
        var encryptedRefreshToken = File.ReadAllText(RefreshTokenFileName);
        return Task.FromResult(protector.Unprotect(encryptedRefreshToken));
    }

    public async Task<bool> IsTokenValidAsync()
    {
        var token = await GetAccessTokenAsync();
        if (string.IsNullOrEmpty(token))
            return false;

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        var expiration = jwtToken.ValidTo;

        return expiration > DateTime.UtcNow;
    }

    public Task ClearTokensAsync()
    {
        _accessToken = null;
        if (File.Exists(RefreshTokenFileName))
        {
            File.Delete(RefreshTokenFileName);
        }
        return Task.CompletedTask;
    }
}