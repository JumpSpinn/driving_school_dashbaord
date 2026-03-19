namespace DrivingSchoolDashboard.Web.Interfaces;

public interface IInstructorApiClient
{
    public Task<List<InstructorModel>?> GetAllInstructorsAsync();
    public Task<InstructorModel?> CreateInstructorAsync(InstructorModel newInstructor);
    public Task<bool> DeleteInstructorAsync(int id);
    public Task<bool> UpdateInstructorAsync(InstructorModel editInstructor);
}