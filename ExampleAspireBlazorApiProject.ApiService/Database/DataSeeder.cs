using ExampleAspireBlazorApiProject.Shared.Enums;

namespace ExampleAspireBlazorApiProject.ApiService.Database;

public static class DataSeeder
{
    public static async Task SeedDataAsync(ApplicationDbContext dbContext)
    {
        if (dbContext.Instructors.Any()) return;
        
        var instructor = new InstructorModel { 
            FirstName = "Max", 
            LastName = "Mustermann", 
            Mail = "max@fahrschule.de" 
        };
        
        var instructor2 = new InstructorModel { 
            FirstName = "Kevin", 
            LastName = "Hille", 
            Mail = "kevin@fahrschule.de" 
        };
        
        dbContext.Instructors.Add(instructor);
        dbContext.Instructors.Add(instructor2);
        
        var school = new DrivingSchoolModel { 
            Name = "Berry Bit Studios", 
            Owner = instructor2 
        };
        dbContext.DrivingSchools.Add(school);

        var student = new StudentModel()
        {
            FirstName = "Lisa",
            LastName = "Schmidt",
            Mail = "lisa@schmidt.de",
            Phone = "0123456789",
            License = LicenseClass.A
        };
        dbContext.Students.Add(student);
        
        var lesson = new TheoryLessonModel {
            Name = "Grundstoff 1",
            Topic = "Vorfahrt & Verkehrszeichen",
            DayOfWeek = DayOfWeek.Monday,
            StartTime = new TimeOnly(18, 30),
            Instructor = instructor,
            Price = 45.00m
        };
        dbContext.TheoryLessons.Add(lesson);
        
        await dbContext.SaveChangesAsync();
    }
}