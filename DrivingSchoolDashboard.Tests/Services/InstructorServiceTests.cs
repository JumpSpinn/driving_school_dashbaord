namespace DrivingSchoolDashboard.Tests.Services;

[TestFixture]
public sealed class InstructorServiceTests
{
    private ApplicationDbContext _dbContext = null!;
    private InstructorService _service = null!;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new ApplicationDbContext(options);
        var logger = NullLogger<InstructorService>.Instance;
        _service = new InstructorService(_dbContext, logger);
    }

    [TearDown]
    public void TearDown()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }

    // ---------------------------------------------------------------------------
    // GetInstructors
    // ---------------------------------------------------------------------------

    [Test]
    public void GetInstructors_ReturnsEmptyListWhenNoInstructors()
    {
        var result = _service.GetInstructors();

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Empty);
    }

    [Test]
    public void GetInstructors_ReturnsAllNonDeletedInstructors()
    {
        _dbContext.Instructors.AddRange(
            new InstructorModel { Id = 1, FirstName = "John", LastName = "Doe", IsDeleted = false },
            new InstructorModel { Id = 2, FirstName = "Jane", LastName = "Smith", IsDeleted = false },
            new InstructorModel { Id = 3, FirstName = "Bob", LastName = "Brown", IsDeleted = true }
        );
        _dbContext.SaveChanges();

        var result = _service.GetInstructors();

        Assert.That(result.Count, Is.EqualTo(2));
        Assert.That(result.All(i => !i.IsDeleted), Is.True);
    }

    // ---------------------------------------------------------------------------
    // CreateInstructorAsync
    // ---------------------------------------------------------------------------

    [Test]
    public async Task CreateInstructorAsync_CreatesNewInstructor()
    {
        var instructor = new InstructorModel 
        { 
            FirstName = "John", 
            LastName = "Doe", 
            Mail = "john@test.com",
            Phone = "123456789"
        };

        var result = await _service.CreateInstructorAsync(instructor);

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.FirstName, Is.EqualTo("John"));
        Assert.That(_dbContext.Instructors.Count(), Is.EqualTo(1));
    }

    [Test]
    public async Task CreateInstructorAsync_ReturnsExistingIfIdExists()
    {
        var existing = new InstructorModel { Id = 1, FirstName = "John", LastName = "Doe" };
        _dbContext.Instructors.Add(existing);
        _dbContext.SaveChanges();

        var instructor = new InstructorModel { Id = 1, FirstName = "Jane", LastName = "Smith" };
        var result = await _service.CreateInstructorAsync(instructor);

        Assert.That(result, Is.Not.Null);
        Assert.That(_dbContext.Instructors.Count(), Is.EqualTo(1));
    }

    // ---------------------------------------------------------------------------
    // UpdateInstructorAsync
    // ---------------------------------------------------------------------------

    [Test]
    public async Task UpdateInstructorAsync_UpdatesExistingInstructor()
    {
        var instructor = new InstructorModel 
        { 
            Id = 1, 
            FirstName = "John", 
            LastName = "Doe", 
            Mail = "old@test.com" 
        };
        _dbContext.Instructors.Add(instructor);
        _dbContext.SaveChanges();

        instructor.Mail = "new@test.com";
        instructor.FirstName = "Johnny";
        var result = await _service.UpdateInstructorAsync(instructor);

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Mail, Is.EqualTo("new@test.com"));
        Assert.That(result.FirstName, Is.EqualTo("Johnny"));
    }

    [Test]
    public async Task UpdateInstructorAsync_ReturnsNullWhenNotFound()
    {
        var instructor = new InstructorModel { Id = 999, FirstName = "Unknown", LastName = "User" };

        var result = await _service.UpdateInstructorAsync(instructor);

        Assert.That(result, Is.Null);
    }

    // ---------------------------------------------------------------------------
    // DeleteInstructorAsync
    // ---------------------------------------------------------------------------

    [Test]
    public async Task DeleteInstructorAsync_SoftDeletesInstructor()
    {
        var instructor = new InstructorModel { Id = 1, FirstName = "John", LastName = "Doe", IsDeleted = false };
        _dbContext.Instructors.Add(instructor);
        _dbContext.SaveChanges();

        var result = await _service.DeleteInstructorAsync(1);

        Assert.That(result, Is.True);
        var deleted = _dbContext.Instructors.Find(1);
        Assert.That(deleted!.IsDeleted, Is.True);
    }

    [Test]
    public async Task DeleteInstructorAsync_ReturnsFalseWhenNotFound()
    {
        var result = await _service.DeleteInstructorAsync(999);

        Assert.That(result, Is.False);
    }

    [Test]
    public async Task DeleteInstructorAsync_ReturnsFalseWhenAlreadyDeleted()
    {
        var instructor = new InstructorModel { Id = 1, FirstName = "John", LastName = "Doe", IsDeleted = true };
        _dbContext.Instructors.Add(instructor);
        _dbContext.SaveChanges();

        var result = await _service.DeleteInstructorAsync(1);

        Assert.That(result, Is.False);
    }
}
