namespace ExampleAspireBlazorApiProject.ApiService.Database;

public sealed partial class ApplicationDbContext
{
    public DbSet<DrivingSchoolModel> DrivingSchools => Set<DrivingSchoolModel>();
    public DbSet<StudentModel> Students => Set<StudentModel>();
    public DbSet<InstructorModel> Instructors => Set<InstructorModel>();
    public DbSet<TheoryLessonModel> TheoryLessons => Set<TheoryLessonModel>();
    public DbSet<CourseBookingModel> CourseBookings => Set<CourseBookingModel>();
}