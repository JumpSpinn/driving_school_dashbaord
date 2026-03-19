namespace DrivingSchoolDashboard.Tests.Apis;

[TestFixture]
public sealed class CourseBookingApiClientTests
{
    // ---------------------------------------------------------------------------
    // Helpers
    // ---------------------------------------------------------------------------

    private static CourseBookingModel SampleBooking(int id = 1) => new()
    {
        Id = id,
        StudentId = 10,
        TheoryLessonId = 20,
        BookingDate = new DateTime(2025, 1, 1)
    };

    /// <summary>
    /// Creates a fake HttpClient whose handler always returns the given response.
    /// </summary>
    private static HttpClient BuildClient(HttpResponseMessage response)
    {
        var handler = new FakeHttpMessageHandler(response);
        return new HttpClient(handler) { BaseAddress = new Uri("http://test/") };
    }

    private static CourseBookingApiClient BuildApiClient(HttpResponseMessage response)
    {
        var httpClient = BuildClient(response);
        var logger = NullLogger<CourseBookingApiClient>.Instance;
        return new CourseBookingApiClient(httpClient, logger);
    }

    // ---------------------------------------------------------------------------
    // GetAllCourseBookingsAsync
    // ---------------------------------------------------------------------------

    [Test]
    public async Task GetAllCourseBookingsAsync_ReturnsListOnSuccess()
    {
        var bookings = new List<CourseBookingModel> { SampleBooking(1), SampleBooking(2) };
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(bookings)
        };

        var client = BuildApiClient(response);
        var result = await client.GetAllCourseBookingsAsync();

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Count, Is.EqualTo(2));
    }

    [Test]
    public async Task GetAllCourseBookingsAsync_ReturnsNullWhenResponseIsNull()
    {
        // Endpoint returns JSON null → deserialized as null list
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("null", System.Text.Encoding.UTF8, "application/json")
        };

        var client = BuildApiClient(response);
        var result = await client.GetAllCourseBookingsAsync();

        Assert.That(result, Is.Null);
    }

    // ---------------------------------------------------------------------------
    // CreateCourseBookingAsync
    // ---------------------------------------------------------------------------

    [Test]
    public async Task CreateCourseBookingAsync_ReturnsCreatedBookingOnSuccess()
    {
        var booking = SampleBooking();
        var response = new HttpResponseMessage(HttpStatusCode.Created)
        {
            Content = JsonContent.Create(booking)
        };

        var client = BuildApiClient(response);
        var result = await client.CreateCourseBookingAsync(booking);

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Id, Is.EqualTo(booking.Id));
    }

    [Test]
    public async Task CreateCourseBookingAsync_ReturnsNullWhenResponseBodyIsNull()
    {
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("null", System.Text.Encoding.UTF8, "application/json")
        };

        var client = BuildApiClient(response);
        var result = await client.CreateCourseBookingAsync(SampleBooking());

        Assert.That(result, Is.Null);
    }

    // ---------------------------------------------------------------------------
    // DeleteCourseBookingAsync
    // ---------------------------------------------------------------------------

    [Test]
    public async Task DeleteCourseBookingAsync_ReturnsTrueOnSuccess()
    {
        var response = new HttpResponseMessage(HttpStatusCode.NoContent);

        var client = BuildApiClient(response);
        var result = await client.DeleteCourseBookingAsync(1);

        Assert.That(result, Is.True);
    }

    [Test]
    public async Task DeleteCourseBookingAsync_ReturnsFalseOnFailure()
    {
        var response = new HttpResponseMessage(HttpStatusCode.NotFound);

        var client = BuildApiClient(response);
        var result = await client.DeleteCourseBookingAsync(99);

        Assert.That(result, Is.False);
    }

    // ---------------------------------------------------------------------------
    // UpdateCourseBookingAsync
    // ---------------------------------------------------------------------------

    [Test]
    public async Task UpdateCourseBookingAsync_ReturnsUpdatedBookingOnSuccess()
    {
        var booking = SampleBooking();
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(booking)
        };

        var client = BuildApiClient(response);
        var result = await client.UpdateCourseBookingAsync(booking);

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Id, Is.EqualTo(booking.Id));
    }

    [Test]
    public async Task UpdateCourseBookingAsync_ReturnsNullOnFailure()
    {
        var response = new HttpResponseMessage(HttpStatusCode.BadRequest)
        {
            Content = new StringContent("Invalid booking data")
        };

        var client = BuildApiClient(response);
        var result = await client.UpdateCourseBookingAsync(SampleBooking());

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