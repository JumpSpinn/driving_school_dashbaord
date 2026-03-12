namespace ExampleAspireBlazorApiProject.Tests;

[TestFixture]
public class InstructorApiClientTests
{
    // ---------------------------------------------------------------------------
    // Helpers
    // ---------------------------------------------------------------------------

    private static InstructorModel SampleInstructor(int id = 1) => new()
    {
        Id = id,
        FirstName = "Jane",
        LastName = "Smith"
    };

    /// <summary>
    /// Creates a fake HttpClient whose handler always returns the given response.
    /// </summary>
    private static HttpClient BuildClient(HttpResponseMessage response)
    {
        var handler = new FakeHttpMessageHandler(response);
        return new HttpClient(handler) { BaseAddress = new Uri("http://test/") };
    }

    private static InstructorApiClient BuildApiClient(HttpResponseMessage response)
    {
        var httpClient = BuildClient(response);
        var logger = NullLogger<InstructorApiClient>.Instance;
        return new InstructorApiClient(httpClient, logger);
    }

    // ---------------------------------------------------------------------------
    // GetAllInstructorsAsync
    // ---------------------------------------------------------------------------

    [Test]
    public async Task GetAllInstructorsAsync_ReturnsListOnSuccess()
    {
        var instructors = new List<InstructorModel> { SampleInstructor(1), SampleInstructor(2) };
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(instructors)
        };

        var client = BuildApiClient(response);
        var result = await client.GetAllInstructorsAsync();

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Count, Is.EqualTo(2));
    }

    [Test]
    public async Task GetAllInstructorsAsync_ReturnsNullWhenResponseIsNull()
    {
        // Endpoint returns JSON null → deserialized as null list
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("null", System.Text.Encoding.UTF8, "application/json")
        };

        var client = BuildApiClient(response);
        var result = await client.GetAllInstructorsAsync();

        Assert.That(result, Is.Null);
    }

    // ---------------------------------------------------------------------------
    // CreateInstructorAsync
    // ---------------------------------------------------------------------------

    [Test]
    public async Task CreateInstructorAsync_ReturnsCreatedInstructorOnSuccess()
    {
        var instructor = SampleInstructor();
        var response = new HttpResponseMessage(HttpStatusCode.Created)
        {
            Content = JsonContent.Create(instructor)
        };

        var client = BuildApiClient(response);
        var result = await client.CreateInstructorAsync(instructor);

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Id, Is.EqualTo(instructor.Id));
    }

    [Test]
    public async Task CreateInstructorAsync_ReturnsNullWhenResponseBodyIsNull()
    {
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("null", System.Text.Encoding.UTF8, "application/json")
        };

        var client = BuildApiClient(response);
        var result = await client.CreateInstructorAsync(SampleInstructor());

        Assert.That(result, Is.Null);
    }

    // ---------------------------------------------------------------------------
    // DeleteInstructorAsync
    // ---------------------------------------------------------------------------

    [Test]
    public async Task DeleteInstructorAsync_ReturnsTrueOnSuccess()
    {
        var response = new HttpResponseMessage(HttpStatusCode.NoContent);

        var client = BuildApiClient(response);
        var result = await client.DeleteInstructorAsync(1);

        Assert.That(result, Is.True);
    }

    [Test]
    public async Task DeleteInstructorAsync_ReturnsFalseOnFailure()
    {
        var response = new HttpResponseMessage(HttpStatusCode.NotFound);

        var client = BuildApiClient(response);
        var result = await client.DeleteInstructorAsync(99);

        Assert.That(result, Is.False);
    }

    // ---------------------------------------------------------------------------
    // UpdateInstructorAsync
    // ---------------------------------------------------------------------------

    [Test]
    public async Task UpdateInstructorAsync_ReturnsTrueOnSuccess()
    {
        var instructor = SampleInstructor();
        var response = new HttpResponseMessage(HttpStatusCode.OK);

        var client = BuildApiClient(response);
        var result = await client.UpdateInstructorAsync(instructor);

        Assert.That(result, Is.True);
    }

    [Test]
    public async Task UpdateInstructorAsync_ReturnsFalseOnFailure()
    {
        var response = new HttpResponseMessage(HttpStatusCode.BadRequest)
        {
            Content = new StringContent("Invalid instructor data")
        };

        var client = BuildApiClient(response);
        var result = await client.UpdateInstructorAsync(SampleInstructor());

        Assert.That(result, Is.False);
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
