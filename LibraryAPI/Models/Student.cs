namespace LibraryAPI.Models;

public class Student
{
    public required int? StudentId { get; set; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required int LibraryId { get; init; }
}