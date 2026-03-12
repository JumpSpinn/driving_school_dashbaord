using System.Diagnostics.CodeAnalysis;

namespace ExampleAspireBlazorApiProject.ApiService.Database;

[ExcludeFromCodeCoverage]
public static class DataSeeder
{
    public static async Task SeedDataAsync(ApplicationDbContext dbContext)
    {
        if (dbContext.Instructors.Any()) return;

        // Instructors
        var instructor1 = new InstructorModel { FirstName = "Max", LastName = "Mustermann", Mail = "max@fahrschule.de" };
        var instructor2 = new InstructorModel { FirstName = "Kevin", LastName = "Hille", Mail = "kevin@fahrschule.de" };
        var instructor3 = new InstructorModel { FirstName = "Sandra", LastName = "Bauer", Mail = "sandra@fahrschule.de" };
        var instructor4 = new InstructorModel { FirstName = "Thomas", LastName = "Richter", Mail = "thomas@fahrschule.de" };

        dbContext.Instructors.AddRange(instructor1, instructor2, instructor3, instructor4);

        // Driving Schools
        var school1 = new DrivingSchoolModel { Name = "Berry Bit Studios", Owner = instructor2 };
        var school2 = new DrivingSchoolModel { Name = "Fahrschule Sonnenschein", Owner = instructor1 };
        var school3 = new DrivingSchoolModel { Name = "Drive & Learn", Owner = instructor3 };

        dbContext.DrivingSchools.AddRange(school1, school2, school3);

        // Students
        var students = new List<StudentModel>
        {
            new() { FirstName = "Lisa",     LastName = "Schmidt",   Mail = "lisa@schmidt.de",     Phone = "0123456789", License = LicenseClass.A, HasPassed = false, Birthday = DateTimeHelper.RandomBirthday()},
            new() { FirstName = "Tom",      LastName = "Müller",    Mail = "tom@mueller.de",      Phone = "0176123456", License = LicenseClass.B, HasPassed = false, Birthday = DateTimeHelper.RandomBirthday()},
            new() { FirstName = "Anna",     LastName = "Fischer",   Mail = "anna@fischer.de",     Phone = "0151987654", License = LicenseClass.B, HasPassed = false, Birthday = DateTimeHelper.RandomBirthday()},
            new() { FirstName = "Jonas",    LastName = "Weber",     Mail = "jonas@weber.de",      Phone = "0162345678", License = LicenseClass.C, HasPassed = true, Birthday = DateTimeHelper.RandomBirthday()},
            new() { FirstName = "Marie",    LastName = "Wagner",    Mail = "marie@wagner.de",     Phone = "0172456789", License = LicenseClass.A, HasPassed = true, Birthday = DateTimeHelper.RandomBirthday()},
            new() { FirstName = "Felix",    LastName = "Becker",    Mail = "felix@becker.de",     Phone = "0163567890", License = LicenseClass.B, HasPassed = true, Birthday = DateTimeHelper.RandomBirthday()},
            new() { FirstName = "Sophie",   LastName = "Hoffmann",  Mail = "sophie@hoffmann.de",  Phone = "0174678901", License = LicenseClass.B, HasPassed = false, Birthday = DateTimeHelper.RandomBirthday()},
            new() { FirstName = "Lukas",    LastName = "Schäfer",   Mail = "lukas@schaefer.de",   Phone = "0155789012", License = LicenseClass.A, HasPassed = true, Birthday = DateTimeHelper.RandomBirthday()},
            new() { FirstName = "Emma",     LastName = "Koch",      Mail = "emma@koch.de",        Phone = "0166890123", License = LicenseClass.C, HasPassed = true, Birthday = DateTimeHelper.RandomBirthday()},
            new() { FirstName = "Noah",     LastName = "Bauer",     Mail = "noah@bauer.de",       Phone = "0177901234", License = LicenseClass.B, HasPassed = true, Birthday = DateTimeHelper.RandomBirthday()},
            new() { FirstName = "Mia",      LastName = "Richter",   Mail = "mia@richter.de",      Phone = "0158012345", License = LicenseClass.B, HasPassed = false, Birthday = DateTimeHelper.RandomBirthday()},
            new() { FirstName = "Leon",     LastName = "Klein",     Mail = "leon@klein.de",       Phone = "0169123456", License = LicenseClass.A, HasPassed = true, Birthday = DateTimeHelper.RandomBirthday()},
        };

        dbContext.Students.AddRange(students);
        
        // Assign driving schools to students
        var studentsWithDrivingSchool = students.Take(5);
        foreach (var student in studentsWithDrivingSchool)
        {
            List<DrivingSchoolModel> schools = [school1, school2, school3];
            var rndIdx = new Random().Next(0, 3);
            var randomSchool = schools[rndIdx];
            student.DrivingSchool =randomSchool;
        }

        // Theory Lessons
        var lessons = new List<TheoryLessonModel>
        {
            new() { Name = "Grundstoff 1",  Topic = "Vorfahrt & Verkehrszeichen",      DayOfWeek = DayOfWeek.Monday,    StartTime = new TimeOnly(18, 30), Instructor = instructor1, Price = 45.00m, DurationMinutes = 90,  MaxStudents = 12},
            new() { Name = "Grundstoff 2",  Topic = "Geschwindigkeit & Abstand",       DayOfWeek = DayOfWeek.Tuesday,   StartTime = new TimeOnly(19, 00), Instructor = instructor2, Price = 45.00m, DurationMinutes = 75,  MaxStudents = 8 },
            new() { Name = "Grundstoff 3",  Topic = "Überholen & Fahrstreifenwechsel", DayOfWeek = DayOfWeek.Wednesday, StartTime = new TimeOnly(18, 00), Instructor = instructor1, Price = 45.00m, DurationMinutes = 120, MaxStudents = 15},
            new() { Name = "Grundstoff 4",  Topic = "Beleuchtung & Sicht",             DayOfWeek = DayOfWeek.Thursday,  StartTime = new TimeOnly(17, 30), Instructor = instructor3, Price = 45.00m, DurationMinutes = 60,  MaxStudents = 10},
            new() { Name = "Grundstoff 5",  Topic = "Umwelt & Energie",                DayOfWeek = DayOfWeek.Friday,    StartTime = new TimeOnly(18, 30), Instructor = instructor4, Price = 40.00m, DurationMinutes = 90,  MaxStudents = 6 },
            new() { Name = "Zusatzstoff A", Topic = "Motorrad & Zweiräder",            DayOfWeek = DayOfWeek.Saturday,  StartTime = new TimeOnly(10, 00), Instructor = instructor2, Price = 50.00m, DurationMinutes = 105, MaxStudents = 20},
            new() { Name = "Zusatzstoff B", Topic = "Autobahn & Schnellstraßen",       DayOfWeek = DayOfWeek.Monday,    StartTime = new TimeOnly(20, 00), Instructor = instructor3, Price = 50.00m, DurationMinutes = 75,  MaxStudents = 9 },
            new() { Name = "Zusatzstoff C", Topic = "Gefahrgut & Schwertransport",     DayOfWeek = DayOfWeek.Wednesday, StartTime = new TimeOnly(19, 30), Instructor = instructor4, Price = 55.00m, DurationMinutes = 120, MaxStudents = 5 },
        };

        dbContext.TheoryLessons.AddRange(lessons);

        // Save so IDs are generated
        await dbContext.SaveChangesAsync();

        // Course Bookings - spread across different days
        var bookings = new List<CourseBookingModel>
        {
            new() { StudentId = students[0].Id,  TheoryLessonId = lessons[0].Id, BookingDate = DateTime.Now.AddDays(-30) },
            new() { StudentId = students[1].Id,  TheoryLessonId = lessons[1].Id, BookingDate = DateTime.Now.AddDays(-28) },
            new() { StudentId = students[2].Id,  TheoryLessonId = lessons[2].Id, BookingDate = DateTime.Now.AddDays(-25) },
            new() { StudentId = students[3].Id,  TheoryLessonId = lessons[3].Id, BookingDate = DateTime.Now.AddDays(-22) },
            new() { StudentId = students[4].Id,  TheoryLessonId = lessons[4].Id, BookingDate = DateTime.Now.AddDays(-20) },
            new() { StudentId = students[5].Id,  TheoryLessonId = lessons[5].Id, BookingDate = DateTime.Now.AddDays(-18) },
            new() { StudentId = students[6].Id,  TheoryLessonId = lessons[6].Id, BookingDate = DateTime.Now.AddDays(-15) },
            new() { StudentId = students[7].Id,  TheoryLessonId = lessons[7].Id, BookingDate = DateTime.Now.AddDays(-12) },
            new() { StudentId = students[8].Id,  TheoryLessonId = lessons[0].Id, BookingDate = DateTime.Now.AddDays(-10) },
            new() { StudentId = students[9].Id,  TheoryLessonId = lessons[1].Id, BookingDate = DateTime.Now.AddDays(-7)  },
            new() { StudentId = students[10].Id, TheoryLessonId = lessons[2].Id, BookingDate = DateTime.Now.AddDays(-5)  },
            new() { StudentId = students[11].Id, TheoryLessonId = lessons[3].Id, BookingDate = DateTime.Now.AddDays(-3)  },
            new() { StudentId = students[0].Id,  TheoryLessonId = lessons[4].Id, BookingDate = DateTime.Now.AddDays(-2)  },
            new() { StudentId = students[1].Id,  TheoryLessonId = lessons[5].Id, BookingDate = DateTime.Now.AddDays(-1)  },
            new() { StudentId = students[2].Id,  TheoryLessonId = lessons[6].Id, BookingDate = DateTime.Now               },
        };

        dbContext.CourseBookings.AddRange(bookings);

        await dbContext.SaveChangesAsync();
    }
}