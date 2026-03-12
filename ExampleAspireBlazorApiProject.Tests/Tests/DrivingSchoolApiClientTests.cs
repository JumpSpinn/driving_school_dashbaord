namespace ExampleAspireBlazorApiProject.Tests;

[TestFixture]
public class DrivingSchoolApiClientTests
{
    // ---------------------------------------------------------------------------
    // Helpers
    // ---------------------------------------------------------------------------

    private static DrivingSchoolModel SampleDrivingSchool(int id = 1) => new()
    {
        Id = id,
        Name = "Test Driving School",
    };

    /// <summary>
    /// Creates a fake HttpClient whose handler always returns the given response.
    /// </summary>
    private static HttpClient BuildClient(HttpResponseMessage response)
    {
        var handler = new FakeHttpMessageHandler(response);
        return new HttpClient(handler) { BaseAddress = new Uri("http://test/") };
    }

    private static DrivingSchoolApiClient BuildApiClient(HttpResponseMessage response)
    {
        var httpClient = BuildClient(response);
        var logger = NullLogger<DrivingSchoolApiClient>.Instance;
        return new DrivingSchoolApiClient(httpClient, logger);
    }

    // ---------------------------------------------------------------------------
    // GetAllDrivingSchoolsAsync
    // ---------------------------------------------------------------------------

    [Test]
    public async Task GetAllDrivingSchoolsAsync_ReturnsListOnSuccess()
    {
        var schools = new List<DrivingSchoolModel> { SampleDrivingSchool(1), SampleDrivingSchool(2) };
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(schools)
        };

        var client = BuildApiClient(response);
        var result = await client.GetAllDrivingSchoolsAsync();

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Count, Is.EqualTo(2));
    }

    [Test]
    public async Task GetAllDrivingSchoolsAsync_ReturnsNullWhenResponseIsNull()
    {
        // Endpoint returns JSON null → deserialized as null list
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("null", System.Text.Encoding.UTF8, "application/json")
        };

        var client = BuildApiClient(response);
        var result = await client.GetAllDrivingSchoolsAsync();

        Assert.That(result, Is.Null);
    }

    // ---------------------------------------------------------------------------
    // CreateDrivingSchoolAsync
    // ---------------------------------------------------------------------------

    [Test]
    public async Task CreateDrivingSchoolAsync_ReturnsCreatedSchoolOnSuccess()
    {
        var school = SampleDrivingSchool();
        var response = new HttpResponseMessage(HttpStatusCode.Created)
        {
            Content = JsonContent.Create(school)
        };

        var client = BuildApiClient(response);
        var result = await client.CreateDrivingSchoolAsync(school);

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Id, Is.EqualTo(school.Id));
    }

    [Test]
    public async Task CreateDrivingSchoolAsync_ReturnsNullWhenResponseBodyIsNull()
    {
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("null", System.Text.Encoding.UTF8, "application/json")
        };

        var client = BuildApiClient(response);
        var result = await client.CreateDrivingSchoolAsync(SampleDrivingSchool());

        Assert.That(result, Is.Null);
    }

    // ---------------------------------------------------------------------------
    // DeleteDrivingSchoolAsync
    // ---------------------------------------------------------------------------

    [Test]
    public async Task DeleteDrivingSchoolAsync_ReturnsTrueOnSuccess()
    {
        var response = new HttpResponseMessage(HttpStatusCode.NoContent);

        var client = BuildApiClient(response);
        var result = await client.DeleteDrivingSchoolAsync(1);

        Assert.That(result, Is.True);
    }

    [Test]
    public async Task DeleteDrivingSchoolAsync_ReturnsFalseOnFailure()
    {
        var response = new HttpResponseMessage(HttpStatusCode.NotFound);

        var client = BuildApiClient(response);
        var result = await client.DeleteDrivingSchoolAsync(99);

        Assert.That(result, Is.False);
    }

    // ---------------------------------------------------------------------------
    // UpdateDrivingSchoolAsync
    // ---------------------------------------------------------------------------

    [Test]
    public async Task UpdateDrivingSchoolAsync_ReturnsUpdatedSchoolOnSuccess()
    {
        var school = SampleDrivingSchool();
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(school)
        };

        var client = BuildApiClient(response);
        var result = await client.UpdateDrivingSchoolAsync(school);

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Id, Is.EqualTo(school.Id));
    }

    [Test]
    public async Task UpdateDrivingSchoolAsync_ReturnsNullOnFailure()
    {
        var response = new HttpResponseMessage(HttpStatusCode.BadRequest)
        {
            Content = new StringContent("Invalid school data")
        };

        var client = BuildApiClient(response);
        var result = await client.UpdateDrivingSchoolAsync(SampleDrivingSchool());

        Assert.That(result, Is.Null);
    }

    // ---------------------------------------------------------------------------
    // Fake handler
    // ---------------------------------------------------------------------------

    private sealed class FakeHttpMessageHandler(HttpResponseMessage response) : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken) =>
            Task.FromResult(response);
    }
}
