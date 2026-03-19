namespace DrivingSchoolDashboard.Tests.Models;

[TestFixture]
public sealed class DrivingSchoolModelTests
{
    // ---------------------------------------------------------------------------
    // Validation Tests
    // ---------------------------------------------------------------------------

    [Test]
    public void DrivingSchoolModel_RequiresName()
    {
        var school = new DrivingSchoolModel { Name = "" };
        var context = new ValidationContext(school);
        var results = new List<ValidationResult>();

        var isValid = Validator.TryValidateObject(school, context, results, true);

        Assert.That(isValid, Is.False);
        Assert.That(results.Any(r => r.MemberNames.Contains("Name")), Is.True);
    }

    [Test]
    public void DrivingSchoolModel_NameMinLength3()
    {
        var school = new DrivingSchoolModel { Name = "AB" };
        var context = new ValidationContext(school);
        var results = new List<ValidationResult>();

        var isValid = Validator.TryValidateObject(school, context, results, true);

        Assert.That(isValid, Is.False);
        Assert.That(results.Any(r => r.MemberNames.Contains("Name")), Is.True);
    }

    [Test]
    public void DrivingSchoolModel_NameMaxLength50()
    {
        var school = new DrivingSchoolModel { Name = new string('A', 51) };
        var context = new ValidationContext(school);
        var results = new List<ValidationResult>();

        var isValid = Validator.TryValidateObject(school, context, results, true);

        Assert.That(isValid, Is.False);
        Assert.That(results.Any(r => r.MemberNames.Contains("Name")), Is.True);
    }

    [Test]
    public void DrivingSchoolModel_ValidWhenNameIsValid()
    {
        var school = new DrivingSchoolModel { Name = "Valid School Name" };
        var context = new ValidationContext(school);
        var results = new List<ValidationResult>();

        var isValid = Validator.TryValidateObject(school, context, results, true);

        Assert.That(isValid, Is.True);
    }

    // ---------------------------------------------------------------------------
    // Copy Constructor Tests
    // ---------------------------------------------------------------------------

    [Test]
    public void DrivingSchoolModel_CopyConstructor_CopiesAllProperties()
    {
        var student1 = new StudentModel { Id = 1, FirstName = "John", LastName = "Doe" };
        var original = new DrivingSchoolModel
        {
            Id = 1,
            Name = "Test School",
            OwnerId = 10,
            IsDeleted = false,
            Students = new List<StudentModel> { student1 }
        };

        var copy = new DrivingSchoolModel(original);

        Assert.That(copy.Id, Is.EqualTo(original.Id));
        Assert.That(copy.Name, Is.EqualTo(original.Name));
        Assert.That(copy.OwnerId, Is.EqualTo(original.OwnerId));
        Assert.That(copy.IsDeleted, Is.EqualTo(original.IsDeleted));
        Assert.That(copy.Students.Count, Is.EqualTo(1));
    }

    // ---------------------------------------------------------------------------
    // Default Values Tests
    // ---------------------------------------------------------------------------

    [Test]
    public void DrivingSchoolModel_DefaultIsDeleted_IsFalse()
    {
        var school = new DrivingSchoolModel();

        Assert.That(school.IsDeleted, Is.False);
    }

    [Test]
    public void DrivingSchoolModel_DefaultStudents_IsEmptyCollection()
    {
        var school = new DrivingSchoolModel();

        Assert.That(school.Students, Is.Not.Null);
        Assert.That(school.Students, Is.Empty);
    }
}
