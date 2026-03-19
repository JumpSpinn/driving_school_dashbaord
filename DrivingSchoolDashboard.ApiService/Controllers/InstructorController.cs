namespace DrivingSchoolDashboard.ApiService.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class InstructorController(InstructorService instructorService, ILogger<InstructorController> logger) : ControllerBase
{
    [HttpGet]
    public IActionResult GetAllInstructors()
    {
        var allInstructors = instructorService.GetInstructors();
        logger.LogDebug("{c} Instructors loaded!", allInstructors.Count);
        return StatusCode(StatusCodes.Status200OK, allInstructors);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateInstructorAsync(InstructorModel newInstructor)
    {
        var instructor = await instructorService.CreateInstructorAsync(newInstructor);
        logger.LogDebug(instructor is null ? "Instructor creation failed!" : "Instructor #{Id} created!", instructor?.Id);
        return instructor is null ?
            BadRequest(StatusCodes.Status400BadRequest) : 
            StatusCode(StatusCodes.Status201Created, instructor);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteInstructorAsync(int id)
    {
        var deleted = await instructorService.DeleteInstructorAsync(id);
        logger.LogDebug("Instructor #{Id} deleted: {Result}", id, deleted);
        if (!deleted)
            return NotFound($"Fahrlehrer mit der ID {id} ist nicht registriert!");
        return NoContent();
    }
    
    [HttpPut]
    public async Task<IActionResult> UpdateInstructorAsync(InstructorModel editInstructor)
    {
        var instructor = await instructorService.UpdateInstructorAsync(editInstructor);
        logger.LogDebug("Instructor #{Id} updated: {Result}", editInstructor.Id, instructor is not null);
        if (instructor is null)
            return NotFound($"Fahrlehrer mit der ID {editInstructor.Id} konnte nicht aktualisiert werden!");
        return StatusCode(StatusCodes.Status201Created, instructor);
    }
}