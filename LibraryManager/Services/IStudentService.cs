using System.Collections.Generic;
using System.Threading.Tasks;
using LibraryManager.Models;

namespace LibraryManager.Services;

public interface IStudentService
{
    Task<IEnumerable<Student>> GetLibraryStudentsAsync(int libraryId);
    Task<bool> UpdateStudentNameAsync(int studentId, string firstName, string lastName);
    Task<int> DeleteStudentsAsync(IEnumerable<int> studentIds);
    Task<bool> AddStudentAsync(Student student);
}