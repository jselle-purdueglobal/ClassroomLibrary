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
    public async Task<int> DeleteStudentsAsync(IEnumerable<int> studentIds)
    {
        var dataTable = new DataTable();
        dataTable.Columns.Add("Value", typeof(int));
        foreach (var id in studentIds)
        {
            dataTable.Rows.Add(id);
        }

        var parameter = new DynamicParameters();
        parameter.Add("@StudentIds", dataTable.AsTableValuedParameter("IntList"));
        
        var result = await connection.ExecuteScalarAsync<int>(
            "spDeleteStudents",
            parameter,
            commandType: CommandType.StoredProcedure);
        
        return result;
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