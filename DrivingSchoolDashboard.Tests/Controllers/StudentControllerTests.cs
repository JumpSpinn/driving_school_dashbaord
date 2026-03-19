namespace DrivingSchoolDashboard.Tests.Controllers;

[TestFixture]
public sealed class StudentControllerTests
{
    private StudentService _service = null!;
    private StudentController _controller = null!;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var dbContext = new ApplicationDbContext(options);
        var serviceLogger = NullLogger<StudentService>.Instance;
        _service = new StudentService(dbContext, serviceLogger);

        var controllerLogger = NullLogger<StudentController>.Instance;
        _controller = new StudentController(_service, controllerLogger);
    }

    // ---------------------------------------------------------------------------
    // GetAllStudents
    // ---------------------------------------------------------------------------

    [Test]
    public void GetAllStudents_ReturnsOkWithEmptyList()
    {
        var result = _controller.GetAllStudents() as ObjectResult;

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.StatusCode, Is.EqualTo(200));
        Assert.That(result.Value, Is.InstanceOf<List<StudentModel>>());
        var students = result.Value as List<StudentModel>;
        Assert.That(students, Is.Empty);
    }

    // ---------------------------------------------------------------------------
    // CreateStudentAsync
    // ---------------------------------------------------------------------------

    [Test]
    public async Task CreateStudentAsync_ReturnsCreatedWhenSuccessful()
    {
        var student = new StudentModel { FirstName = "John", LastName = "Doe", Mail = "john@test.com" };

        var result = await _controller.CreateStudentAsync(student) as ObjectResult;

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.StatusCode, Is.EqualTo(201));
    }

    // ---------------------------------------------------------------------------
    // DeleteStudentAsync
    // ---------------------------------------------------------------------------

    [Test]
    public async Task DeleteStudentAsync_ReturnsNoContentWhenSuccessful()
    {
        var student = new StudentModel { FirstName = "John", LastName = "Doe" };
        var created = await _service.CreateStudentAsync(student);

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
    public async Task UpdateStudentAsync_ReturnsCreatedWhenSuccessful()
    {
        var student = new StudentModel { FirstName = "John", LastName = "Doe", Mail = "old@test.com" };
        var created = await _service.CreateStudentAsync(student);
        created!.Mail = "new@test.com";

        var result = await _controller.UpdateStudentAsync(created) as ObjectResult;

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.StatusCode, Is.EqualTo(201));
    }

    [Test]
    public async Task UpdateStudentAsync_ReturnsNotFoundWhenNotExists()
    {
        var student = new StudentModel { Id = 999, FirstName = "Unknown", LastName = "User" };

        var result = await _controller.UpdateStudentAsync(student) as NotFoundObjectResult;

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.StatusCode, Is.EqualTo(404));
    }
}
