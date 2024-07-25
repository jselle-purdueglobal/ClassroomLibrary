using LibraryAPI.Models;

namespace LibraryAPI.Services;

public interface IBookService
{
    Task<IEnumerable<Book>> GetBooksAsync();
}