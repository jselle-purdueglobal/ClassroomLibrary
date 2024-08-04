namespace LibraryAPI.Models;

public class BookDto
{
    public int BookId { get; init; }
    public required string Title { get; init; }
    public string Authors { get; init; } = string.Empty;
    public string Illustrators { get; init; } = string.Empty;
    public string ImagePath { get; init; } = string.Empty;
}