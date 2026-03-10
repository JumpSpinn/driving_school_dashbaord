namespace ExampleAspireBlazorApiProject.ApiService.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class StudentController(StudentService studentService, ILogger<StudentController> logger) : ControllerBase
{
    [HttpGet]
    public IActionResult GetAllStudents()
    {
        var allStudents = studentService.GetAllDrivers();
        logger.LogDebug("{c} Students loaded!", allStudents.Count);
        return StatusCode(StatusCodes.Status200OK, allStudents);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateStudentAsync(StudentModel newStudent)
    {
        var student = await studentService.CreateStudentAsync(newStudent);
        logger.LogDebug(student is null ? "Student creation failed!" : "Student #{Id} created!", student?.Id);
        return student is null ?
            BadRequest(StatusCodes.Status400BadRequest) : 
            StatusCode(StatusCodes.Status201Created, student);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStudentAsync(int id)
    {
        var deleted = await studentService.DeleteStudentAsync(id);
        logger.LogDebug("Student #{Id} deleted: {Result}", id, deleted);
        if (!deleted)
            return NotFound($"Fahrschüler mit der ID {id} ist nicht registriert!");
        return NoContent();
    }
    
    [HttpPut]
    public async Task<IActionResult> UpdateStudentAsync(StudentModel editStudent)
    {
        var updated = await studentService.UpdateStudentAsync(editStudent);
        logger.LogDebug("Student #{Id} updated: {Result}", editStudent.Id, updated);
        if (!updated)
            return NotFound($"Fahrschüler mit der ID {editStudent.Id} konnte nicht aktualisiert werden!");
        return NoContent();
    }
    
    [HttpPut]
    [Route("state")]
    public async Task<IActionResult> UpdateStudentActiveStateAsync([FromBody] StudentStateRequest request)
    {
        var updated = await studentService.UpdateStudentAsync(request.StudentId, request.IsActive);
        logger.LogDebug("Student #{Id} state was updated to {NewState}", request.StudentId, updated ? "active" : "inactive");
        if (!updated)
            return NotFound($"Fahrschüler mit der ID {request.StudentId} konnte der Status nicht aktualisiert werden!");
        return NoContent();
    }
}