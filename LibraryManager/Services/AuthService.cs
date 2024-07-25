using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using LibraryManager.Models;
using ReactiveUI;

namespace LibraryManager.Services;

public class AuthService(IApiClient apiClient, ITokenService tokenService, UserContext userContext) : IAuthService
{
    public async Task<AuthResult> LoginAsync(string username, string password)
        {
            var result = await apiClient.LoginAsync(username, password);
            await tokenService.SaveTokensAsync(result.Token, result.RefreshToken);
            SetUserContext(result.Token);
            return result;
        }

        public async Task<bool> RefreshTokenAsync()
        {
            try
            {
                var refreshToken = await tokenService.GetRefreshTokenAsync();
                if (string.IsNullOrEmpty(refreshToken))
                    return false;

                var result = await apiClient.RefreshTokenAsync(refreshToken);
                await tokenService.SaveTokensAsync(result.Token, result.RefreshToken);
                SetUserContext(result.Token);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task LogoutAsync()
        {
            await tokenService.ClearTokensAsync();
        }

        public async Task<string?> GetAccessTokenAsync()
        {
            if (await tokenService.IsTokenValidAsync()) return await tokenService.GetAccessTokenAsync();
            var refreshSuccessful = await RefreshTokenAsync();
            if (!refreshSuccessful)
            {
                throw new UnauthorizedAccessException("Unable to refresh token. Please log in again.");
            }
            return await tokenService.GetAccessTokenAsync();
        }
        
        private void SetUserContext(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var username = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            var libraryIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "LibraryId")?.Value;

            userContext.Username = username ?? string.Empty;
            userContext.LibraryId = int.TryParse(libraryIdClaim, out var libraryId) ? libraryId : 0;
        }
}