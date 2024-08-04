using LibraryAPI.Models;
using LibraryAPI.Repositories.Interfaces;

namespace LibraryAPI.Services;

public class BookService(IBookRepository bookRepository) : IBookService
{
    public async Task<IEnumerable<BookDto>> GetBooksAsync()
    {
        var books = await bookRepository.GetBooksAsync();
        return books.Select(book => new BookDto
        {
            BookId = book.BookId,
            Title = book.Title,
            ImagePath = book.ImagePath,
            Authors = string.Join(", ", book.AuthorList.Select(a => $"{a.FirstName} {a.LastName}")),
            Illustrators = string.Join(", ", book.IllustratorList.Select(i => $"{i.FirstName} {i.LastName}"))
        });
    }
}