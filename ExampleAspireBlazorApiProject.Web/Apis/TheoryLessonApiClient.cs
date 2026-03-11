namespace ExampleAspireBlazorApiProject.Web.Apis;

public class TheoryLessonApiClient(HttpClient client, ILogger<TheoryLessonApiClient> logger)
{
    public async Task<List<TheoryLessonModel>?> GetAllLessonsAsync()
    {
        var result = await client.GetFromJsonAsync<List<TheoryLessonModel>>("api/TheoryLesson");
        if (result is null)
        {
            logger.LogError("no lessons registered!");
            return null;
        }

        logger.LogInformation($"{result.Count} registered lessons loaded!");
        return result;
    }

    public async Task<TheoryLessonModel?> CreateLessonAsync(TheoryLessonModel newLesson)
    {
        var result = await client.PostAsJsonAsync("api/TheoryLesson", newLesson);
        
        var lesson = await result.Content.ReadFromJsonAsync<TheoryLessonModel>();
        if (lesson is not null) return lesson;
        
        logger.LogError("error creating lesson!");
        return null;
    }

    public async Task<bool> DeleteLessonAsync(int id)
    {
        var result = await client.DeleteAsync($"api/TheoryLesson/{id}");
        logger.LogInformation($"Web API => lesson with id {id} deleted! - Success: {result.IsSuccessStatusCode}");
        return result.IsSuccessStatusCode;
    }
    
    public async Task<TheoryLessonModel?> UpdateLessonAsync(TheoryLessonModel editLesson)
    {
        var result = await client.PutAsJsonAsync($"api/TheoryLesson/", editLesson);
        logger.LogInformation($"Web API => lesson with id {editLesson.Id} updated! - Success: {result.IsSuccessStatusCode}");

        if (result.IsSuccessStatusCode)
        {
            var updatedModel = await result.Content.ReadFromJsonAsync<TheoryLessonModel>();
            return updatedModel;
        }
        
        var errorDetails = await result.Content.ReadAsStringAsync();
        logger.LogWarning($"Update fehlgeschlagen: {errorDetails}");
        return null; 
    }
}