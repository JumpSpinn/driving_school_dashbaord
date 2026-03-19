namespace DrivingSchoolDashboard.Tests.Apis;

[TestFixture]
public sealed class StudentApiClientTests
{
    // ---------------------------------------------------------------------------
    // Helpers
    // ---------------------------------------------------------------------------

    private static StudentModel SampleStudent(int id = 1) => new()
    {
        Id = id,
        FirstName = "John",
        LastName = "Doe"
    };

    /// <summary>
    /// Creates a fake HttpClient whose handler always returns the given response.
    /// </summary>
    private static HttpClient BuildClient(HttpResponseMessage response)
    {
        var handler = new FakeHttpMessageHandler(response);
        return new HttpClient(handler) { BaseAddress = new Uri("http://test/") };
    }

    private static StudentApiClient BuildApiClient(HttpResponseMessage response)
    {
        var httpClient = BuildClient(response);
        var logger = NullLogger<StudentApiClient>.Instance;
        return new StudentApiClient(httpClient, logger);
    }

    // ---------------------------------------------------------------------------
    // GetAllStudentsAsync
    // ---------------------------------------------------------------------------

    [Test]
    public async Task GetAllStudentsAsync_ReturnsListOnSuccess()
    {
        var students = new List<StudentModel> { SampleStudent(1), SampleStudent(2) };
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(students)
        };

        var client = BuildApiClient(response);
        var result = await client.GetAllStudentsAsync();

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Count, Is.EqualTo(2));
    }

    [Test]
    public async Task GetAllStudentsAsync_ReturnsNullWhenResponseIsNull()
    {
        // Endpoint returns JSON null → deserialized as null list
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("null", System.Text.Encoding.UTF8, "application/json")
        };

        var client = BuildApiClient(response);
        var result = await client.GetAllStudentsAsync();

        Assert.That(result, Is.Null);
    }

    // ---------------------------------------------------------------------------
    // CreateStudentAsync
    // ---------------------------------------------------------------------------

    [Test]
    public async Task CreateStudentAsync_ReturnsCreatedStudentOnSuccess()
    {
        var student = SampleStudent();
        var response = new HttpResponseMessage(HttpStatusCode.Created)
        {
            Content = JsonContent.Create(student)
        };

        var client = BuildApiClient(response);
        var result = await client.CreateStudentAsync(student);

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Id, Is.EqualTo(student.Id));
    }

    [Test]
    public async Task CreateStudentAsync_ReturnsNullWhenResponseBodyIsNull()
    {
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("null", System.Text.Encoding.UTF8, "application/json")
        };

        var client = BuildApiClient(response);
        var result = await client.CreateStudentAsync(SampleStudent());

        Assert.That(result, Is.Null);
    }

    // ---------------------------------------------------------------------------
    // DeleteStudentAsync
    // ---------------------------------------------------------------------------

    [Test]
    public async Task DeleteStudentAsync_ReturnsTrueOnSuccess()
    {
        var response = new HttpResponseMessage(HttpStatusCode.NoContent);

        var client = BuildApiClient(response);
        var result = await client.DeleteStudentAsync(1);

        Assert.That(result, Is.True);
    }

    [Test]
    public async Task DeleteStudentAsync_ReturnsFalseOnFailure()
    {
        var response = new HttpResponseMessage(HttpStatusCode.NotFound);

        var client = BuildApiClient(response);
        var result = await client.DeleteStudentAsync(99);

        Assert.That(result, Is.False);
    }

    // ---------------------------------------------------------------------------
    // UpdateStudentAsync
    // ---------------------------------------------------------------------------

    [Test]
    public async Task UpdateStudentAsync_ReturnsUpdatedStudentOnSuccess()
    {
        var student = SampleStudent();
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(student)
        };

        var client = BuildApiClient(response);
        var result = await client.UpdateStudentAsync(student);

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Id, Is.EqualTo(student.Id));
    }

    [Test]
    public async Task UpdateStudentAsync_ReturnsNullOnFailure()
    {
        var response = new HttpResponseMessage(HttpStatusCode.BadRequest)
        {
            Content = new StringContent("Invalid student data")
        };

        var client = BuildApiClient(response);
        var result = await client.UpdateStudentAsync(SampleStudent());

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
