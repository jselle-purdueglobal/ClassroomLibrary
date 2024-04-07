using LibraryAPI.Models;

namespace LibraryAPI.Services.Interfaces;

public interface IBookService
{
    Task<IEnumerable<Book>> GetBooksAsync();
}