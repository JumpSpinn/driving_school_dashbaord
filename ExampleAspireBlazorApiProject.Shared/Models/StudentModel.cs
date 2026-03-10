namespace ExampleAspireBlazorApiProject.Shared.Models;

public sealed class StudentModel
{
    [Key]
    public int Id { get; init; }
    
    [Required(ErrorMessage = "Vorname ist Pflicht")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Vorname muss zwischen 3 und 50 Zeichen lang sein")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Nachname ist Pflicht")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Nachname muss zwischen 3 und 50 Zeichen lang sein")]
    public string LastName { get; set; } = string.Empty;
    
    public string FullName => $"{FirstName} {LastName}";
    
    [Required(ErrorMessage = "E-Mail wird benötigt")]
    [EmailAddress(ErrorMessage = "Das ist keine gültige E-Mail Adresse")]
    public string Mail { get; set; } = string.Empty;
    
    public string? Phone { get; set; }
    public DateTime? Birthday { get; set; }
    public LicenseClass? License { get; set; }
    public DateTime? EnrollmentDate { get; set; } = DateTime.Now;
    public DateTime? ExamDate { get; set; }
    public bool HasPassed { get; set; }
    public bool IsDeleted { get; set; }
    
    // Navigator Properties
    public int? DrivingSchoolId { get; set; }
    
    [ForeignKey("DrivingSchoolId")]
    public DrivingSchoolModel? DrivingSchool { get; set; }
    
    public StudentModel() { }
    public StudentModel(StudentModel other)
    {
        Id = other.Id;
        
        FirstName = other.FirstName;
        LastName = other.LastName;
        Mail = other.Mail;
        Phone = other.Phone;
        Birthday = other.Birthday;
        License = other.License;
        EnrollmentDate = other.EnrollmentDate;
        ExamDate = other.ExamDate;
        HasPassed = other.HasPassed;
        IsDeleted = other.IsDeleted;
        
        // Navigator Properties
        DrivingSchoolId = other.DrivingSchoolId;
    }
}