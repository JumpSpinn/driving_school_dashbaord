namespace ExampleAspireBlazorApiProject.Web.Interfaces;

public interface IDrivingSchoolApiClient
{
    public Task<List<DrivingSchoolModel>?> GetAllDrivingSchoolsAsync();
    public Task<DrivingSchoolModel?> CreateDrivingSchoolAsync(DrivingSchoolModel newDrivingSchool);
    public Task<bool> DeleteDrivingSchoolAsync(int id);
    public Task<DrivingSchoolModel?> UpdateDrivingSchoolAsync(DrivingSchoolModel editDrivingSchool);
}