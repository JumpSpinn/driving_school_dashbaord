namespace ExampleAspireBlazorApiProject.Shared.Models;

public sealed class DrivingSchoolModel
{
    [Key]
    public int Id { get; init; }
    
    [Required(ErrorMessage = "Bezeichnung ist Pflicht")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Bezeichnung muss zwischen 3 und 50 Zeichen lang sein")]
    public string Name { get; set; } = string.Empty;
    
    public List<StudentModel> Students { get; set; } = [];
    public bool IsDeleted { get; set; }

    
    public DrivingSchoolModel() { }
    public DrivingSchoolModel(DrivingSchoolModel other)
    {
        Id = other.Id;
        Name = other.Name;
        Students = other.Students
            .Select(x => new StudentModel(x))
            .ToList();
        IsDeleted = other.IsDeleted;
    }
}