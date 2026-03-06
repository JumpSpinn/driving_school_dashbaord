namespace ExampleAspireBlazorApiProject.ApiService.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class DriverController(DriverService driverService, ILogger<DriverController> logger) : ControllerBase
{
    [HttpGet]
    public IActionResult GetAllDrivers()
    {
        var allDrivers = driverService.GetAllDrivers();
        logger.LogDebug("{c} Drivers loaded!", allDrivers.Count);
        return StatusCode(StatusCodes.Status200OK, allDrivers);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateDriverAsync(DriverModel newDriver)
    {
        var driver = await driverService.CreateDriverAsync(newDriver);
        logger.LogDebug(driver is null ? "Driver creation failed!" : "Driver #{Id} created!", driver?.Id);
        return driver is null ?
            BadRequest(StatusCodes.Status400BadRequest) : 
            StatusCode(StatusCodes.Status201Created, driver);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDriverAsync(int id)
    {
        var deleted = await driverService.DeleteDriverAsync(id);
        logger.LogDebug("Driver #{Id} deleted: {Result}", id, deleted);
        if (!deleted)
            return NotFound($"Fahrschüler mit der ID {id} ist nicht registriert!");
        return NoContent();
    }
    
    [HttpPut]
    public async Task<IActionResult> UpdateDriverAsync(DriverModel editDriver)
    {
        var updated = await driverService.UpdateDriverAsync(editDriver);
        logger.LogDebug("Driver #{Id} updated: {Result}", editDriver.Id, updated);
        if (!updated)
            return NotFound($"Fahrschüler mit der ID {editDriver.Id} konnte nicht aktualisiert werden!");
        return NoContent();
    }
    
    [HttpPut]
    [Route("state")]
    public async Task<IActionResult> UpdateDriverActiveState([FromBody] DriverStateRequest request)
    {
        var updated = await driverService.UpdateDriverAsync(request.DriverId, request.IsActive);
        logger.LogDebug("Drivers #{Id} state was updated to {NewState}", request.DriverId, updated ? "active" : "inactive");
        if (!updated)
            return NotFound($"Fahrschüler mit der ID {request.DriverId} konnte der Status nicht aktualisiert werden!");
        return NoContent();
    }
}