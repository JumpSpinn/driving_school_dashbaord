namespace ExampleAspireBlazorApiProject.Tests.Controllers;

[TestFixture]
public sealed class TheoryLessonControllerTests
{
    private TheoryLessonService _service = null!;
    private TheoryLessonController _controller = null!;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var dbContext = new ApplicationDbContext(options);
        var serviceLogger = NullLogger<TheoryLessonService>.Instance;
        _service = new TheoryLessonService(dbContext, serviceLogger);

        var controllerLogger = NullLogger<TheoryLessonController>.Instance;
        _controller = new TheoryLessonController(_service, controllerLogger);
    }

    // ---------------------------------------------------------------------------
    // GetAllLessons
    // ---------------------------------------------------------------------------

    [Test]
    public void GetAllLessons_ReturnsOkWithEmptyList()
    {
        var result = _controller.GetAllLessons() as ObjectResult;

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.StatusCode, Is.EqualTo(200));
        Assert.That(result.Value, Is.InstanceOf<List<TheoryLessonModel>>());
        var lessons = result.Value as List<TheoryLessonModel>;
        Assert.That(lessons, Is.Empty);
    }

    // ---------------------------------------------------------------------------
    // CreateLessonAsync
    // ---------------------------------------------------------------------------

    [Test]
    public async Task CreateLessonAsync_ReturnsCreatedWhenSuccessful()
    {
        var lesson = new TheoryLessonModel { Name = "Traffic Rules", Topic = "Basic rules", DurationMinutes = 90 };

        var result = await _controller.CreateLessonAsync(lesson) as ObjectResult;

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.StatusCode, Is.EqualTo(201));
    }

    // ---------------------------------------------------------------------------
    // DeleteLessonAsync
    // ---------------------------------------------------------------------------

    [Test]
    public async Task DeleteLessonAsync_ReturnsNoContentWhenSuccessful()
    {
        var lesson = new TheoryLessonModel { Name = "Traffic Rules" };
        var created = await _service.CreateTheoryLessonAsync(lesson);

        var result = await _controller.DeleteLessonAsync(created!.Id);

        Assert.That(result, Is.InstanceOf<NoContentResult>());
    }

    [Test]
    public async Task DeleteLessonAsync_ReturnsNotFoundWhenNotExists()
    {
        var result = await _controller.DeleteLessonAsync(999) as NotFoundObjectResult;

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.StatusCode, Is.EqualTo(404));
    }

    // ---------------------------------------------------------------------------
    // UpdateLessonAsync
    // ---------------------------------------------------------------------------

    [Test]
    public async Task UpdateLessonAsync_ReturnsCreatedWhenSuccessful()
    {
        var lesson = new TheoryLessonModel { Name = "Old Name", Topic = "Old Topic" };
        var created = await _service.CreateTheoryLessonAsync(lesson);
        created!.Name = "New Name";

        var result = await _controller.UpdateLessonAsync(created) as ObjectResult;

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.StatusCode, Is.EqualTo(201));
    }

    [Test]
    public async Task UpdateLessonAsync_ReturnsNotFoundWhenNotExists()
    {
        var lesson = new TheoryLessonModel { Id = 999, Name = "Unknown" };

        var result = await _controller.UpdateLessonAsync(lesson) as NotFoundObjectResult;

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.StatusCode, Is.EqualTo(404));
    }
}
