using System.Data;
using Dapper;
using LibraryAPI.Models;
using LibraryAPI.Repositories.Interfaces;

namespace LibraryAPI.Repositories.Implementations;

public class BookRepository(IDbConnection connection) : IBookRepository
{
    public async Task<IEnumerable<Book>> GetBookListAsync()
    {
        using (connection)
        {
            var lookup = new Dictionary<int, Book>();

            var books = (await connection.QueryAsync<Book, Author, Book>(
                "GetBookList",
                (book, author) =>
                {
                    if (!lookup.TryGetValue(book.BookId, out var existingBook))
                    {
                        existingBook = book;
                        existingBook.Authors = new List<Author>();
                        lookup.Add(existingBook.BookId, existingBook);
                    }

                    existingBook.Authors.Add(author);
                    return existingBook;
                },
                splitOn: "AuthorID",
                commandType: CommandType.StoredProcedure
            )).Distinct().ToList();
            
            return books;
        }
    }
}