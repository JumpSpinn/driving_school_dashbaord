namespace DrivingSchoolDashboard.Tests.Helpers;

[TestFixture]
public sealed class DateTimeHelperTests
{
    // ---------------------------------------------------------------------------
    // RandomBirthday
    // ---------------------------------------------------------------------------

    [Test]
    public void RandomBirthday_ReturnsDateWithinExpectedRange()
    {
        var start = new DateTime(1989, 1, 1);
        var end = new DateTime(2008, 12, 31);

        var result = DateTimeHelper.RandomBirthday();

        Assert.That(result, Is.GreaterThanOrEqualTo(start));
        Assert.That(result, Is.LessThanOrEqualTo(end));
    }

    [Test]
    public void RandomBirthday_GeneratesDifferentDates()
    {
        var dates = new HashSet<DateTime>();

        for (int i = 0; i < 100; i++)
        {
            dates.Add(DateTimeHelper.RandomBirthday());
        }

        // Should generate at least some variety in 100 calls
        Assert.That(dates.Count, Is.GreaterThan(10));
    }

    // ---------------------------------------------------------------------------
    // ConvertToIsoString
    // ---------------------------------------------------------------------------

    [Test]
    public void ConvertToIsoString_FormatsDateCorrectly()
    {
        var date = new DateTime(2025, 3, 15);

        var result = date.ConvertToIsoString();

        Assert.That(result, Is.EqualTo("15.03.2025"));
    }

    [Test]
    public void ConvertToIsoString_HandlesSingleDigitDayAndMonth()
    {
        var date = new DateTime(2025, 1, 5);

        var result = date.ConvertToIsoString();

        Assert.That(result, Is.EqualTo("05.01.2025"));
    }

    [Test]
    public void ConvertToIsoString_HandlesEndOfYear()
    {
        var date = new DateTime(2025, 12, 31);

        var result = date.ConvertToIsoString();

        Assert.That(result, Is.EqualTo("31.12.2025"));
    }
}
