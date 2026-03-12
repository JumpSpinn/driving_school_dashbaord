namespace ExampleAspireBlazorApiProject.Shared.Helpers;

public static class DateTimeHelper
{
    public static DateTime RandomBirthday()
    {
        var rnd = new Random();
        var start = new DateTime(1989, 1, 1);
        var end   = new DateTime(2008, 12, 31);
        var range = (end - start).Days;
        return start.AddDays(rnd.Next(range));
    }
    
    public static string ConvertToIsoString(this DateTime dateTime) => dateTime.ToString("dd.MM.yyyy");
}