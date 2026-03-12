namespace ExampleAspireBlazorApiProject.Tests.Controllers;

[TestFixture]
public sealed class InstructorControllerTests
{
    private InstructorService _service = null!;
    private InstructorController _controller = null!;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var dbContext = new ApplicationDbContext(options);
        var serviceLogger = NullLogger<InstructorService>.Instance;
        _service = new InstructorService(dbContext, serviceLogger);

        var controllerLogger = NullLogger<InstructorController>.Instance;
        _controller = new InstructorController(_service, controllerLogger);
    }

    // ---------------------------------------------------------------------------
    // GetAllInstructors
    // ---------------------------------------------------------------------------

    [Test]
    public void GetAllInstructors_ReturnsOkWithEmptyList()
    {
        var result = _controller.GetAllInstructors() as ObjectResult;

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.StatusCode, Is.EqualTo(200));
        Assert.That(result.Value, Is.InstanceOf<List<InstructorModel>>());
        var instructors = result.Value as List<InstructorModel>;
        Assert.That(instructors, Is.Empty);
    }

    // ---------------------------------------------------------------------------
    // CreateInstructorAsync
    // ---------------------------------------------------------------------------

    [Test]
    public async Task CreateInstructorAsync_ReturnsCreatedWhenSuccessful()
    {
        var instructor = new InstructorModel { FirstName = "Jane", LastName = "Smith", Mail = "jane@test.com" };

        var result = await _controller.CreateInstructorAsync(instructor) as ObjectResult;

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.StatusCode, Is.EqualTo(201));
    }

    // ---------------------------------------------------------------------------
    // DeleteStudentAsync
    // ---------------------------------------------------------------------------

    [Test]
    public async Task DeleteStudentAsync_ReturnsNoContentWhenSuccessful()
    {
        var instructor = new InstructorModel { FirstName = "Jane", LastName = "Smith" };
        var created = await _service.CreateInstructorAsync(instructor);

        var result = await _controller.DeleteStudentAsync(created!.Id);

        Assert.That(result, Is.InstanceOf<NoContentResult>());
    }

    [Test]
    public async Task DeleteStudentAsync_ReturnsNotFoundWhenNotExists()
    {
        var result = await _controller.DeleteStudentAsync(999) as NotFoundObjectResult;

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.StatusCode, Is.EqualTo(404));
    }

    // ---------------------------------------------------------------------------
    // UpdateStudentAsync
    // ---------------------------------------------------------------------------

    [Test]
    public async Task UpdateStudentAsync_ReturnsNoContentWhenSuccessful()
    {
        var instructor = new InstructorModel { FirstName = "Jane", LastName = "Smith", Mail = "old@test.com" };
        var created = await _service.CreateInstructorAsync(instructor);
        created!.Mail = "new@test.com";

        var result = await _controller.UpdateStudentAsync(created);

        Assert.That(result, Is.InstanceOf<NoContentResult>());
    }

    [Test]
    public async Task UpdateStudentAsync_ReturnsNotFoundWhenNotExists()
    {
        var instructor = new InstructorModel { Id = 999, FirstName = "Unknown", LastName = "User" };

        var result = await _controller.UpdateStudentAsync(instructor) as NotFoundObjectResult;

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.StatusCode, Is.EqualTo(404));
    }
}
