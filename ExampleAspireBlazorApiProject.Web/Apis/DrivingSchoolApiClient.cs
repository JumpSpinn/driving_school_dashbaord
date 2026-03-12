namespace ExampleAspireBlazorApiProject.Web.Apis;

public sealed class DrivingSchoolApiClient(HttpClient client, ILogger<DrivingSchoolApiClient> logger) : IDrivingSchoolApiClient
{
    public async Task<List<DrivingSchoolModel>?> GetAllDrivingSchoolsAsync()
    {
        var result = await client.GetFromJsonAsync<List<DrivingSchoolModel>>("api/DrivingSchool");
        if (result is null)
        {
            logger.LogError("no driving schools registered!");
            return null;
        }

        logger.LogInformation($"{result.Count} registered driving schools loaded!");
        return result;
    }

    public async Task<DrivingSchoolModel?> CreateDrivingSchoolAsync(DrivingSchoolModel newDrivingSchool)
    {
        var result = await client.PostAsJsonAsync("api/DrivingSchool", newDrivingSchool);
        
        var drivingSchool = await result.Content.ReadFromJsonAsync<DrivingSchoolModel>();
        if (drivingSchool is not null) return drivingSchool;
        
        logger.LogError("error creating driving school!");
        return null;
    }

    public async Task<bool> DeleteDrivingSchoolAsync(int id)
    {
        var result = await client.DeleteAsync($"api/DrivingSchool/{id}");
        logger.LogInformation($"Web API => driving school with id {id} deleted! - Success: {result.IsSuccessStatusCode}");
        return result.IsSuccessStatusCode;
    }

    public async Task<DrivingSchoolModel?> UpdateDrivingSchoolAsync(DrivingSchoolModel editDrivingSchool)
    {
        var result = await client.PutAsJsonAsync($"api/DrivingSchool/", editDrivingSchool);
        logger.LogInformation($"Web API => driving school with id {editDrivingSchool.Id} updated! - Success: {result.IsSuccessStatusCode}");

        if (result.IsSuccessStatusCode)
        {
            var updatedModel = await result.Content.ReadFromJsonAsync<DrivingSchoolModel>();
            return updatedModel;
        }
        
        var errorDetails = await result.Content.ReadAsStringAsync();
        logger.LogWarning($"Update fehlgeschlagen: {errorDetails}");
        return null; 
    }
}