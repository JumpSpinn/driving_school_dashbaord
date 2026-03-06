namespace ExampleAspireBlazorApiProject.Shared.Models;

public sealed class DriverModel
{
    [Key]
    public int Id { get; set; } // asd
    
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName => $"{FirstName} {LastName}";
    public string Mail { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public int? DrivingSchoolId { get; set; }
    public bool IsDeleted { get; set; }
    
    
    public DriverModel() { }
    public DriverModel(DriverModel other)
    {
        Id = other.Id;
        FirstName = other.FirstName;
        LastName = other.LastName;
        Mail = other.Mail;
        Phone = other.Phone;
        IsActive = other.IsActive;
        DrivingSchoolId = other.DrivingSchoolId;
        IsDeleted = other.IsDeleted;
    }
}