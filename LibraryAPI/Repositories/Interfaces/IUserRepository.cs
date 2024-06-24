using LibraryAPI.Models;

namespace LibraryAPI.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User> GetUserAsync(string username);
}