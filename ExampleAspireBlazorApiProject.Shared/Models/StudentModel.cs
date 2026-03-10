namespace ExampleAspireBlazorApiProject.Shared.Models;

public sealed class StudentModel
{
    [Key]
    public int Id { get; set; }
    
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName => $"{FirstName} {LastName}";
    public string Mail { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public DateTime? Birthday { get; set; }
    public LicenseClass? License { get; set; }
    public DateTime? EnrollmentDate { get; set; } = DateTime.Now;
    public DateTime? ExamDate { get; set; }
    public bool HasPassed { get; set; }
    public int? DrivingSchoolId { get; set; }
    public bool IsDeleted { get; set; }
    
    
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
        
        DrivingSchoolId = other.DrivingSchoolId;
        IsDeleted = other.IsDeleted;
    }
}