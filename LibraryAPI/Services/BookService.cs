using LibraryAPI.Models;
using LibraryAPI.Repositories.Interfaces;

namespace LibraryAPI.Services;

public class BookService(IBookRepository bookRepository) : IBookService
{
    public async Task<IEnumerable<Book>> GetBooksAsync()
    {
        return await bookRepository.GetBookListAsync();
    }
}