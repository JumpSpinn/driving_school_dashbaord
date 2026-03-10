namespace ExampleAspireBlazorApiProject.Shared.Models;

public sealed class TheoryLessonModel
{
    [Key]
    public int Id { get; init; }

    [Required(ErrorMessage = "Kursname ist Pflicht")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Kursname muss zwischen 3 und 50 Zeichen lang sein")]
    public string Name { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Thema ist Pflicht")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Thema muss zwischen 3 und 50 Zeichen lang sein")]
    public string Topic { get; set; } = string.Empty;
    
    public DayOfWeek DayOfWeek { get; set; }
    public TimeOnly StartTime { get; set; }
    public int DurationMinutes { get; set; }
    public int MaxStudents { get; set; }
    
    [Column(TypeName = "decimal(10,2)")]
    public decimal Price { get; set; }
    
    public bool IsBasic { get; set; }
    public bool IsDeleted { get; set; }
    
    // Navigator Properties
    public int? InstructorId { get; set; }
    [ForeignKey(nameof(InstructorId))]
    public InstructorModel? Instructor { get; set; }
    
    
    public TheoryLessonModel() { }
    public TheoryLessonModel(TheoryLessonModel other)
    {
        Id = other.Id;
        
        Name = other.Name;
        Topic = other.Topic;
        DayOfWeek = other.DayOfWeek;
        StartTime = other.StartTime;
        DurationMinutes = other.DurationMinutes;
        MaxStudents = other.MaxStudents;
        Price = other.Price;
        IsBasic = other.IsBasic;
        IsDeleted = other.IsDeleted;
        
        // Navigator Properties
        InstructorId = other.InstructorId;
        Instructor = other.Instructor;
    }
}