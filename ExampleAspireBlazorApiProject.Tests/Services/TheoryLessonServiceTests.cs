namespace ExampleAspireBlazorApiProject.Tests.Services;

[TestFixture]
public sealed class TheoryLessonServiceTests
{
    private ApplicationDbContext _dbContext = null!;
    private TheoryLessonService _service = null!;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new ApplicationDbContext(options);
        var logger = NullLogger<TheoryLessonService>.Instance;
        _service = new TheoryLessonService(_dbContext, logger);
    }

    [TearDown]
    public void TearDown()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }

    // ---------------------------------------------------------------------------
    // GetTheoryLessons
    // ---------------------------------------------------------------------------

    [Test]
    public void GetTheoryLessons_ReturnsEmptyListWhenNoLessons()
    {
        var result = _service.GetTheoryLessons();

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Empty);
    }

    [Test]
    public void GetTheoryLessons_ReturnsAllNonDeletedLessons()
    {
        _dbContext.TheoryLessons.AddRange(
            new TheoryLessonModel { Id = 1, Name = "Lesson A", IsDeleted = false },
            new TheoryLessonModel { Id = 2, Name = "Lesson B", IsDeleted = false },
            new TheoryLessonModel { Id = 3, Name = "Lesson C", IsDeleted = true }
        );
        _dbContext.SaveChanges();

        var result = _service.GetTheoryLessons();

        Assert.That(result.Count, Is.EqualTo(2));
        Assert.That(result.All(l => !l.IsDeleted), Is.True);
    }

    // ---------------------------------------------------------------------------
    // CreateTheoryLessonAsync
    // ---------------------------------------------------------------------------

    [Test]
    public async Task CreateTheoryLessonAsync_CreatesNewLesson()
    {
        var lesson = new TheoryLessonModel 
        { 
            Name = "Traffic Rules", 
            Topic = "Basic rules",
            DurationMinutes = 90
        };

        var result = await _service.CreateTheoryLessonAsync(lesson);

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Name, Is.EqualTo("Traffic Rules"));
        Assert.That(_dbContext.TheoryLessons.Count(), Is.EqualTo(1));
    }

    [Test]
    public async Task CreateTheoryLessonAsync_ReturnsExistingIfIdExists()
    {
        var existing = new TheoryLessonModel { Id = 1, Name = "Existing Lesson" };
        _dbContext.TheoryLessons.Add(existing);
        _dbContext.SaveChanges();

        var lesson = new TheoryLessonModel { Id = 1, Name = "New Lesson" };
        var result = await _service.CreateTheoryLessonAsync(lesson);

        Assert.That(result, Is.Not.Null);
        Assert.That(_dbContext.TheoryLessons.Count(), Is.EqualTo(1));
    }

    // ---------------------------------------------------------------------------
    // UpdateTheoryLessonAsync
    // ---------------------------------------------------------------------------

    [Test]
    public async Task UpdateTheoryLessonAsync_UpdatesExistingLesson()
    {
        var lesson = new TheoryLessonModel 
        { 
            Id = 1, 
            Name = "Old Name", 
            Topic = "Old Topic",
            DurationMinutes = 60
        };
        _dbContext.TheoryLessons.Add(lesson);
        _dbContext.SaveChanges();

        lesson.Name = "New Name";
        lesson.Topic = "New Topic";
        lesson.DurationMinutes = 90;
        var result = await _service.UpdateTheoryLessonAsync(lesson);

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Name, Is.EqualTo("New Name"));
        Assert.That(result.Topic, Is.EqualTo("New Topic"));
        Assert.That(result.DurationMinutes, Is.EqualTo(90));
    }

    [Test]
    public async Task UpdateTheoryLessonAsync_ReturnsNullWhenNotFound()
    {
        var lesson = new TheoryLessonModel { Id = 999, Name = "Unknown" };

        var result = await _service.UpdateTheoryLessonAsync(lesson);

        Assert.That(result, Is.Null);
    }

    // ---------------------------------------------------------------------------
    // DeleteTheoryLessonAsync
    // ---------------------------------------------------------------------------

    [Test]
    public async Task DeleteTheoryLessonAsync_SoftDeletesLesson()
    {
        var lesson = new TheoryLessonModel { Id = 1, Name = "Lesson", IsDeleted = false };
        _dbContext.TheoryLessons.Add(lesson);
        _dbContext.SaveChanges();

        var result = await _service.DeleteTheoryLessonAsync(1);

        Assert.That(result, Is.True);
        var deleted = _dbContext.TheoryLessons.Find(1);
        Assert.That(deleted!.IsDeleted, Is.True);
    }

    [Test]
    public async Task DeleteTheoryLessonAsync_RemovesFromInstructorCollection()
    {
        var instructor = new InstructorModel { Id = 1, FirstName = "John", LastName = "Doe" };
        var lesson = new TheoryLessonModel { Id = 1, Name = "Lesson", InstructorId = 1 };
        _dbContext.Instructors.Add(instructor);
        _dbContext.TheoryLessons.Add(lesson);
        _dbContext.SaveChanges();

        var result = await _service.DeleteTheoryLessonAsync(1);

        Assert.That(result, Is.True);
        var updatedInstructor = _dbContext.Instructors.Include(i => i.TheoryLessons).First(i => i.Id == 1);
        Assert.That(updatedInstructor.TheoryLessons.Any(l => l.Id == 1), Is.False);
    }

    [Test]
    public async Task DeleteTheoryLessonAsync_ReturnsFalseWhenNotFound()
    {
        var result = await _service.DeleteTheoryLessonAsync(999);

        Assert.That(result, Is.False);
    }

    [Test]
    public async Task DeleteTheoryLessonAsync_ReturnsFalseWhenAlreadyDeleted()
    {
        var lesson = new TheoryLessonModel { Id = 1, Name = "Lesson", IsDeleted = true };
        _dbContext.TheoryLessons.Add(lesson);
        _dbContext.SaveChanges();

        var result = await _service.DeleteTheoryLessonAsync(1);

        Assert.That(result, Is.False);
    }
}
