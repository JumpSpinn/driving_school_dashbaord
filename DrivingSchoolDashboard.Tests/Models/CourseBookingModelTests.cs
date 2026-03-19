namespace DrivingSchoolDashboard.Tests.Models;

[TestFixture]
public sealed class CourseBookingModelTests
{
    // ---------------------------------------------------------------------------
    // Validation Tests
    // ---------------------------------------------------------------------------

    [Test]
    public void CourseBookingModel_RequiresStudentId()
    {
        var booking = new CourseBookingModel { TheoryLessonId = 1 };
        var context = new ValidationContext(booking);
        var results = new List<ValidationResult>();

        var isValid = Validator.TryValidateObject(booking, context, results, true);

        Assert.That(isValid, Is.False);
        Assert.That(results.Any(r => r.MemberNames.Contains("StudentId")), Is.True);
    }

    [Test]
    public void CourseBookingModel_RequiresTheoryLessonId()
    {
        var booking = new CourseBookingModel { StudentId = 1 };
        var context = new ValidationContext(booking);
        var results = new List<ValidationResult>();

        var isValid = Validator.TryValidateObject(booking, context, results, true);

        Assert.That(isValid, Is.False);
        Assert.That(results.Any(r => r.MemberNames.Contains("TheoryLessonId")), Is.True);
    }

    [Test]
    public void CourseBookingModel_ValidWhenAllRequiredFieldsProvided()
    {
        var booking = new CourseBookingModel { StudentId = 1, TheoryLessonId = 1 };
        var context = new ValidationContext(booking);
        var results = new List<ValidationResult>();

        var isValid = Validator.TryValidateObject(booking, context, results, true);

        Assert.That(isValid, Is.True);
    }

    // ---------------------------------------------------------------------------
    // Copy Constructor Tests
    // ---------------------------------------------------------------------------

    [Test]
    public void CourseBookingModel_CopyConstructor_CopiesAllProperties()
    {
        var original = new CourseBookingModel
        {
            Id = 1,
            StudentId = 10,
            TheoryLessonId = 20,
            BookingDate = new DateTime(2025, 1, 1)
        };

        var copy = new CourseBookingModel(original);

        Assert.That(copy.Id, Is.EqualTo(original.Id));
        Assert.That(copy.StudentId, Is.EqualTo(original.StudentId));
        Assert.That(copy.TheoryLessonId, Is.EqualTo(original.TheoryLessonId));
        Assert.That(copy.BookingDate, Is.EqualTo(original.BookingDate));
    }

    // ---------------------------------------------------------------------------
    // Default Values Tests
    // ---------------------------------------------------------------------------

    [Test]
    public void CourseBookingModel_DefaultBookingDate_IsNow()
    {
        var booking = new CourseBookingModel();
        var now = DateTime.Now;

        Assert.That(booking.BookingDate, Is.Not.Null);
        Assert.That(booking.BookingDate!.Value.Date, Is.EqualTo(now.Date));
    }
}
