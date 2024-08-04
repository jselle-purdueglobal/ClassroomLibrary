namespace LibraryAPI.Models;

public class Book
{
    public int BookId { get; init; }
    public required string Title { get; init; }
    public string ImagePath { get; init; } = string.Empty;
    public List<Author> AuthorList { get; set; } = [];
    public List<Illustrator> IllustratorList { get; set; } = [];
}