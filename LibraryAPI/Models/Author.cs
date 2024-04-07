namespace LibraryAPI.Models;

public class Author
{
    private int AuthorId { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
}