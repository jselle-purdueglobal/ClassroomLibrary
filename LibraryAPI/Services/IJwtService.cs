using LibraryAPI.Models;

namespace LibraryAPI.Services;

public interface IJwtService
{
    string? GenerateToken(User user);
}