namespace LibraryAPI.Models;

public class User
{
    public required string Username { get; init; }
    public required string PasswordHash { get; init; }
    public required string UserRole { get; init; }
    public required bool IsFirstLogin { get; init; }
    public string? RefreshToken { get; init; }
    public DateTime? RefreshTokenExpiryTime { get; init; }
    public required int LibraryId { get; init; }
}