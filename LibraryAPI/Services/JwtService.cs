using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LibraryAPI.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NuGet.Common;

namespace LibraryAPI.Services;

public class JwtService(IOptions<JwtSettings> jwtSettings, ILibraryService libraryService) : IJwtService
{
    private readonly JwtSettings _jwtSettings = jwtSettings.Value;
    
    // Generate User Token
    public string? GenerateUserToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.UserRole),
            new Claim("LibraryId", user.LibraryId.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(_jwtSettings.DurationInMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
    // Generate Library Token
    public async Task<string?> GenerateLibraryToken(string libraryCode)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var library = await libraryService.GetLibraryByCodeAsync(libraryCode);
        if (library == null) return null;
        

        var claims = new[]
        {
            new Claim("LibraryId", library.LibraryId.ToString()),
            new Claim("LibraryCode", library.LibraryCode.ToString()),
            new Claim("LibraryName", library.LibraryName.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.Now.AddMonths(12),
            signingCredentials: credentials
        );
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}