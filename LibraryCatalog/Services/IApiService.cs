using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using LibraryCatalog.Models;
using RestSharp;

namespace LibraryCatalog.Services;

public interface IApiService
{
    Task<IEnumerable<Book>> GetBooksAsync();
    Task<Book?> GetCheckInBookAsync(int bookId);
    Task<IEnumerable<Checkout>?> GetActiveCheckoutsAsync(int libraryId);
    Task<List<Student>> GetStudentsAsync(int libraryId);
    Task<bool> AddCheckoutAsync(AddCheckoutDto checkoutDto);
    Task<bool> AddCheckInAsync(CheckInDto checkInDto);
    Task<string?> LinkLibraryAsync(string libraryCode);
}