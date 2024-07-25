using LibraryAPI.Models;

namespace LibraryAPI.Services;

public interface IStudentService
{
    Task<IEnumerable<Student>> GetLibraryStudentsAsync(int libraryId);
    Task<bool> UpdateStudentNameAsync(int studentId, string firstName, string lastName);
    Task<int> DeleteStudentsAsync(IEnumerable<int> studentIds);
    Task<int> AddStudentAsync(Student student);
    Task<Student?> GetStudentAsync(int studentId);
}