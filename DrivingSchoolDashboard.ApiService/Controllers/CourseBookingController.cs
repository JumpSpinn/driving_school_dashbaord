namespace DrivingSchoolDashboard.ApiService.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class CourseBookingController(CourseBookingService courseBookingService, ILogger<CourseBookingController> logger) : ControllerBase
{
    [HttpGet]
    public IActionResult GetAllCourseBookings()
    {
        var allBookings = courseBookingService.GetCourseBookings();
        logger.LogDebug("{c} Course Bookings loaded!", allBookings.Count);
        return StatusCode(StatusCodes.Status200OK, allBookings);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateCourseBookingAsync(CourseBookingModel newCourseBooking)
    {
        var courseBooking = await courseBookingService.CreateCourseBookingAsync(newCourseBooking);
        logger.LogDebug(courseBooking is null ? "Course booking creation failed!" : "Course booking #{Id} created!", courseBooking?.Id);
        return courseBooking is null ?
            BadRequest(StatusCodes.Status400BadRequest) : 
            StatusCode(StatusCodes.Status201Created, courseBooking);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCourseBookingAsync(int id)
    {
        var deleted = await courseBookingService.DeleteCourseBookingAsync(id);
        logger.LogDebug("Course booking #{Id} deleted: {Result}", id, deleted);
        if (!deleted)
            return NotFound($"Kursbuchung mit der ID {id} ist nicht registriert!");
        return NoContent();
    }
    
    [HttpPut]
    public async Task<IActionResult> UpdateCourseBookingAsync(CourseBookingModel editCourseBooking)
    {
        var courseBooking = await courseBookingService.UpdateCourseBookingAsync(editCourseBooking);
        logger.LogDebug("Course booking #{Id} updated: {Result}", editCourseBooking.Id, courseBooking is not null);
        if (courseBooking is null)
            return NotFound($"Kursbuchung mit der ID {editCourseBooking.Id} konnte nicht aktualisiert werden!");
        return StatusCode(StatusCodes.Status201Created, courseBooking);
    }
}