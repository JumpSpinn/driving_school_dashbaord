namespace ExampleAspireBlazorApiProject.Web.Interfaces;

public interface IStudentApiClient
{
    public Task<List<StudentModel>?> GetAllStudentsAsync();
    public Task<StudentModel?> CreateStudentAsync(StudentModel newStudent);
    public Task<bool> DeleteStudentAsync(int id);
    public Task<StudentModel?> UpdateStudentAsync(StudentModel editStudent);
}