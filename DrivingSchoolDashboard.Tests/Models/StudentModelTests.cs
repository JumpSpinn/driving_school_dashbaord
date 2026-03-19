namespace DrivingSchoolDashboard.Tests.Models;

[TestFixture]
public sealed class StudentModelTests
{
    // ---------------------------------------------------------------------------
    // Validation Tests - FirstName
    // ---------------------------------------------------------------------------

    [Test]
    public void StudentModel_RequiresFirstName()
    {
        var student = new StudentModel { FirstName = "", LastName = "Doe", Mail = "test@test.com" };
        var context = new ValidationContext(student);
        var results = new List<ValidationResult>();

        var isValid = Validator.TryValidateObject(student, context, results, true);

        Assert.That(isValid, Is.False);
        Assert.That(results.Any(r => r.MemberNames.Contains("FirstName")), Is.True);
    }

    [Test]
    public void StudentModel_FirstNameMinLength3()
    {
        var student = new StudentModel { FirstName = "AB", LastName = "Doe", Mail = "test@test.com" };
        var context = new ValidationContext(student);
        var results = new List<ValidationResult>();

        var isValid = Validator.TryValidateObject(student, context, results, true);

        Assert.That(isValid, Is.False);
    }

    [Test]
    public void StudentModel_FirstNameMaxLength50()
    {
        var student = new StudentModel { FirstName = new string('A', 51), LastName = "Doe", Mail = "test@test.com" };
        var context = new ValidationContext(student);
        var results = new List<ValidationResult>();

        var isValid = Validator.TryValidateObject(student, context, results, true);

        Assert.That(isValid, Is.False);
    }

    // ---------------------------------------------------------------------------
    // Validation Tests - LastName
    // ---------------------------------------------------------------------------

    [Test]
    public void StudentModel_RequiresLastName()
    {
        var student = new StudentModel { FirstName = "John", LastName = "", Mail = "test@test.com" };
        var context = new ValidationContext(student);
        var results = new List<ValidationResult>();

        var isValid = Validator.TryValidateObject(student, context, results, true);

        Assert.That(isValid, Is.False);
        Assert.That(results.Any(r => r.MemberNames.Contains("LastName")), Is.True);
    }

    // ---------------------------------------------------------------------------
    // Validation Tests - Mail
    // ---------------------------------------------------------------------------

    [Test]
    public void StudentModel_RequiresMail()
    {
        var student = new StudentModel { FirstName = "John", LastName = "Doe", Mail = "" };
        var context = new ValidationContext(student);
        var results = new List<ValidationResult>();

        var isValid = Validator.TryValidateObject(student, context, results, true);

        Assert.That(isValid, Is.False);
        Assert.That(results.Any(r => r.MemberNames.Contains("Mail")), Is.True);
    }

    [Test]
    public void StudentModel_MailMustBeValidEmail()
    {
        var student = new StudentModel { FirstName = "John", LastName = "Doe", Mail = "invalid-email" };
        var context = new ValidationContext(student);
        var results = new List<ValidationResult>();

        var isValid = Validator.TryValidateObject(student, context, results, true);

        Assert.That(isValid, Is.False);
        Assert.That(results.Any(r => r.MemberNames.Contains("Mail")), Is.True);
    }

    [Test]
    public void StudentModel_ValidWhenAllRequiredFieldsProvided()
    {
        var student = new StudentModel { FirstName = "John", LastName = "Doe", Mail = "john@test.com" };
        var context = new ValidationContext(student);
        var results = new List<ValidationResult>();

        var isValid = Validator.TryValidateObject(student, context, results, true);

        Assert.That(isValid, Is.True);
    }

    // ---------------------------------------------------------------------------
    // FullName Property Tests
    // ---------------------------------------------------------------------------

    [Test]
    public void StudentModel_FullName_CombinesFirstAndLastName()
    {
        var student = new StudentModel { FirstName = "John", LastName = "Doe" };

        Assert.That(student.FullName, Is.EqualTo("John Doe"));
    }

    // ---------------------------------------------------------------------------
    // Copy Constructor Tests
    // ---------------------------------------------------------------------------

    [Test]
    public void StudentModel_CopyConstructor_CopiesAllProperties()
    {
        var original = new StudentModel
        {
            Id = 1,
            FirstName = "John",
            LastName = "Doe",
            Mail = "john@test.com",
            Phone = "123456789",
            Birthday = new DateTime(2000, 1, 1),
            License = LicenseClass.B,
            EnrollmentDate = DateTime.Now,
            ExamDate = DateTime.Now.AddMonths(3),
            HasPassed = false,
            IsDeleted = false,
            DrivingSchoolId = 5
        };

        var copy = new StudentModel(original);

        Assert.That(copy.Id, Is.EqualTo(original.Id));
        Assert.That(copy.FirstName, Is.EqualTo(original.FirstName));
        Assert.That(copy.LastName, Is.EqualTo(original.LastName));
        Assert.That(copy.Mail, Is.EqualTo(original.Mail));
        Assert.That(copy.License, Is.EqualTo(original.License));
        Assert.That(copy.DrivingSchoolId, Is.EqualTo(original.DrivingSchoolId));
    }

    // ---------------------------------------------------------------------------
    // Default Values Tests
    // ---------------------------------------------------------------------------

    [Test]
    public void StudentModel_DefaultEnrollmentDate_IsNow()
    {
        var student = new StudentModel();
        var now = DateTime.Now;

        Assert.That(student.EnrollmentDate, Is.Not.Null);
        Assert.That(student.EnrollmentDate!.Value.Date, Is.EqualTo(now.Date));
    }

    [Test]
    public void StudentModel_DefaultHasPassed_IsFalse()
    {
        var student = new StudentModel();

        Assert.That(student.HasPassed, Is.False);
    }

    [Test]
    public void StudentModel_DefaultIsDeleted_IsFalse()
    {
        var student = new StudentModel();

        Assert.That(student.IsDeleted, Is.False);
    }
}
