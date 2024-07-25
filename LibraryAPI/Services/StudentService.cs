using LibraryAPI.Models;
using LibraryAPI.Repositories;

namespace LibraryAPI.Services;

public class StudentService(IStudentRepository studentRepository) : IStudentService
{
    // Get Library Students
    public async Task<IEnumerable<Student>> GetLibraryStudentsAsync(int libraryId)
    {
        return await studentRepository.GetLibraryStudentsAsync(libraryId);
    }
    
    // Update Student Name
    public async Task<bool> UpdateStudentNameAsync(int studentId, string firstName, string lastName)
    {
        return await studentRepository.UpdateStudentNameAsync(studentId, firstName, lastName);
    }

    // Delete Students
    public async Task<int> DeleteStudentsAsync(IEnumerable<int> studentIds)
    {
        return await studentRepository.DeleteStudentsAsync(studentIds);
    }
    
    // Add Student
    public async Task<int> AddStudentAsync(Student student)
    {
        return await studentRepository.AddStudentAsync(student);
    }
    
    // Get Student
    public async Task<Student?> GetStudentAsync(int studentId)
    {
        return await studentRepository.GetStudentAsync(studentId);
    }
}