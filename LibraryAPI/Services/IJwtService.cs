using LibraryAPI.Models;

namespace LibraryAPI.Services;

public interface IJwtService
{
    string? GenerateUserToken(User user);
    Task<string?> GenerateLibraryToken(string libraryCode);
}