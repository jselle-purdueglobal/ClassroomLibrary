using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using LibraryManager.Models;
using RestSharp;

namespace LibraryManager.Services;

public class ApiClient(string baseUrl) : IApiClient
{
    // Fields
    private readonly RestClient _client = new(baseUrl);
    private string _username = string.Empty;
    private string _accessToken = string.Empty;
    
    // Login
    public async Task<AuthResult> LoginAsync(string username, string password)
    {
        var request = new RestRequest("api/auth/login", Method.Post);
        request.AddJsonBody(new { Username = username, Password = password });

        var response = await _client.ExecuteAsync<AuthResult>(request);

        if (response.IsSuccessful)
        {
            _username = username;
            _accessToken = response.Data!.Token;
            return response.Data!;
        }
        
        // Unauthorized Check
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            throw new HttpRequestException("Incorrect username or password");
        }

        throw new HttpRequestException("System error");
    }
    
    // Refresh Token
    public async Task<AuthResult> RefreshTokenAsync(string refreshToken)
    {
        var request = new RestRequest("api/auth/refresh", Method.Post);
        request.AddJsonBody(new { RefreshToken = refreshToken });

        var response = await _client.ExecuteAsync<AuthResult>(request);

        if (!response.IsSuccessful) throw new HttpRequestException($"Error refreshing token: {response.ErrorMessage}");
        _accessToken = response.Data!.Token;
        return response.Data!;

    }

    // Reset Password
    public async Task<bool> ResetPasswordAsync(string newPassword)
    {
        var request = new RestRequest("api/auth/reset-password", Method.Post);
        request.AddHeader("Authorization", $"Bearer {_accessToken}");
        request.AddJsonBody(new { Username = _username, NewPassword = newPassword });

        var response = await _client.ExecuteAsync(request);

        if (response.IsSuccessful)
        {
            return true;
        }

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            throw new HttpRequestException("Unauthorized. Please log in again.");
        }

        throw new HttpRequestException($"Failed to reset password: {response.ErrorMessage}");
    }
    
    // Get Students
    public async Task<IEnumerable<Student>> GetLibraryStudentsAsync(int libraryId)
    {
        var request = new RestRequest($"api/students/library/{libraryId}", Method.Get);
        request.AddHeader("Authorization", $"Bearer {_accessToken}");

        var response = await _client.ExecuteAsync<List<Student>>(request);

        if (response.IsSuccessful)
        {
            return response.Data ?? [];
        }

        throw new HttpRequestException($"Error fetching students: {response.ErrorMessage}");
    }
    
    // Edit Student
    public async Task<bool> UpdateStudentNameAsync(int studentId, string firstName, string lastName)
    {
        var request = new RestRequest($"api/students/{studentId}", Method.Put);
        request.AddHeader("Authorization", $"Bearer {_accessToken}");
        request.AddJsonBody(new { FirstName = firstName, LastName = lastName });

        var response = await _client.ExecuteAsync(request);

        if (response.IsSuccessful)
        {
            return true;
        }

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            throw new HttpRequestException("Unauthorized. Please log in again.");
        }

        throw new HttpRequestException($"Failed to update student: {response.ErrorMessage}");
    }
    
    // Delete Students
    public async Task<int> DeleteStudentsAsync(IEnumerable<int> studentIds)
    {
        var request = new RestRequest($"api/students", Method.Delete);
        request.AddHeader("Authorization", $"Bearer {_accessToken}");
        request.AddJsonBody(studentIds);
        
        var response = await _client.ExecuteAsync<DeleteResult>(request);

        if (response.IsSuccessful)
        {
            return response.Data?.DeletedCount ?? 0;
        }

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            throw new HttpRequestException("Unauthorized. Please log in again.");
        }

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return 0; // No students were found with the provided IDs
        }

        throw new HttpRequestException($"Failed to delete students: {response.ErrorMessage}");
    }

    public async Task<bool> AddStudentAsync(Student student)
    {
        var request = new RestRequest($"api/students", Method.Post);
        request.AddHeader("Authorization", $"Bearer {_accessToken}");
        request.AddJsonBody(student);
        
        var response = await _client.ExecuteAsync(request);
        if (response.IsSuccessful)
        {
            return true;
        }

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            throw new HttpRequestException("Unauthorized. Please log in again.");
        }

        throw new HttpRequestException($"Failed to update student: {response.ErrorMessage}");
    }
}