namespace ExampleAspireBlazorApiProject.ApiService.Services;

public sealed class CourseBookingService(ApplicationDbContext dbContext, ILogger<CourseBookingService> logger)
{
    public List<CourseBookingModel> GetCourseBookings()
    {
        try
        {
            return dbContext.CourseBookings
                .Include(d => d.Student)
                .Include(d => d.TheoryLesson)
                .ToList();
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error getting course bookings");
        }

        return [];
    }
    
    private CourseBookingModel? GetCourseBooking(int studentId, int theoryLessonId) 
        => dbContext.CourseBookings
            .Include(d => d.Student)
            .Include(d => d.TheoryLesson)
            .FirstOrDefault(x => x.StudentId == studentId && x.TheoryLessonId == theoryLessonId);
    
    private CourseBookingModel? GetCourseBooking(int id) 
        => dbContext.CourseBookings
            .Include(d => d.Student)
            .Include(d => d.TheoryLesson)
            .FirstOrDefault(x => x.Id == id);
    
    public async Task<CourseBookingModel?> CreateCourseBookingAsync(CourseBookingModel booking)
    {
        try
        {
            if (booking.StudentId is null && booking.TheoryLessonId is null) return null;
            
            var exist = GetCourseBooking((int)booking.StudentId!, (int)booking.TheoryLessonId!);
            if (exist is not null) return exist;

            var courseBooking = new CourseBookingModel
            {
                StudentId = booking.StudentId,
                TheoryLessonId = booking.TheoryLessonId,
                BookingDate = booking.BookingDate
            };
            
            dbContext.CourseBookings.Add(courseBooking);
            await dbContext.SaveChangesAsync();
            
            await dbContext.Entry(courseBooking).Reference(x => x.Student).LoadAsync();
            await dbContext.Entry(courseBooking).Reference(x => x.TheoryLesson).LoadAsync();
            
            return courseBooking;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error creating course booking");
        }

        return null;
    }
    
    public async Task<CourseBookingModel?> UpdateCourseBookingAsync(CourseBookingModel toUpdate)
    {
        try
        {
            var courseBooking = GetCourseBooking(toUpdate.Id);
            if (courseBooking is null) return null;
            
            courseBooking.StudentId = toUpdate.StudentId;
            courseBooking.TheoryLessonId = toUpdate.TheoryLessonId;
            courseBooking.BookingDate = toUpdate.BookingDate;
            
            await dbContext.SaveChangesAsync();
            
            await dbContext.Entry(courseBooking).Reference(x => x.Student).LoadAsync();
            await dbContext.Entry(courseBooking).Reference(x => x.TheoryLesson).LoadAsync();
            
            return courseBooking;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error updating course booking");
        }

        return null;
    }
    
    public async Task<bool> DeleteCourseBookingAsync(int id)
    {
        try
        {
            var courseBooking = GetCourseBooking(id);
            if (courseBooking is null) return false;
            
            dbContext.CourseBookings.Remove(courseBooking);
            await dbContext.SaveChangesAsync();
            
            return true;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error deleting course booking");
        }

        return false;
    }
}