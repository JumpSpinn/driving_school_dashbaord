namespace ExampleAspireBlazorApiProject.Tests.Services;

[TestFixture]
public sealed class StudentServiceTests
{
    private ApplicationDbContext _dbContext = null!;
    private StudentService _service = null!;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new ApplicationDbContext(options);
        var logger = NullLogger<StudentService>.Instance;
        _service = new StudentService(_dbContext, logger);
    }

    [TearDown]
    public void TearDown()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }

    // ---------------------------------------------------------------------------
    // GetAllDrivers
    // ---------------------------------------------------------------------------

    [Test]
    public void GetAllDrivers_ReturnsEmptyListWhenNoStudents()
    {
        var result = _service.GetAllDrivers();

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Empty);
    }

    [Test]
    public void GetAllDrivers_ReturnsAllNonDeletedStudents()
    {
        _dbContext.Students.AddRange(
            new StudentModel { Id = 1, FirstName = "John", LastName = "Doe", IsDeleted = false },
            new StudentModel { Id = 2, FirstName = "Jane", LastName = "Smith", IsDeleted = false },
            new StudentModel { Id = 3, FirstName = "Bob", LastName = "Brown", IsDeleted = true }
        );
        _dbContext.SaveChanges();

        var result = _service.GetAllDrivers();

        Assert.That(result.Count, Is.EqualTo(2));
        Assert.That(result.All(s => !s.IsDeleted), Is.True);
    }

    // ---------------------------------------------------------------------------
    // CreateStudentAsync
    // ---------------------------------------------------------------------------

    [Test]
    public async Task CreateStudentAsync_CreatesNewStudent()
    {
        var student = new StudentModel { FirstName = "John", LastName = "Doe", Mail = "john@test.com" };

        var result = await _service.CreateStudentAsync(student);

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.FirstName, Is.EqualTo("John"));
        Assert.That(_dbContext.Students.Count(), Is.EqualTo(1));
    }

    // ---------------------------------------------------------------------------
    // UpdateStudentAsync
    // ---------------------------------------------------------------------------

    [Test]
    public async Task UpdateStudentAsync_UpdatesExistingStudent()
    {
        var student = new StudentModel 
        { 
            Id = 1, 
            FirstName = "John", 
            LastName = "Doe", 
            Mail = "old@test.com" 
        };
        _dbContext.Students.Add(student);
        _dbContext.SaveChanges();

        student.Mail = "new@test.com";
        student.FirstName = "Johnny";
        var result = await _service.UpdateStudentAsync(student);

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Mail, Is.EqualTo("new@test.com"));
        Assert.That(result.FirstName, Is.EqualTo("Johnny"));
    }

    [Test]
    public async Task UpdateStudentAsync_ReturnsNullWhenNotFound()
    {
        var student = new StudentModel { Id = 999, FirstName = "Unknown", LastName = "User" };

        var result = await _service.UpdateStudentAsync(student);

        Assert.That(result, Is.Null);
    }

    // ---------------------------------------------------------------------------
    // DeleteStudentAsync
    // ---------------------------------------------------------------------------

    [Test]
    public async Task DeleteStudentAsync_DeletesExistingStudent()
    {
        var student = new StudentModel { Id = 1, FirstName = "John", LastName = "Doe" };
        _dbContext.Students.Add(student);
        _dbContext.SaveChanges();

        var result = await _service.DeleteStudentAsync(1);

        Assert.That(result, Is.True);
        Assert.That(_dbContext.Students.Count(), Is.EqualTo(0));
    }

    [Test]
    public async Task DeleteStudentAsync_ReturnsFalseWhenNotFound()
    {
        var result = await _service.DeleteStudentAsync(999);

        Assert.That(result, Is.False);
    }

    [Test]
    public async Task DeleteStudentAsync_ReturnsFalseWhenAlreadyDeleted()
    {
        var student = new StudentModel { Id = 1, FirstName = "John", LastName = "Doe", IsDeleted = true };
        _dbContext.Students.Add(student);
        _dbContext.SaveChanges();

        var result = await _service.DeleteStudentAsync(1);

        Assert.That(result, Is.False);
    }
}
