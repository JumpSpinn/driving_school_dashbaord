namespace ExampleAspireBlazorApiProject.Web.Apis;

public sealed class StudentApiClient(HttpClient client, ILogger<StudentApiClient> logger)
{
    public async Task<List<StudentModel>?> GetAllStudentsAsync()
    {
        var result = await client.GetFromJsonAsync<List<StudentModel>>("api/Student");
        if (result is null)
        {
            logger.LogError("no students registered!");
            return null;
        }

        logger.LogInformation($"{result.Count} registered students loaded!");
        return result;
    }

    public async Task<StudentModel?> CreateStudentAsync(StudentModel newStudent)
    {
        var result = await client.PostAsJsonAsync("api/Student", newStudent);
        
        var driver = await result.Content.ReadFromJsonAsync<StudentModel>();
        if (driver is not null) return driver;
        
        logger.LogError("error creating student!");
        return null;
    }

    public async Task<bool> DeleteStudentAsync(int id)
    {
        var result = await client.DeleteAsync($"api/Student/{id}");
        logger.LogInformation($"Web API => student with id {id} deleted! - Success: {result.IsSuccessStatusCode}");
        return result.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateStudentAsync(StudentModel editStudent)
    {
        var result = await client.PutAsJsonAsync($"api/Student/", editStudent);
        logger.LogInformation($"Web API => student with id {editStudent.Id} updated! - Success: {result.IsSuccessStatusCode}");
        return result.IsSuccessStatusCode;
    }
}