namespace ExampleAspireBlazorApiProject.ApiService.Services;

public sealed class StudentService(ApplicationDbContext dbContext, ILogger<StudentService> l)
    : BaseService<StudentService>(l)
{
    #region GET

    public List<StudentModel> GetAllDrivers() 
        => dbContext.Students
            .Where(x => !x.IsDeleted)
            .Include(d => d.DrivingSchool)
            .Include(d => d.CourseBookings)
            .ToList();

    private StudentModel? GetStudent(int id) 
        => dbContext.Students
            .Include(d => d.DrivingSchool)
            .Include(d => d.CourseBookings)
            .FirstOrDefault(x => x.Id == id && !x.IsDeleted);

    #endregion

    #region CREATE

    public async Task<StudentModel?> CreateStudentAsync(StudentModel newStudent)
    {
        try
        {
            dbContext.Students.Add(newStudent);
            await dbContext.SaveChangesAsync();
            await dbContext.Entry(newStudent).Reference(x => x.DrivingSchool).LoadAsync();
            await dbContext.Entry(newStudent).Collection(x => x.CourseBookings).LoadAsync();
            return newStudent;
        }
        catch (Exception e)
        {
            return HandleError<StudentModel>(e, "Error creating student");
        }
    }

    #endregion

    #region DELETE

    public async Task<bool> DeleteStudentAsync(int id)
    {
        try
        {
            var student = GetStudent(id);
            if (student is null || student.IsDeleted) return false;

            dbContext.Students.Remove(student);
            await dbContext.SaveChangesAsync();

            return true;
        }
        catch (Exception e)
        {
            return HandleErrorBool(e, "Error deleting student");
        }
    }

    #endregion

    #region UPDATE

    public async Task<StudentModel?> UpdateStudentAsync(StudentModel editStudent)
    {
        try
        {
            var student = GetStudent(editStudent.Id);
            if (student is null) return null;

            student.FirstName = editStudent.FirstName;
            student.LastName = editStudent.LastName;
            student.Mail = editStudent.Mail;
            student.Phone = editStudent.Phone;
            student.Birthday = editStudent.Birthday;
            student.License = editStudent.License;
            student.EnrollmentDate = editStudent.EnrollmentDate;
            student.ExamDate = editStudent.ExamDate;
            student.HasPassed = editStudent.HasPassed;
            student.DrivingSchoolId = editStudent.DrivingSchoolId;
            
            await dbContext.SaveChangesAsync();
            await dbContext.Entry(student).Reference(x => x.DrivingSchool).LoadAsync();
            await dbContext.Entry(student).Collection(x => x.CourseBookings).LoadAsync();

            return student;
        }
        catch (Exception e)
        {
            return HandleError<StudentModel>(e, "Error updating student");
        }
    }

    #endregion
}