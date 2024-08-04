using LibraryAPI.Models;

namespace LibraryAPI.Repositories;

public interface IStudentRepository
{
    Task<IEnumerable<Student>> GetLibraryStudentsAsync(int libraryId);
    Task<bool> UpdateStudentNameAsync(int studentId, string firstName, string lastName);
    Task<bool> DeleteStudentAsync(int studentId);
    Task<int> AddStudentAsync(Student student);
    Task<Student?> GetStudentAsync(int studentId);
}