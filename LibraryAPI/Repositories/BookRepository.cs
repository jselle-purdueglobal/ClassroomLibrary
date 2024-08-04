using System.Data;
using Dapper;
using LibraryAPI.Models;
using LibraryAPI.Repositories.Interfaces;

namespace LibraryAPI.Repositories;

public class BookRepository(IDbConnection connection) : IBookRepository
{
    public async Task<IEnumerable<Book>> GetBooksAsync()
    {
        using (connection)
        {
            var bookDictionary = new Dictionary<int, Book>();

            await connection.QueryAsync<Book, Author, Illustrator?, Book>(
                "spGetBooks",
                (book, author, illustrator) =>
                {
                    if (!bookDictionary.TryGetValue(book.BookId, out var bookEntry))
                    {
                        bookEntry = book;
                        bookEntry.AuthorList = new List<Author>();
                        bookEntry.IllustratorList = new List<Illustrator>();
                        bookDictionary.Add(bookEntry.BookId, bookEntry);
                    }

                    if (bookEntry.AuthorList.All(a => a.AuthorId != author.AuthorId))
                        bookEntry.AuthorList.Add(author);
                    
                    if (illustrator != null && bookEntry.IllustratorList.All(i => i.IllustratorId != illustrator.IllustratorId))
                        bookEntry.IllustratorList.Add(illustrator);

                    return bookEntry;
                },
                commandType: CommandType.StoredProcedure,
                splitOn: "AuthorId,IllustratorId"
            );

            return bookDictionary.Values;
        }
    }
}