using LibraryAPI.Models;

namespace LibraryAPI.Services.Interfaces;

public interface IJwtService
{
    string GenerateToken(User user);
}