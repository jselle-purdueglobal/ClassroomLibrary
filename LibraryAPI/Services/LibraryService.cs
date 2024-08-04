using LibraryAPI.Models;
using LibraryAPI.Repositories;

namespace LibraryAPI.Services;

public class LibraryService(ILibraryRepository libraryRepository) : ILibraryService
{
    public async Task<Library?> GetLibraryByCodeAsync(string libraryCode)
    {
        var libraries = await libraryRepository.GetLibrariesAsync();
        return libraries.FirstOrDefault(library => library?.LibraryCode == libraryCode);
    }
}