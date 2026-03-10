namespace ExampleAspireBlazorApiProject.ApiService.Database.Configurations;

public class TheoryLessonConfiguration : IEntityTypeConfiguration<TheoryLessonModel>
{
    public void Configure(EntityTypeBuilder<TheoryLessonModel> builder)
    {
        builder.HasOne(t => t.Instructor)
            .WithMany(i => i.TheoryLessons)
            .HasForeignKey(t => t.InstructorId)
            .OnDelete(DeleteBehavior.SetNull);
        
        builder.Property(t => t.Price).HasPrecision(10, 2);
    }
}