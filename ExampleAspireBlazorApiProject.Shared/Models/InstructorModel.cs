namespace ExampleAspireBlazorApiProject.Shared.Models;

public sealed class InstructorModel
{
    [Key] 
    public int Id { get; set; }

    [Required(ErrorMessage = "Vorname ist Pflicht")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Vorname muss zwischen 3 und 50 Zeichen lang sein")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Nachname ist Pflicht")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Nachname muss zwischen 3 und 50 Zeichen lang sein")]//
    public string LastName { get; set; } = string.Empty;

    public string FullName => $"{FirstName} {LastName}";

    [Required(ErrorMessage = "E-Mail wird benötigt")]
    [EmailAddress(ErrorMessage = "Das ist keine gültige E-Mail Adresse")]
    public string Mail { get; set; } = string.Empty;

    public string? Phone { get; set; }
    public bool IsDeleted { get; set; }

    // Navigation Properties
    public ICollection<TheoryLessonModel> TheoryLessons { get; set; } = [];


    public InstructorModel() { }
    public InstructorModel(InstructorModel other)
    {
        Id = other.Id;
        
        FirstName = other.FirstName;
        LastName = other.LastName;
        Mail = other.Mail;
        Phone = other.Phone;
        IsDeleted = other.IsDeleted;
        TheoryLessons = other.TheoryLessons
            .Select(x => new TheoryLessonModel(x))
            .ToList();
    }
}