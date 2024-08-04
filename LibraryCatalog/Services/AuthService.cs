using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Maui.Storage;

namespace LibraryCatalog.Services;

public class AuthService(IApiService apiService, LibraryContext libraryContext, IDataProtectionProvider dataProtection) : IAuthService
{
    // Fields
    private const string TokenFileName = "auth_token.bin";
    
    // Link Library
    public async Task<bool> LinkLibraryAsync(string libraryCode)
    {
        var token = await apiService.LinkLibraryAsync(libraryCode);
        if (token == null) return false;
        await SaveTokenAsync(token);
        return true;
    }
    
    // Validate Link
    public async Task<bool> ValidateLibraryLinkAsync()
    {
        var token = await GetTokenAsync();
        if (token == null) return false;
        SetLibraryContext(token);
        return true;
    }

    // Save Token
    private async Task SaveTokenAsync(string token)
    {
        var protector = dataProtection.CreateProtector("AuthTokenProtection");
        var encryptedToken = protector.Protect(token);
        await File.WriteAllTextAsync(TokenFileName, encryptedToken);
    }

    // Get Token
    private async Task<string?> GetTokenAsync()
    {
        if (!File.Exists(TokenFileName))
            return null;
        var protector = dataProtection.CreateProtector("AuthTokenProtection");
        var encryptedToken = await File.ReadAllTextAsync(TokenFileName);
        return protector.Unprotect(encryptedToken);
    }
    
    // Set Library Context
    private void SetLibraryContext(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        var libraryId = jwtToken.Claims.FirstOrDefault(c => c.Type == "LibraryId")?.Value;
        var libraryName = jwtToken.Claims.FirstOrDefault(c => c.Type == "LibraryName")?.Value;

        libraryContext.AccessToken = token;
        libraryContext.LibraryId = Convert.ToInt32(libraryId);
        libraryContext.LibraryName = libraryName!;
    }
}