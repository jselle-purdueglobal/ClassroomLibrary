namespace LibraryAPI.Models;

public class AddStudentDto
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required int LibraryId { get; set; }
}