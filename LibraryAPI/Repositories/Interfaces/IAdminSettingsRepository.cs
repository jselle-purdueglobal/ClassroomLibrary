namespace LibraryAPI.Repositories.Interfaces;

public interface IAdminSettingsRepository
{
    Task<bool> AuthenticateAdminPin(string pin);
    Task UpdateAdminPin(string newPin);
}