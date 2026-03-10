namespace ExampleAspireBlazorApiProject.Shared.Models;

public sealed class CourseBookingModel
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int StudentId { get; set; }

    [Required]
    public int TheoryLessonId { get; set; }

    [Required]
    public DateTime BookingDate { get; set; } = DateTime.Now;

    // Navigation Properties
    public StudentModel? Student { get; set; }
    public TheoryLessonModel? TheoryLesson { get; set; }
}