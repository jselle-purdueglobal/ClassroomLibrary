using System.Data;
using Dapper;
using LibraryAPI.Models;
using LibraryAPI.Repositories.Interfaces;

namespace LibraryAPI.Repositories.Implementations;

public class UserRepository(IDbConnection connection) : IUserRepository
{
    public async Task<User> GetUserAsync(string username)
    {
        return (await connection.QuerySingleOrDefaultAsync<User>("spGetUser", new { Username = username }, commandType: CommandType.StoredProcedure))!;
    }
}