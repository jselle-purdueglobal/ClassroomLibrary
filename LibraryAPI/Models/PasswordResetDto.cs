namespace LibraryAPI.Models;

public class PasswordResetDto
{
    public required string Username { get; set; }
    public required string NewPassword { get; set; }
}