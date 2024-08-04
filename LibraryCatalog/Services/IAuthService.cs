using System.Threading.Tasks;

namespace LibraryCatalog.Services;

public interface IAuthService
{
    Task<bool> LinkLibraryAsync(string libraryCode);
    Task<bool> ValidateLibraryLinkAsync();
}