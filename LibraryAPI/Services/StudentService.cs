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
    public async Task<bool> DeleteStudentsAsync(List<int> studentIds)
    {
        var totalDeleteRequests = studentIds.Count;
        var totalDeleted = 0;
        foreach (var studentId in studentIds)
        {
            var result = await studentRepository.DeleteStudentAsync(studentId);
            if (result) totalDeleted++;
        }

        return totalDeleted == totalDeleteRequests;
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