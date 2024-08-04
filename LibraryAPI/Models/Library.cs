namespace LibraryAPI.Models;

public class Library
{
    public int LibraryId { get; init; }
    public required string LibraryCode { get; set; }
    public required string LibraryName { get; set; }
}