namespace ExampleAspireBlazorApiProject.Shared.Models;

public sealed class CourseBookingModel
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int StudentId { get; set; }

    [Required]
    public int TheoryLessonId { get; set; }
    
    public DateTime BookingDate { get; set; } = DateTime.Now;

    // Navigation Properties
    [ForeignKey(nameof(StudentId))]
    public StudentModel? Student { get; set; }
    [ForeignKey(nameof(TheoryLessonId))]
    public TheoryLessonModel? TheoryLesson { get; set; }
}