namespace DrivingSchoolDashboard.Tests.Models;

[TestFixture]
public sealed class InstructorModelTests
{
    // ---------------------------------------------------------------------------
    // Validation Tests - FirstName
    // ---------------------------------------------------------------------------

    [Test]
    public void InstructorModel_RequiresFirstName()
    {
        var instructor = new InstructorModel { FirstName = "", LastName = "Smith", Mail = "test@test.com" };
        var context = new ValidationContext(instructor);
        var results = new List<ValidationResult>();

        var isValid = Validator.TryValidateObject(instructor, context, results, true);

        Assert.That(isValid, Is.False);
        Assert.That(results.Any(r => r.MemberNames.Contains("FirstName")), Is.True);
    }

    [Test]
    public void InstructorModel_FirstNameMinLength3()
    {
        var instructor = new InstructorModel { FirstName = "AB", LastName = "Smith", Mail = "test@test.com" };
        var context = new ValidationContext(instructor);
        var results = new List<ValidationResult>();

        var isValid = Validator.TryValidateObject(instructor, context, results, true);

        Assert.That(isValid, Is.False);
    }

    [Test]
    public void InstructorModel_FirstNameMaxLength50()
    {
        var instructor = new InstructorModel { FirstName = new string('A', 51), LastName = "Smith", Mail = "test@test.com" };
        var context = new ValidationContext(instructor);
        var results = new List<ValidationResult>();

        var isValid = Validator.TryValidateObject(instructor, context, results, true);

        Assert.That(isValid, Is.False);
    }

    // ---------------------------------------------------------------------------
    // Validation Tests - LastName
    // ---------------------------------------------------------------------------

    [Test]
    public void InstructorModel_RequiresLastName()
    {
        var instructor = new InstructorModel { FirstName = "Jane", LastName = "", Mail = "test@test.com" };
        var context = new ValidationContext(instructor);
        var results = new List<ValidationResult>();

        var isValid = Validator.TryValidateObject(instructor, context, results, true);

        Assert.That(isValid, Is.False);
        Assert.That(results.Any(r => r.MemberNames.Contains("LastName")), Is.True);
    }

    // ---------------------------------------------------------------------------
    // Validation Tests - Mail
    // ---------------------------------------------------------------------------

    [Test]
    public void InstructorModel_RequiresMail()
    {
        var instructor = new InstructorModel { FirstName = "Jane", LastName = "Smith", Mail = "" };
        var context = new ValidationContext(instructor);
        var results = new List<ValidationResult>();

        var isValid = Validator.TryValidateObject(instructor, context, results, true);

        Assert.That(isValid, Is.False);
        Assert.That(results.Any(r => r.MemberNames.Contains("Mail")), Is.True);
    }

    [Test]
    public void InstructorModel_MailMustBeValidEmail()
    {
        var instructor = new InstructorModel { FirstName = "Jane", LastName = "Smith", Mail = "invalid-email" };
        var context = new ValidationContext(instructor);
        var results = new List<ValidationResult>();

        var isValid = Validator.TryValidateObject(instructor, context, results, true);

        Assert.That(isValid, Is.False);
        Assert.That(results.Any(r => r.MemberNames.Contains("Mail")), Is.True);
    }

    [Test]
    public void InstructorModel_ValidWhenAllRequiredFieldsProvided()
    {
        var instructor = new InstructorModel { FirstName = "Jane", LastName = "Smith", Mail = "jane@test.com" };
        var context = new ValidationContext(instructor);
        var results = new List<ValidationResult>();

        var isValid = Validator.TryValidateObject(instructor, context, results, true);

        Assert.That(isValid, Is.True);
    }

    // ---------------------------------------------------------------------------
    // FullName Property Tests
    // ---------------------------------------------------------------------------

    [Test]
    public void InstructorModel_FullName_CombinesFirstAndLastName()
    {
        var instructor = new InstructorModel { FirstName = "Jane", LastName = "Smith" };

        Assert.That(instructor.FullName, Is.EqualTo("Jane Smith"));
    }

    // ---------------------------------------------------------------------------
    // Copy Constructor Tests
    // ---------------------------------------------------------------------------

    [Test]
    public void InstructorModel_CopyConstructor_CopiesAllProperties()
    {
        var lesson1 = new TheoryLessonModel { Id = 1, Name = "Test", Topic = "Test" };
        var original = new InstructorModel
        {
            Id = 1,
            FirstName = "Jane",
            LastName = "Smith",
            Mail = "jane@test.com",
            Phone = "987654321",
            IsDeleted = false,
            TheoryLessons = new List<TheoryLessonModel> { lesson1 }
        };

        var copy = new InstructorModel(original);

        Assert.That(copy.Id, Is.EqualTo(original.Id));
        Assert.That(copy.FirstName, Is.EqualTo(original.FirstName));
        Assert.That(copy.LastName, Is.EqualTo(original.LastName));
        Assert.That(copy.Mail, Is.EqualTo(original.Mail));
        Assert.That(copy.Phone, Is.EqualTo(original.Phone));
        Assert.That(copy.IsDeleted, Is.EqualTo(original.IsDeleted));
        Assert.That(copy.TheoryLessons.Count, Is.EqualTo(1));
    }

    // ---------------------------------------------------------------------------
    // Default Values Tests
    // ---------------------------------------------------------------------------

    [Test]
    public void InstructorModel_DefaultIsDeleted_IsFalse()
    {
        var instructor = new InstructorModel();

        Assert.That(instructor.IsDeleted, Is.False);
    }

    [Test]
    public void InstructorModel_DefaultTheoryLessons_IsEmptyCollection()
    {
        var instructor = new InstructorModel();

        Assert.That(instructor.TheoryLessons, Is.Not.Null);
        Assert.That(instructor.TheoryLessons, Is.Empty);
    }
}
