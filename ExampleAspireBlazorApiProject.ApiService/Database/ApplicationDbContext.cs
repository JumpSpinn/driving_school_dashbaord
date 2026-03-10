namespace ExampleAspireBlazorApiProject.ApiService.Database;

public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<DrivingSchoolModel> DrivingSchools { get; set; }
    public DbSet<StudentModel> Students { get; set; }
    public DbSet<InstructorModel> Instructors { get; set; }
    public DbSet<TheoryLessonModel> TheoryLessons { get; set; }
    public DbSet<CourseBookingModel> CourseBookings { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DrivingSchoolModel>()
            .HasMany(d => d.Students)
            .WithOne()
            .HasForeignKey(s => s.DrivingSchoolId);
        
        modelBuilder.Entity<TheoryLessonModel>()
            .HasOne(t => t.Instructor)
            .WithMany(i => i.TheoryLessons)
            .HasForeignKey(t => t.InstructorId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<CourseBookingModel>()
            .HasOne(b => b.Student)
            .WithMany()
            .HasForeignKey(b => b.StudentId);

        modelBuilder.Entity<CourseBookingModel>()
            .HasOne(b => b.TheoryLesson)
            .WithMany()
            .HasForeignKey(b => b.TheoryLessonId);
    }
}