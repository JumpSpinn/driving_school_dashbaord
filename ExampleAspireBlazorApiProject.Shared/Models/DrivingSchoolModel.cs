namespace ExampleAspireBlazorApiProject.Shared.Models;

public sealed class DrivingSchoolModel
{
    [Key]
    public int Id { get; init; }
    
    [StringLength(100, MinimumLength = 3)]
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