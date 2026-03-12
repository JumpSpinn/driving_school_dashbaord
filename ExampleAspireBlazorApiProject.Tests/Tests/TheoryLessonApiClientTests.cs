namespace ExampleAspireBlazorApiProject.Tests;

[TestFixture]
public class TheoryLessonApiClientTests
{
    // ---------------------------------------------------------------------------
    // Helpers
    // ---------------------------------------------------------------------------

    private static TheoryLessonModel SampleLesson(int id = 1) => new()
    {
        Id = id,
        InstructorId = 5,
        MaxStudents = 20
    };

    /// <summary>
    /// Creates a fake HttpClient whose handler always returns the given response.
    /// </summary>
    private static HttpClient BuildClient(HttpResponseMessage response)
    {
        var handler = new FakeHttpMessageHandler(response);
        return new HttpClient(handler) { BaseAddress = new Uri("http://test/") };
    }

    private static TheoryLessonApiClient BuildApiClient(HttpResponseMessage response)
    {
        var httpClient = BuildClient(response);
        var logger = NullLogger<TheoryLessonApiClient>.Instance;
        return new TheoryLessonApiClient(httpClient, logger);
    }

    // ---------------------------------------------------------------------------
    // GetAllLessonsAsync
    // ---------------------------------------------------------------------------

    [Test]
    public async Task GetAllLessonsAsync_ReturnsListOnSuccess()
    {
        var lessons = new List<TheoryLessonModel> { SampleLesson(1), SampleLesson(2) };
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(lessons)
        };

        var client = BuildApiClient(response);
        var result = await client.GetAllLessonsAsync();

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Count, Is.EqualTo(2));
    }

    [Test]
    public async Task GetAllLessonsAsync_ReturnsNullWhenResponseIsNull()
    {
        // Endpoint returns JSON null → deserialized as null list
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("null", System.Text.Encoding.UTF8, "application/json")
        };

        var client = BuildApiClient(response);
        var result = await client.GetAllLessonsAsync();

        Assert.That(result, Is.Null);
    }

    // ---------------------------------------------------------------------------
    // CreateLessonAsync
    // ---------------------------------------------------------------------------

    [Test]
    public async Task CreateLessonAsync_ReturnsCreatedLessonOnSuccess()
    {
        var lesson = SampleLesson();
        var response = new HttpResponseMessage(HttpStatusCode.Created)
        {
            Content = JsonContent.Create(lesson)
        };

        var client = BuildApiClient(response);
        var result = await client.CreateLessonAsync(lesson);

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Id, Is.EqualTo(lesson.Id));
    }

    [Test]
    public async Task CreateLessonAsync_ReturnsNullWhenResponseBodyIsNull()
    {
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("null", System.Text.Encoding.UTF8, "application/json")
        };

        var client = BuildApiClient(response);
        var result = await client.CreateLessonAsync(SampleLesson());

        Assert.That(result, Is.Null);
    }

    // ---------------------------------------------------------------------------
    // DeleteLessonAsync
    // ---------------------------------------------------------------------------

    [Test]
    public async Task DeleteLessonAsync_ReturnsTrueOnSuccess()
    {
        var response = new HttpResponseMessage(HttpStatusCode.NoContent);

        var client = BuildApiClient(response);
        var result = await client.DeleteLessonAsync(1);

        Assert.That(result, Is.True);
    }

    [Test]
    public async Task DeleteLessonAsync_ReturnsFalseOnFailure()
    {
        var response = new HttpResponseMessage(HttpStatusCode.NotFound);

        var client = BuildApiClient(response);
        var result = await client.DeleteLessonAsync(99);

        Assert.That(result, Is.False);
    }

    // ---------------------------------------------------------------------------
    // UpdateLessonAsync
    // ---------------------------------------------------------------------------

    [Test]
    public async Task UpdateLessonAsync_ReturnsUpdatedLessonOnSuccess()
    {
        var lesson = SampleLesson();
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(lesson)
        };

        var client = BuildApiClient(response);
        var result = await client.UpdateLessonAsync(lesson);

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Id, Is.EqualTo(lesson.Id));
    }

    [Test]
    public async Task UpdateLessonAsync_ReturnsNullOnFailure()
    {
        var response = new HttpResponseMessage(HttpStatusCode.BadRequest)
        {
            Content = new StringContent("Invalid lesson data")
        };

        var client = BuildApiClient(response);
        var result = await client.UpdateLessonAsync(SampleLesson());

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
