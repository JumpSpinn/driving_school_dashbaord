using ExampleAspireBlazorApiProject.Shared.Models;

namespace ExampleAspireBlazorApiProject.Web.Apis;

public sealed class DriverApiClient(HttpClient client, ILogger<DriverApiClient> logger)
{
    public async Task<List<DriverModel>?> GetAllDriversAsync()
    {
        var result = await client.GetFromJsonAsync<List<DriverModel>>("api/Driver");
        if (result is null)
        {
            logger.LogError("no drivers registered!");
            return null;
        }

        logger.LogInformation($"{result.Count} registered drivers loaded!");
        return result;
    }

    public async Task<DriverModel?> CreateDriverAsync(DriverModel newDriver)
    {
        var result = await client.PostAsJsonAsync("api/Driver", newDriver);
        
        var driver = await result.Content.ReadFromJsonAsync<DriverModel>();
        if (driver is not null) return driver;
        
        logger.LogError("error creating driver!");
        return null;
    }

    public async Task<bool> DeleteDriverAsync(int id)
    {
        var result = await client.DeleteAsync($"api/Driver/{id}");
        logger.LogInformation($"Web API => driver with id {id} deleted! - Success: {result.IsSuccessStatusCode}");
        return result.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateDriverAsync(DriverModel editDriver)
    {
        var result = await client.PutAsJsonAsync($"api/Driver/", editDriver);
        logger.LogInformation($"Web API => driver with id {editDriver.Id} updated! - Success: {result.IsSuccessStatusCode}");
        return result.IsSuccessStatusCode;
    }
}