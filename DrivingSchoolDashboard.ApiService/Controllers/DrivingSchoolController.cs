namespace DrivingSchoolDashboard.ApiService.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class DrivingSchoolController(DrivingSchoolService drivingSchoolService, ILogger<DrivingSchoolController> logger) : ControllerBase
{
    [HttpGet]
    public IActionResult GetAllDrivingSchools()
    {
        var allDrivingSchools = drivingSchoolService.GetAllDrivingSchools();
        logger.LogDebug("{c} Driving Schools loaded!", allDrivingSchools.Count);
        return StatusCode(StatusCodes.Status200OK, allDrivingSchools);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateDrivingSchool(DrivingSchoolModel newDrivingSchool)
    {
        var drivingSchool = await drivingSchoolService.CreateDrivingSchoolAsync(newDrivingSchool);
        logger.LogDebug(drivingSchool is null ? "Driving School creation failed!" : "Driving School #{Id} created!", drivingSchool?.Id);
        return drivingSchool is null ?
            BadRequest(StatusCodes.Status400BadRequest) : 
            StatusCode(StatusCodes.Status201Created, drivingSchool);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDrivingSchool(int id)
    {
        var deleted = await drivingSchoolService.DeleteDrivingSchoolAsync(id);
        logger.LogDebug("Driving School #{Id} deleted: {Result}", id, deleted);
        if (!deleted)
            return NotFound("Fahrschule konnte nicht gefunden werden!");
        return NoContent();
    }

    [HttpPut]
    public async Task<IActionResult> UpdateDrivingSchoolAsync(DrivingSchoolModel editDrivingSchool)
    {
        var updated = await drivingSchoolService.UpdateDrivingSchoolAsync(editDrivingSchool);
        logger.LogDebug("Driving School #{Id} updated: {Result}", editDrivingSchool.Id, updated);
        if (updated is null)
            return NotFound($"Fahrschule mit der ID {editDrivingSchool.Id} konnte nicht aktualisiert werden!");
        return StatusCode(StatusCodes.Status201Created, updated);
    }
}