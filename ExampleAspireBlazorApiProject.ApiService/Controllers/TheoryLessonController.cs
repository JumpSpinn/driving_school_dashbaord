namespace ExampleAspireBlazorApiProject.ApiService.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class TheoryLessonController(TheoryLessonService theoryLessonService, ILogger<TheoryLessonController> logger) : ControllerBase
{
    [HttpGet]
    public IActionResult GetAllLessons()
    {
        var allLessons = theoryLessonService.GetTheoryLessons();
        logger.LogDebug("{c} Lessons loaded!", allLessons.Count);
        return StatusCode(StatusCodes.Status200OK, allLessons);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateLessonAsync(TheoryLessonModel newLesson)
    {
        var lesson = await theoryLessonService.CreateTheoryLessonAsync(newLesson);
        logger.LogDebug(lesson is null ? "Lesson creation failed!" : "Lesson #{Id} created!", lesson?.Id);
        return lesson is null ?
            BadRequest(StatusCodes.Status400BadRequest) : 
            StatusCode(StatusCodes.Status201Created, lesson);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLessonAsync(int id)
    {
        var deleted = await theoryLessonService.DeleteTheoryLessonAsync(id);
        logger.LogDebug("Lesson #{Id} deleted: {Result}", id, deleted);
        if (!deleted)
            return NotFound($"Theorieunterricht mit der ID {id} ist nicht registriert!");
        return NoContent();
    }
    
    [HttpPut]
    public async Task<IActionResult> UpdateLessonAsync(TheoryLessonModel editLesson)
    {
        var lesson = await theoryLessonService.UpdateTheoryLessonAsync(editLesson);
        logger.LogDebug("Lesson #{Id} updated: {Result}", editLesson.Id, lesson is not null);
        if (lesson is null)
            return NotFound($"Theorieunterricht mit der ID {editLesson.Id} konnte nicht aktualisiert werden!");
        return StatusCode(StatusCodes.Status201Created, lesson);
    }
}