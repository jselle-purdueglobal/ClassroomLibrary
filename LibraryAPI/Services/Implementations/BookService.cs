using LibraryAPI.Models;
using LibraryAPI.Repositories.Interfaces;
using LibraryAPI.Services.Interfaces;

namespace LibraryAPI.Services.Implementations;

public class BookService(IBookRepository bookRepository) : IBookService
{
    public async Task<IEnumerable<Book>> GetBooksAsync()
    {
        return await bookRepository.GetBookListAsync();
    }
}