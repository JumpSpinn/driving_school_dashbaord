namespace DrivingSchoolDashboard.Tests.Services;

[TestFixture]
public sealed class DrivingSchoolServiceTests
{
    private ApplicationDbContext _dbContext = null!;
    private DrivingSchoolService _service = null!;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new ApplicationDbContext(options);
        var logger = NullLogger<DrivingSchoolService>.Instance;
        _service = new DrivingSchoolService(_dbContext, logger);
    }

    [TearDown]
    public void TearDown()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }

    // ---------------------------------------------------------------------------
    // GetAllDrivingSchools
    // ---------------------------------------------------------------------------

    [Test]
    public void GetAllDrivingSchools_ReturnsEmptyListWhenNoSchools()
    {
        var result = _service.GetAllDrivingSchools();

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Empty);
    }

    [Test]
    public void GetAllDrivingSchools_ReturnsAllNonDeletedSchools()
    {
        _dbContext.DrivingSchools.AddRange(
            new DrivingSchoolModel { Id = 1, Name = "School A", IsDeleted = false },
            new DrivingSchoolModel { Id = 2, Name = "School B", IsDeleted = false },
            new DrivingSchoolModel { Id = 3, Name = "School C", IsDeleted = true }
        );
        _dbContext.SaveChanges();

        var result = _service.GetAllDrivingSchools();

        Assert.That(result.Count, Is.EqualTo(2));
        Assert.That(result.All(s => !s.IsDeleted), Is.True);
    }

    // ---------------------------------------------------------------------------
    // CreateDrivingSchoolAsync
    // ---------------------------------------------------------------------------

    [Test]
    public async Task CreateDrivingSchoolAsync_CreatesNewSchool()
    {
        var school = new DrivingSchoolModel { Name = "New School", OwnerId = 1 };

        var result = await _service.CreateDrivingSchoolAsync(school);

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Name, Is.EqualTo("New School"));
        Assert.That(_dbContext.DrivingSchools.Count(), Is.EqualTo(1));
    }

    [Test]
    public async Task CreateDrivingSchoolAsync_ReturnsExistingIfNameExists()
    {
        var existing = new DrivingSchoolModel { Id = 1, Name = "Existing School" };
        _dbContext.DrivingSchools.Add(existing);
        _dbContext.SaveChanges();

        var school = new DrivingSchoolModel { Name = "Existing School", OwnerId = 2 };
        var result = await _service.CreateDrivingSchoolAsync(school);

        Assert.That(result, Is.Not.Null);
        Assert.That(_dbContext.DrivingSchools.Count(), Is.EqualTo(1));
    }

    // ---------------------------------------------------------------------------
    // UpdateDrivingSchoolAsync
    // ---------------------------------------------------------------------------

    [Test]
    public async Task UpdateDrivingSchoolAsync_UpdatesExistingSchool()
    {
        var school = new DrivingSchoolModel { Id = 1, Name = "Old Name", OwnerId = 1 };
        _dbContext.DrivingSchools.Add(school);
        _dbContext.SaveChanges();

        school.Name = "New Name";
        var result = await _service.UpdateDrivingSchoolAsync(school);

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Name, Is.EqualTo("New Name"));
    }

    [Test]
    public async Task UpdateDrivingSchoolAsync_ReturnsNullWhenNotFound()
    {
        var school = new DrivingSchoolModel { Id = 999, Name = "Unknown" };

        var result = await _service.UpdateDrivingSchoolAsync(school);

        Assert.That(result, Is.Null);
    }

    // ---------------------------------------------------------------------------
    // DeleteDrivingSchoolAsync
    // ---------------------------------------------------------------------------

    [Test]
    public async Task DeleteDrivingSchoolAsync_SoftDeletesSchool()
    {
        var school = new DrivingSchoolModel { Id = 1, Name = "School", IsDeleted = false };
        _dbContext.DrivingSchools.Add(school);
        _dbContext.SaveChanges();

        var result = await _service.DeleteDrivingSchoolAsync(1);

        Assert.That(result, Is.True);
        var deleted = _dbContext.DrivingSchools.Find(1);
        Assert.That(deleted!.IsDeleted, Is.True);
    }

    [Test]
    public async Task DeleteDrivingSchoolAsync_RemovesStudentAssociations()
    {
        var school = new DrivingSchoolModel { Id = 1, Name = "School" };
        var student = new StudentModel { Id = 1, FirstName = "John", LastName = "Doe", DrivingSchoolId = 1 };
        _dbContext.DrivingSchools.Add(school);
        _dbContext.Students.Add(student);
        _dbContext.SaveChanges();

        var result = await _service.DeleteDrivingSchoolAsync(1);

        Assert.That(result, Is.True);
        var updatedStudent = _dbContext.Students.Find(1);
        Assert.That(updatedStudent!.DrivingSchoolId, Is.Null);
    }

    [Test]
    public async Task DeleteDrivingSchoolAsync_ReturnsFalseWhenNotFound()
    {
        var result = await _service.DeleteDrivingSchoolAsync(999);

        Assert.That(result, Is.False);
    }

    [Test]
    public async Task DeleteDrivingSchoolAsync_ReturnsFalseWhenAlreadyDeleted()
    {
        var school = new DrivingSchoolModel { Id = 1, Name = "School", IsDeleted = true };
        _dbContext.DrivingSchools.Add(school);
        _dbContext.SaveChanges();

        var result = await _service.DeleteDrivingSchoolAsync(1);

        Assert.That(result, Is.False);
    }
}
