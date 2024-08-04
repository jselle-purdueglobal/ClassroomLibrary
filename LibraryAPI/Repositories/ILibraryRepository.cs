using LibraryAPI.Models;

namespace LibraryAPI.Repositories;

public interface ILibraryRepository
{
    Task<IEnumerable<Library?>> GetLibrariesAsync();
}