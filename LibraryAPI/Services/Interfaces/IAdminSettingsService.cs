namespace LibraryAPI.Services.Interfaces;

public interface IAdminSettingsService
{
    Task<bool> AuthenticateAdminPin(string pin);
    Task UpdateAdminPin(string newPin);
}