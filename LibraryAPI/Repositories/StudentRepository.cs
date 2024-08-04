using System.Data;
using Dapper;
using LibraryAPI.Models;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

namespace LibraryAPI.Repositories;

public class StudentRepository(IDbConnection connection) : IStudentRepository
{
    // Get Students
    public async Task<IEnumerable<Student>> GetLibraryStudentsAsync(int libraryId)
    {
        return await connection.QueryAsync<Student>(
            "spGetLibraryStudents", 
            new { LibraryId = libraryId }, 
            commandType: CommandType.StoredProcedure);
    }
    
    // Update Student
    public async Task<bool> UpdateStudentNameAsync(int studentId, string firstName, string lastName)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@StudentId", studentId);
        parameters.Add("@FirstName", firstName);
        parameters.Add("@LastName", lastName);

        var result = await connection.ExecuteScalarAsync<int>(
            "spUpdateStudentName",
            parameters,
            commandType: CommandType.StoredProcedure
        );

        return result > 0;
    }
    
    // Delete Students
    public async Task<bool> DeleteStudentAsync(int studentId)
    {
        var result = await connection.ExecuteScalarAsync<int>(
            "spDeleteStudent",
            new { StudentId = studentId },
            commandType: CommandType.StoredProcedure);
        
        return result == 1;
    }

    // Add Student
    public async Task<int> AddStudentAsync(Student student)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@FirstName", student.FirstName);
        parameters.Add("@LastName", student.LastName);
        parameters.Add("@LibraryId", student.LibraryId);

        var newStudentId = await connection.ExecuteScalarAsync<int>(
            "spAddStudent",
            parameters,
            commandType: CommandType.StoredProcedure
        );

        return newStudentId;
    }
    
    // Get Student
    public async Task<Student?> GetStudentAsync(int studentId)
    {
        return await connection.QuerySingleOrDefaultAsync<Student>(
            "spGetStudent", 
            new { StudentId = studentId },
            commandType: CommandType.StoredProcedure);
    }
}