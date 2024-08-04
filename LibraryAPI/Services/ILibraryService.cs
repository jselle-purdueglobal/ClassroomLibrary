using LibraryAPI.Models;

namespace LibraryAPI.Services;

public interface ILibraryService
{
    Task<Library?> GetLibraryByCodeAsync(string libraryCode);
}