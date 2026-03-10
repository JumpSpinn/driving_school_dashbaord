namespace ExampleAspireBlazorApiProject.ApiService.Database;

public sealed partial class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<StudentModel>().Navigation(d => d.DrivingSchool).AutoInclude();
        modelBuilder.Entity<TheoryLessonModel>().Navigation(d => d.Instructor).AutoInclude();
        modelBuilder.Entity<CourseBookingModel>().Navigation(d => d.Student).AutoInclude();
        modelBuilder.Entity<CourseBookingModel>().Navigation(d => d.TheoryLesson).AutoInclude();
    }
}