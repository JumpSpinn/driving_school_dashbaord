namespace ExampleAspireBlazorApiProject.Web.Apis;

public class InstructorApiClient(HttpClient client, ILogger<InstructorApiClient> logger) :IInstructorApiClient
{
    public async Task<List<InstructorModel>?> GetAllInstructorsAsync()
    {
        var result = await client.GetFromJsonAsync<List<InstructorModel>>("api/Instructor");
        if (result is null)
        {
            logger.LogError("no instructors registered!");
            return null;
        }

        logger.LogInformation($"{result.Count} registered instructors loaded!");
        return result;
    }

    public async Task<InstructorModel?> CreateInstructorAsync(InstructorModel newInstructor)
    {
        var result = await client.PostAsJsonAsync("api/Instructor", newInstructor);
        
        var instructor = await result.Content.ReadFromJsonAsync<InstructorModel>();
        if (instructor is not null) return instructor;
        
        logger.LogError("error creating instructor!");
        return null;
    }

    public async Task<bool> DeleteInstructorAsync(int id)
    {
        var result = await client.DeleteAsync($"api/Instructor/{id}");
        logger.LogInformation($"Web API => instructor with id {id} deleted! - Success: {result.IsSuccessStatusCode}");
        return result.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateInstructorAsync(InstructorModel editInstructor)
    {
        var result = await client.PutAsJsonAsync($"api/Instructor/", editInstructor);
        logger.LogInformation($"Web API => instructor with id {editInstructor.Id} updated! - Success: {result.IsSuccessStatusCode}");
        return result.IsSuccessStatusCode;
    }
}