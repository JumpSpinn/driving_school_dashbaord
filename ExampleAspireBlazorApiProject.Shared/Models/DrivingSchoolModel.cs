namespace ExampleAspireBlazorApiProject.Shared.Models;

public sealed class DrivingSchoolModel
{
    [Key]
    public int Id { get; init; }
    
    [Required(ErrorMessage = "Bezeichnung ist Pflicht")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Bezeichnung muss zwischen 3 und 50 Zeichen lang sein")]
    public string Name { get; set; } = string.Empty;
    
    public ICollection<StudentModel> Students { get; set; } = [];
    
    public bool IsDeleted { get; set; }
    
    // Navigation Properties
    public int? OwnerId { get; set; }
    [ForeignKey(nameof(OwnerId))]
    public InstructorModel? Owner { get; set; }

    
    public DrivingSchoolModel() { }
    public DrivingSchoolModel(DrivingSchoolModel other)
    {
        Id = other.Id;
        Name = other.Name;
        OwnerId = other.OwnerId;
        Owner = other.Owner;
        Students = other.Students
            .Select(x => new StudentModel(x))
            .ToList();
        IsDeleted = other.IsDeleted;
    }
}