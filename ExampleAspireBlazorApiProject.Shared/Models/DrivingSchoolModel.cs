namespace ExampleAspireBlazorApiProject.Shared.Models;

public sealed class DrivingSchoolModel
{
    [Key]
    public int Id { get; init; }
    
    [StringLength(100, MinimumLength = 3)]
    public string Name { get; set; } = string.Empty;
    public List<DriverModel> Drivers { get; set; } = [];
    public bool IsDeleted { get; set; }

    
    public DrivingSchoolModel() { }
    public DrivingSchoolModel(DrivingSchoolModel other)
    {
        Id = other.Id;
        Name = other.Name;
        Drivers = other.Drivers
            .Select(x => new DriverModel(x))
            .ToList();
        IsDeleted = other.IsDeleted;
    }
}