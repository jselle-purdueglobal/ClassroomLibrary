namespace LibraryCatalog.Models;

public class Student
{
    public int StudentId { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required int LibraryId { get; set; }

    public string FullName => $"{FirstName} {LastName}";
}