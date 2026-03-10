namespace ExampleAspireBlazorApiProject.ApiService.Database.Configurations;

public class CourseBookingConfiguration : IEntityTypeConfiguration<CourseBookingModel>
{
    public void Configure(EntityTypeBuilder<CourseBookingModel> builder)
    {
        builder.HasOne(b => b.Student)
            .WithMany() 
            .HasForeignKey(b => b.StudentId);

        builder.HasOne(b => b.TheoryLesson)
            .WithMany()
            .HasForeignKey(b => b.TheoryLessonId);
    }
}