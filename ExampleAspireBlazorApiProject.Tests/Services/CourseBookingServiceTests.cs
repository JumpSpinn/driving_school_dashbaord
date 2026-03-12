namespace ExampleAspireBlazorApiProject.Tests.Services;

[TestFixture]
public sealed class CourseBookingServiceTests
{
    private ApplicationDbContext _dbContext = null!;
    private CourseBookingService _service = null!;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new ApplicationDbContext(options);
        var logger = NullLogger<CourseBookingService>.Instance;
        _service = new CourseBookingService(_dbContext, logger);
    }

    [TearDown]
    public void TearDown()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }

    // ---------------------------------------------------------------------------
    // GetCourseBookings
    // ---------------------------------------------------------------------------

    [Test]
    public void GetCourseBookings_ReturnsEmptyListWhenNoBookings()
    {
        var result = _service.GetCourseBookings();

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Empty);
    }

    [Test]
    public void GetCourseBookings_ReturnsAllBookings()
    {
        var student = new StudentModel { Id = 1, FirstName = "John", LastName = "Doe" };
        var lesson = new TheoryLessonModel { Id = 1, Name = "Traffic Rules" };
        _dbContext.Students.Add(student);
        _dbContext.TheoryLessons.Add(lesson);
        _dbContext.CourseBookings.AddRange(
            new CourseBookingModel { StudentId = 1, TheoryLessonId = 1, BookingDate = DateTime.Now },
            new CourseBookingModel { StudentId = 1, TheoryLessonId = 1, BookingDate = DateTime.Now.AddDays(1) }
        );
        _dbContext.SaveChanges();

        var result = _service.GetCourseBookings();

        Assert.That(result.Count, Is.EqualTo(2));
    }

    // ---------------------------------------------------------------------------
    // CreateCourseBookingAsync
    // ---------------------------------------------------------------------------

    [Test]
    public async Task CreateCourseBookingAsync_CreatesNewBooking()
    {
        var student = new StudentModel { Id = 1, FirstName = "John", LastName = "Doe" };
        var lesson = new TheoryLessonModel { Id = 1, Name = "Traffic Rules" };
        _dbContext.Students.Add(student);
        _dbContext.TheoryLessons.Add(lesson);
        _dbContext.SaveChanges();

        var booking = new CourseBookingModel { StudentId = 1, TheoryLessonId = 1, BookingDate = DateTime.Now };

        var result = await _service.CreateCourseBookingAsync(booking);

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.StudentId, Is.EqualTo(1));
        Assert.That(result.TheoryLessonId, Is.EqualTo(1));
    }

    [Test]
    public async Task CreateCourseBookingAsync_ReturnsExistingIfDuplicate()
    {
        var student = new StudentModel { Id = 1, FirstName = "John", LastName = "Doe" };
        var lesson = new TheoryLessonModel { Id = 1, Name = "Traffic Rules" };
        var existing = new CourseBookingModel { StudentId = 1, TheoryLessonId = 1, BookingDate = DateTime.Now };
        _dbContext.Students.Add(student);
        _dbContext.TheoryLessons.Add(lesson);
        _dbContext.CourseBookings.Add(existing);
        _dbContext.SaveChanges();

        var booking = new CourseBookingModel { StudentId = 1, TheoryLessonId = 1, BookingDate = DateTime.Now.AddDays(1) };

        var result = await _service.CreateCourseBookingAsync(booking);

        Assert.That(result, Is.Not.Null);
        Assert.That(_dbContext.CourseBookings.Count(), Is.EqualTo(1));
    }

    [Test]
    public async Task CreateCourseBookingAsync_ReturnsNullWhenIdsAreNull()
    {
        var booking = new CourseBookingModel { StudentId = null, TheoryLessonId = null };

        var result = await _service.CreateCourseBookingAsync(booking);

        Assert.That(result, Is.Null);
    }

    // ---------------------------------------------------------------------------
    // UpdateCourseBookingAsync
    // ---------------------------------------------------------------------------

    [Test]
    public async Task UpdateCourseBookingAsync_UpdatesExistingBooking()
    {
        var student = new StudentModel { Id = 1, FirstName = "John", LastName = "Doe" };
        var lesson1 = new TheoryLessonModel { Id = 1, Name = "Traffic Rules" };
        var lesson2 = new TheoryLessonModel { Id = 2, Name = "Road Signs" };
        var booking = new CourseBookingModel { Id = 1, StudentId = 1, TheoryLessonId = 1, BookingDate = DateTime.Now };
        _dbContext.Students.Add(student);
        _dbContext.TheoryLessons.AddRange(lesson1, lesson2);
        _dbContext.CourseBookings.Add(booking);
        _dbContext.SaveChanges();

        booking.TheoryLessonId = 2;
        var result = await _service.UpdateCourseBookingAsync(booking);

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.TheoryLessonId, Is.EqualTo(2));
    }

    [Test]
    public async Task UpdateCourseBookingAsync_ReturnsNullWhenNotFound()
    {
        var booking = new CourseBookingModel { Id = 999, StudentId = 1, TheoryLessonId = 1 };

        var result = await _service.UpdateCourseBookingAsync(booking);

        Assert.That(result, Is.Null);
    }

    // ---------------------------------------------------------------------------
    // DeleteCourseBookingAsync
    // ---------------------------------------------------------------------------

    [Test]
    public async Task DeleteCourseBookingAsync_DeletesExistingBooking()
    {
        var student = new StudentModel { Id = 1, FirstName = "John", LastName = "Doe" };
        var lesson = new TheoryLessonModel { Id = 1, Name = "Traffic Rules" };
        var booking = new CourseBookingModel { Id = 1, StudentId = 1, TheoryLessonId = 1 };
        _dbContext.Students.Add(student);
        _dbContext.TheoryLessons.Add(lesson);
        _dbContext.CourseBookings.Add(booking);
        _dbContext.SaveChanges();

        var result = await _service.DeleteCourseBookingAsync(1);

        Assert.That(result, Is.True);
        Assert.That(_dbContext.CourseBookings.Count(), Is.EqualTo(0));
    }

    [Test]
    public async Task DeleteCourseBookingAsync_ReturnsFalseWhenNotFound()
    {
        var result = await _service.DeleteCourseBookingAsync(999);

        Assert.That(result, Is.False);
    }
}
