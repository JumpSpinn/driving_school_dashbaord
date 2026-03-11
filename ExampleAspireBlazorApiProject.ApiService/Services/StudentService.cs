namespace ExampleAspireBlazorApiProject.ApiService.Services;

public sealed class StudentService(ApplicationDbContext dbContext, ILogger<StudentService> logger)
{
    #region GET

    public List<StudentModel> GetAllDrivers() 
        => dbContext.Students.Where(x => !x.IsDeleted).ToList();

    private StudentModel? GetStudent(int id) 
        => dbContext.Students.FirstOrDefault(x => x.Id == id && !x.IsDeleted);

    #endregion

    #region CREATE

    public async Task<StudentModel?> CreateStudentAsync(StudentModel newStudent)
    {
        try
        {
            dbContext.Students.Add(newStudent);
            await dbContext.SaveChangesAsync();
            await dbContext.Entry(newStudent).Reference(x => x.DrivingSchool).LoadAsync();
            return newStudent;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error creating student");
        }

        return null;
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
            logger.LogError(e, "Error deleting student");
        }

        return false;
    }

    #endregion

    #region UPDATE

    public async Task<bool> UpdateStudentAsync(StudentModel editStudent)
    {
        try
        {
            var student = GetStudent(editStudent.Id);
            if (student is null) return false;

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

            return true;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error updating student #1");
        }

        return false;
    }

    #endregion
}