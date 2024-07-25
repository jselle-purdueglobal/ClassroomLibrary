using LibraryAPI.Models;
using LibraryAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class StudentsController(IStudentService studentService) : ControllerBase
{
    // Get Students
    [HttpGet("library/{libraryId:int}")]
    public async Task<ActionResult<IEnumerable<Student>>> GetStudentsByLibrary(int libraryId)
    {
        var students = await studentService.GetLibraryStudentsAsync(libraryId);
        if (!students.Any())
        {
            return NotFound($"No students found for library with ID {libraryId}");
        }
        return Ok(students);
    }
    
    // Update Student
    [HttpPut("{studentId:int}")]
    public async Task<IActionResult> UpdateStudentName(int studentId, [FromBody] StudentNameDto updateDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await studentService.UpdateStudentNameAsync(studentId, updateDto.FirstName, updateDto.LastName);

        if (result)
        {
            return Ok(new { Message = "Student name updated successfully" });
        }
        else
        {
            return NotFound(new { Message = "Student not found or update failed" });
        }
    }
    
    // Delete Students
    [HttpDelete]
    public async Task<IActionResult> DeleteStudents([FromBody] List<int> studentIds)
    {
        if (studentIds.Count == 0)
        {
            return BadRequest("No student IDs provided");
        }

        var deletedCount = await studentService.DeleteStudentsAsync(studentIds);
        
        if (deletedCount == 0)
        {
            return NotFound("No students were found with the provided IDs");
        }

        return Ok(new { DeletedCount = deletedCount, Message = $"Successfully deleted {deletedCount} student(s)" });
    }
    
    // Add Student
    [HttpPost]
    public async Task<ActionResult<Student>> AddStudent([FromBody] Student student)
    {
        var newStudentId = await studentService.AddStudentAsync(student);
        student.StudentId = newStudentId;
        return CreatedAtAction(nameof(GetStudent), new { id = newStudentId }, student);
    }
    
    // Get Student
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Student>> GetStudent(int id)
    {
        var student = await studentService.GetStudentAsync(id);
        if (student == null)
        {
            return NotFound($"Student with ID {id} not found");
        }
        return Ok(student);
    }

}