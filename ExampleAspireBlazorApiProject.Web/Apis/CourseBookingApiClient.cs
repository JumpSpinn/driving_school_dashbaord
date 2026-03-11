namespace ExampleAspireBlazorApiProject.Web.Apis;

public class CourseBookingApiClient(HttpClient client, ILogger<CourseBookingApiClient> logger)
{
    public async Task<List<CourseBookingModel>?> GetAllCourseBookingsAsync()
    {
        var result = await client.GetFromJsonAsync<List<CourseBookingModel>>("api/CourseBooking");
        if (result is null)
        {
            logger.LogError("no course bookings registered!");
            return null;
        }

        logger.LogInformation($"{result.Count} registered course bookings loaded!");
        return result;
    }

    public async Task<CourseBookingModel?> CreateCourseBookingAsync(CourseBookingModel newCourseBooking)
    {
        var result = await client.PostAsJsonAsync("api/CourseBooking", newCourseBooking);
        
        var courseBooking = await result.Content.ReadFromJsonAsync<CourseBookingModel>();
        if (courseBooking is not null) return courseBooking;
        
        logger.LogError("error creating course booking!");
        return null;
    }

    public async Task<bool> DeleteCourseBookingAsync(int id)
    {
        var result = await client.DeleteAsync($"api/CourseBooking/{id}");
        logger.LogInformation($"Web API => course booking with id {id} deleted! - Success: {result.IsSuccessStatusCode}");
        return result.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateCourseBookingAsync(CourseBookingModel editCourseBooking)
    {
        var result = await client.PutAsJsonAsync($"api/CourseBooking/", editCourseBooking);
        logger.LogInformation($"Web API => course booking with id {editCourseBooking.Id} updated! - Success: {result.IsSuccessStatusCode}");
        return result.IsSuccessStatusCode;
    }
}