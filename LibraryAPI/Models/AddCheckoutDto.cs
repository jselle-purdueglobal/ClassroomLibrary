namespace LibraryAPI.Models;

public class AddCheckoutDto
{
    public int StudentId { get; set; }
    public int BookId { get; set; }
    public int LibraryId { get; set; }
}