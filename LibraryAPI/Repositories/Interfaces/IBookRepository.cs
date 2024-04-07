using LibraryAPI.Models;

namespace LibraryAPI.Repositories.Interfaces;

public interface IBookRepository
{
    Task<IEnumerable<Book>> GetBookListAsync();
}