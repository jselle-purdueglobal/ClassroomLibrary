using System.Data;
using Dapper;
using LibraryAPI.Models;

namespace LibraryAPI.Repositories;

public class LibraryRepository(IDbConnection connection) : ILibraryRepository
{
    public async Task<IEnumerable<Library?>> GetLibrariesAsync()
    {
        return await connection.QueryAsync<Library>(
            "spGetLibraries",
            commandType: CommandType.StoredProcedure);
    }
}