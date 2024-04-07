using LibraryAPI.Repositories.Interfaces;
using LibraryAPI.Services.Interfaces;

namespace LibraryAPI.Services.Implementations;

public class AdminSettingsService(IAdminSettingsRepository repository) : IAdminSettingsService
{
    public async Task<bool> AuthenticateAdminPin(string pin)
    {
        return await repository.AuthenticateAdminPin(pin);
    }

    public async Task UpdateAdminPin(string newPin)
    {
        await repository.UpdateAdminPin(newPin);
    }
}