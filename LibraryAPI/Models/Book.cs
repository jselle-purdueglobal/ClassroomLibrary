namespace LibraryAPI.Models;

public class Book
{
    public int BookId { get; set; }
    public string Title { get; set; } = null!;
    public List<Author> Authors { get; set; } = null!;
}