namespace ExampleAspireBlazorApiProject.Shared.Models;

public sealed class CourseBookingModel
{
    [Key]
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Student ist Pflicht")]
    public int? StudentId { get; set; }
    
    [Required(ErrorMessage = "Kurs ist Pflicht")]
    public int? TheoryLessonId { get; set; }
    public DateTime? BookingDate { get; set; } = DateTime.Now;

    // Navigation Properties
    [ForeignKey(nameof(StudentId))]
    public StudentModel? Student { get; set; }
    [ForeignKey(nameof(TheoryLessonId))]
    public TheoryLessonModel? TheoryLesson { get; set; }

    public CourseBookingModel() { }

    public CourseBookingModel(CourseBookingModel other)
    {
        Id = other.Id;
        StudentId = other.StudentId;
        TheoryLessonId = other.TheoryLessonId;
        BookingDate = other.BookingDate;
    }
}