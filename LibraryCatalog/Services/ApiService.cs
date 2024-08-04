using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using LibraryCatalog.Models;
using RestSharp;

namespace LibraryCatalog.Services;

public class ApiService(string baseUrl, LibraryContext libraryContext) : IApiService
{
    private readonly RestClient _client = new(baseUrl);
    
    // Get Books
    public async Task<IEnumerable<Book>> GetBooksAsync()
    {
        var request = new RestRequest($"api/Books");
        var response = await _client.ExecuteAsync<IEnumerable<Book>>(request);

        if (response.IsSuccessful)
        {
            return response.Data ?? [];
        }

        throw new HttpRequestException($"Error fetching books: {response.ErrorMessage}");
    }
    
    // Get Check In Book
    public async Task<Book?> GetCheckInBookAsync(int bookId)
    {
        var books = await GetBooksAsync();
        return books.FirstOrDefault(book => book.BookId == bookId)!;
    }
    
    // Get Active Checkouts
    public async Task<IEnumerable<Checkout>?> GetActiveCheckoutsAsync(int libraryId)
    {
        var request = new RestRequest($"api/Checkouts/Active/{libraryId}");
        RestResponse<IEnumerable<Checkout>?> response = await _client.ExecuteAsync<IEnumerable<Checkout>>(request);

        if (response.IsSuccessful)
        {
            return response.Data ?? [];
        }

        throw new HttpRequestException($"Error fetching checkouts: {response.ErrorMessage}");
    }
    
    // Get Students
    public async Task<List<Student>> GetStudentsAsync(int libraryId)
    {
        var request = new RestRequest($"api/students/library/{libraryId}");
        request.AddHeader("Authorization", $"Bearer {libraryContext.AccessToken}");

        var response = await _client.ExecuteAsync<List<Student>>(request);

        if (response.IsSuccessful)
        {
            return response.Data ?? [];
        }

        throw new HttpRequestException($"Error fetching students: {response.ErrorMessage}");
    }
    
    // Add Checkout
    public async Task<bool> AddCheckoutAsync(AddCheckoutDto checkoutDto)
    {
        var request = new RestRequest("api/Checkouts", Method.Post);
        request.AddJsonBody(checkoutDto);
        var response = await _client.ExecuteAsync<bool>(request);

        return response.IsSuccessful;
    }
    
    // Add Check In
    public async Task<bool> AddCheckInAsync(CheckInDto checkInDto)
    {
        var request = new RestRequest("api/Checkouts/Return", Method.Post);
        request.AddJsonBody(checkInDto);
        var response = await _client.ExecuteAsync<bool>(request);

        return response.IsSuccessful;
    }
    
    // Link Library
    public async Task<string?> LinkLibraryAsync(string libraryCode)
    {
        var request = new RestRequest("api/auth/link", Method.Post);
        request.AddJsonBody(new {LibraryCode = libraryCode});

        var response = await _client.ExecuteAsync<string?>(request);

        return response.IsSuccessful ? response.Data! : null;
    }
}