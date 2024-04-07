using System.Data;
using Dapper;
using LibraryAPI.Repositories.Interfaces;

namespace LibraryAPI.Repositories.Implementations;

public class AdminSettingsRepository(IDbConnection connection) : IAdminSettingsRepository
{
    public async Task<bool> AuthenticateAdminPin(string pin)
    {
        var result = await connection.QuerySingleOrDefaultAsync<int>("AuthenticateAdminPIN", new { InputPIN = pin }, commandType: CommandType.StoredProcedure);
        return result == 1;
    }

    public async Task UpdateAdminPin(string newPin)
    {
        await connection.ExecuteAsync("UpdateAdminPIN", new { NewPIN = newPin }, commandType: CommandType.StoredProcedure);
    }
}