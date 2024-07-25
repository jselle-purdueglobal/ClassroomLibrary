using System.Collections.Generic;
using System.Threading.Tasks;
using LibraryManager.Models;

namespace LibraryManager.Services;

public class StudentService(IApiClient apiClient) : IStudentService
{
    public async Task<IEnumerable<Student>> GetLibraryStudentsAsync(int libraryId)
    {
        var students = await apiClient.GetLibraryStudentsAsync(libraryId);
        return students;
    }
    
    public async Task<bool> UpdateStudentNameAsync(int studentId, string firstName, string lastName)
    {
        return await apiClient.UpdateStudentNameAsync(studentId, firstName, lastName);
    }

    public async Task<int> DeleteStudentsAsync(IEnumerable<int> studentIds)
    {
        return await apiClient.DeleteStudentsAsync(studentIds);
    }

    public async Task<bool> AddStudentAsync(Student student)
    {
        return await apiClient.AddStudentAsync(student);
    }
}