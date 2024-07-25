namespace LibraryAPI.Models;

public class AuthResult
{
    public string? Token { get; set; }
    
    public string? RefreshToken { get; set; }
    public bool IsFirstLogin { get; set; }
}