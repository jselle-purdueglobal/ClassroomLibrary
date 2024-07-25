namespace LibraryManager.Models;

public class AuthResult
{
    public required string Token { get; set; }
    public required string RefreshToken { get; set; }
    public required bool IsFirstLogin { get; set; }
}