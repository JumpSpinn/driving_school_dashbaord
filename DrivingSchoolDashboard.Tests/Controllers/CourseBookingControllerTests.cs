namespace DrivingSchoolDashboard.Tests.Controllers;

[TestFixture]
public sealed class CourseBookingControllerTests
{
    private ApplicationDbContext _dbContext = null!;
    private CourseBookingService _service = null!;
    private CourseBookingController _controller = null!;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new ApplicationDbContext(options);
        var serviceLogger = NullLogger<CourseBookingService>.Instance;
        _service = new CourseBookingService(_dbContext, serviceLogger);

        var controllerLogger = NullLogger<CourseBookingController>.Instance;
        _controller = new CourseBookingController(_service, controllerLogger);
    }

    [TearDown]
    public void TearDown()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }

    // ---------------------------------------------------------------------------
    // GetAllCourseBookings
    // ---------------------------------------------------------------------------

    [Test]
    public void GetAllCourseBookings_ReturnsOkWithEmptyList()
    {
        var result = _controller.GetAllCourseBookings() as ObjectResult;

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.StatusCode, Is.EqualTo(200));
        Assert.That(result.Value, Is.InstanceOf<List<CourseBookingModel>>());
        var bookings = result.Value as List<CourseBookingModel>;
        Assert.That(bookings, Is.Empty);
    }

    // ---------------------------------------------------------------------------
    // CreateCourseBookingAsync
    // ---------------------------------------------------------------------------

    [Test]
    public async Task CreateCourseBookingAsync_ReturnsCreatedWhenSuccessful()
    {
        // Create required entities first
        var student = new StudentModel { Id = 1, FirstName = "John", LastName = "Doe", Mail = "john@test.com" };
        var lesson = new TheoryLessonModel { Id = 1, Name = "Traffic", Topic = "Rules" };
        _dbContext.Students.Add(student);
        _dbContext.TheoryLessons.Add(lesson);
        await _dbContext.SaveChangesAsync();

        var booking = new CourseBookingModel { StudentId = 1, TheoryLessonId = 1, BookingDate = DateTime.Now };

        var result = await _controller.CreateCourseBookingAsync(booking) as ObjectResult;

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.StatusCode, Is.EqualTo(201));
    }

    [Test]
    public async Task CreateCourseBookingAsync_ReturnsBadRequestWhenFails()
    {
        var booking = new CourseBookingModel { StudentId = null, TheoryLessonId = null };

        var result = await _controller.CreateCourseBookingAsync(booking) as BadRequestObjectResult;

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.StatusCode, Is.EqualTo(400));
    }

    // ---------------------------------------------------------------------------
    // DeleteCourseBookingAsync
    // ---------------------------------------------------------------------------

    [Test]
    public async Task DeleteCourseBookingAsync_ReturnsNoContentWhenSuccessful()
    {
        // Create required entities first
        var student = new StudentModel { Id = 1, FirstName = "John", LastName = "Doe", Mail = "john@test.com" };
        var lesson = new TheoryLessonModel { Id = 1, Name = "Traffic", Topic = "Rules" };
        _dbContext.Students.Add(student);
        _dbContext.TheoryLessons.Add(lesson);
        await _dbContext.SaveChangesAsync();

        var booking = new CourseBookingModel { StudentId = 1, TheoryLessonId = 1 };
        var created = await _service.CreateCourseBookingAsync(booking);
        
        var result = await _controller.DeleteCourseBookingAsync(created!.Id);
        
        Assert.That(result, Is.InstanceOf<NoContentResult>());
    }

    [Test]
    public async Task DeleteCourseBookingAsync_ReturnsNotFoundWhenNotExists()
    {
        var result = await _controller.DeleteCourseBookingAsync(999) as NotFoundObjectResult;

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.StatusCode, Is.EqualTo(404));
    }

    // ---------------------------------------------------------------------------
    // UpdateCourseBookingAsync
    // ---------------------------------------------------------------------------

    [Test]
    public async Task UpdateCourseBookingAsync_ReturnsCreatedWhenSuccessful()
    {
        // Create required entities first
        var student = new StudentModel { Id = 1, FirstName = "John", LastName = "Doe", Mail = "john@test.com" };
        var lesson = new TheoryLessonModel { Id = 1, Name = "Traffic", Topic = "Rules" };
        _dbContext.Students.Add(student);
        _dbContext.TheoryLessons.Add(lesson);
        await _dbContext.SaveChangesAsync();

        var booking = new CourseBookingModel { StudentId = 1, TheoryLessonId = 1 };
        var created = await _service.CreateCourseBookingAsync(booking);
        created!.BookingDate = DateTime.Now.AddDays(1);

        var result = await _controller.UpdateCourseBookingAsync(created) as ObjectResult;

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.StatusCode, Is.EqualTo(201));
    }

    [Test]
    public async Task UpdateCourseBookingAsync_ReturnsNotFoundWhenNotExists()
    {
        var booking = new CourseBookingModel { Id = 999, StudentId = 1, TheoryLessonId = 1 };

        var result = await _controller.UpdateCourseBookingAsync(booking) as NotFoundObjectResult;

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.StatusCode, Is.EqualTo(404));
    }
}
