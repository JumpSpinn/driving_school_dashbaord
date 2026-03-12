namespace ExampleAspireBlazorApiProject.Tests.Controllers;

[TestFixture]
public sealed class DrivingSchoolControllerTests
{
    private DrivingSchoolService _service = null!;
    private DrivingSchoolController _controller = null!;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var dbContext = new ApplicationDbContext(options);
        var serviceLogger = NullLogger<DrivingSchoolService>.Instance;
        _service = new DrivingSchoolService(dbContext, serviceLogger);

        var controllerLogger = NullLogger<DrivingSchoolController>.Instance;
        _controller = new DrivingSchoolController(_service, controllerLogger);
    }

    // ---------------------------------------------------------------------------
    // GetAllDrivingSchools
    // ---------------------------------------------------------------------------

    [Test]
    public void GetAllDrivingSchools_ReturnsOkWithEmptyList()
    {
        var result = _controller.GetAllDrivingSchools() as ObjectResult;

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.StatusCode, Is.EqualTo(200));
        Assert.That(result.Value, Is.InstanceOf<List<DrivingSchoolModel>>());
        var schools = result.Value as List<DrivingSchoolModel>;
        Assert.That(schools, Is.Empty);
    }

    // ---------------------------------------------------------------------------
    // CreateDrivingSchool
    // ---------------------------------------------------------------------------

    [Test]
    public async Task CreateDrivingSchool_ReturnsCreatedWhenSuccessful()
    {
        var school = new DrivingSchoolModel { Name = "Test School", OwnerId = 1 };

        var result = await _controller.CreateDrivingSchool(school) as ObjectResult;

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.StatusCode, Is.EqualTo(201));
    }

    // ---------------------------------------------------------------------------
    // DeleteDrivingSchool
    // ---------------------------------------------------------------------------

    [Test]
    public async Task DeleteDrivingSchool_ReturnsNoContentWhenSuccessful()
    {
        var school = new DrivingSchoolModel { Name = "Test School" };
        var created = await _service.CreateDrivingSchoolAsync(school);

        var result = await _controller.DeleteDrivingSchool(created!.Id);

        Assert.That(result, Is.InstanceOf<NoContentResult>());
    }

    [Test]
    public async Task DeleteDrivingSchool_ReturnsNotFoundWhenNotExists()
    {
        var result = await _controller.DeleteDrivingSchool(999) as NotFoundObjectResult;

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.StatusCode, Is.EqualTo(404));
    }

    // ---------------------------------------------------------------------------
    // UpdateDrivingSchoolAsync
    // ---------------------------------------------------------------------------

    [Test]
    public async Task UpdateDrivingSchoolAsync_ReturnsCreatedWhenSuccessful()
    {
        var school = new DrivingSchoolModel { Name = "Old Name" };
        var created = await _service.CreateDrivingSchoolAsync(school);
        created!.Name = "New Name";

        var result = await _controller.UpdateDrivingSchoolAsync(created) as ObjectResult;

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.StatusCode, Is.EqualTo(201));
    }

    [Test]
    public async Task UpdateDrivingSchoolAsync_ReturnsNotFoundWhenNotExists()
    {
        var school = new DrivingSchoolModel { Id = 999, Name = "Unknown" };

        var result = await _controller.UpdateDrivingSchoolAsync(school) as NotFoundObjectResult;

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.StatusCode, Is.EqualTo(404));
    }
}
