namespace ExampleAspireBlazorApiProject.Web.Interfaces;

public interface ICourseBookingApiClient
{
    public Task<List<CourseBookingModel>?> GetAllCourseBookingsAsync();
    public Task<CourseBookingModel?> CreateCourseBookingAsync(CourseBookingModel newCourseBooking);
    public Task<bool> DeleteCourseBookingAsync(int id);
    public Task<CourseBookingModel?> UpdateCourseBookingAsync(CourseBookingModel editCourseBooking);
}