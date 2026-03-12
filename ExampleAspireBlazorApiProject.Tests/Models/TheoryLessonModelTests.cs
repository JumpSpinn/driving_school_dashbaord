namespace ExampleAspireBlazorApiProject.Tests.Models;

[TestFixture]
public sealed class TheoryLessonModelTests
{
    // ---------------------------------------------------------------------------
    // Validation Tests - Name
    // ---------------------------------------------------------------------------

    [Test]
    public void TheoryLessonModel_RequiresName()
    {
        var lesson = new TheoryLessonModel { Name = "", Topic = "Basic Rules" };
        var context = new ValidationContext(lesson);
        var results = new List<ValidationResult>();

        var isValid = Validator.TryValidateObject(lesson, context, results, true);

        Assert.That(isValid, Is.False);
        Assert.That(results.Any(r => r.MemberNames.Contains("Name")), Is.True);
    }

    [Test]
    public void TheoryLessonModel_NameMinLength3()
    {
        var lesson = new TheoryLessonModel { Name = "AB", Topic = "Basic Rules" };
        var context = new ValidationContext(lesson);
        var results = new List<ValidationResult>();

        var isValid = Validator.TryValidateObject(lesson, context, results, true);

        Assert.That(isValid, Is.False);
    }

    [Test]
    public void TheoryLessonModel_NameMaxLength50()
    {
        var lesson = new TheoryLessonModel { Name = new string('A', 51), Topic = "Basic Rules" };
        var context = new ValidationContext(lesson);
        var results = new List<ValidationResult>();

        var isValid = Validator.TryValidateObject(lesson, context, results, true);

        Assert.That(isValid, Is.False);
    }

    // ---------------------------------------------------------------------------
    // Validation Tests - Topic
    // ---------------------------------------------------------------------------

    [Test]
    public void TheoryLessonModel_RequiresTopic()
    {
        var lesson = new TheoryLessonModel { Name = "Traffic Rules", Topic = "" };
        var context = new ValidationContext(lesson);
        var results = new List<ValidationResult>();

        var isValid = Validator.TryValidateObject(lesson, context, results, true);

        Assert.That(isValid, Is.False);
        Assert.That(results.Any(r => r.MemberNames.Contains("Topic")), Is.True);
    }

    [Test]
    public void TheoryLessonModel_TopicMinLength3()
    {
        var lesson = new TheoryLessonModel { Name = "Traffic Rules", Topic = "AB" };
        var context = new ValidationContext(lesson);
        var results = new List<ValidationResult>();

        var isValid = Validator.TryValidateObject(lesson, context, results, true);

        Assert.That(isValid, Is.False);
    }

    [Test]
    public void TheoryLessonModel_TopicMaxLength50()
    {
        var lesson = new TheoryLessonModel { Name = "Traffic Rules", Topic = new string('A', 51) };
        var context = new ValidationContext(lesson);
        var results = new List<ValidationResult>();

        var isValid = Validator.TryValidateObject(lesson, context, results, true);

        Assert.That(isValid, Is.False);
    }

    [Test]
    public void TheoryLessonModel_ValidWhenAllRequiredFieldsProvided()
    {
        var lesson = new TheoryLessonModel { Name = "Traffic Rules", Topic = "Basic Traffic Rules" };
        var context = new ValidationContext(lesson);
        var results = new List<ValidationResult>();

        var isValid = Validator.TryValidateObject(lesson, context, results, true);

        Assert.That(isValid, Is.True);
    }

    // ---------------------------------------------------------------------------
    // Copy Constructor Tests
    // ---------------------------------------------------------------------------

    [Test]
    public void TheoryLessonModel_CopyConstructor_CopiesAllProperties()
    {
        var original = new TheoryLessonModel
        {
            Id = 1,
            Name = "Traffic Rules",
            Topic = "Basic Rules",
            DayOfWeek = DayOfWeek.Monday,
            StartTime = new TimeOnly(10, 0),
            DurationMinutes = 90,
            MaxStudents = 20,
            Price = 50.00m,
            IsBasic = true,
            IsDeleted = false,
            InstructorId = 5
        };

        var copy = new TheoryLessonModel(original);

        Assert.That(copy.Id, Is.EqualTo(original.Id));
        Assert.That(copy.Name, Is.EqualTo(original.Name));
        Assert.That(copy.Topic, Is.EqualTo(original.Topic));
        Assert.That(copy.DayOfWeek, Is.EqualTo(original.DayOfWeek));
        Assert.That(copy.StartTime, Is.EqualTo(original.StartTime));
        Assert.That(copy.DurationMinutes, Is.EqualTo(original.DurationMinutes));
        Assert.That(copy.MaxStudents, Is.EqualTo(original.MaxStudents));
        Assert.That(copy.Price, Is.EqualTo(original.Price));
        Assert.That(copy.IsBasic, Is.EqualTo(original.IsBasic));
        Assert.That(copy.IsDeleted, Is.EqualTo(original.IsDeleted));
        Assert.That(copy.InstructorId, Is.EqualTo(original.InstructorId));
    }

    // ---------------------------------------------------------------------------
    // Default Values Tests
    // ---------------------------------------------------------------------------

    [Test]
    public void TheoryLessonModel_DefaultIsBasic_IsFalse()
    {
        var lesson = new TheoryLessonModel();

        Assert.That(lesson.IsBasic, Is.False);
    }

    [Test]
    public void TheoryLessonModel_DefaultIsDeleted_IsFalse()
    {
        var lesson = new TheoryLessonModel();

        Assert.That(lesson.IsDeleted, Is.False);
    }
}
