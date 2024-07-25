using System.Data;
using Dapper;
using LibraryAPI.Models;
using LibraryAPI.Repositories.Interfaces;

namespace LibraryAPI.Repositories.Implementations;

public class UserRepository(IDbConnection connection) : IUserRepository
{
    public async Task<User?> GetUserAsync(string username)
    {
        return await connection.QuerySingleOrDefaultAsync<User>(
            "spGetUser", 
            new { Username = username }, 
            commandType: CommandType.StoredProcedure);
    }

    public async Task<User?> GetUserByRefreshTokenAsync(string refreshToken)
    {
        return await connection.QuerySingleOrDefaultAsync<User>(
            "spGetUserByRefreshToken", 
            new { RefreshToken = refreshToken }, 
            commandType: CommandType.StoredProcedure);
    }

    public async Task SaveRefreshTokenAsync(string username, string refreshToken, DateTime expiryTime)
    {
        await connection.ExecuteAsync(
            "spSaveRefreshToken", 
            new { Username = username, RefreshToken = refreshToken, ExpiryTime = expiryTime }, 
            commandType: CommandType.StoredProcedure);
    }
    
    public async Task<bool> UpdatePasswordAsync(string username, string newPasswordHash)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Username", username);
        parameters.Add("@NewPasswordHash", newPasswordHash);

        var result = await connection.ExecuteScalarAsync<int>(
            "spUpdateUserPassword",
            parameters,
            commandType: CommandType.StoredProcedure
        );

        return result > 0;
    }
}