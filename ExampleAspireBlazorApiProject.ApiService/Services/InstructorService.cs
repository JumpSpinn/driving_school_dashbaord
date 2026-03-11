namespace ExampleAspireBlazorApiProject.ApiService.Services;

public sealed class InstructorService(ApplicationDbContext dbContext, ILogger<InstructorService> logger) : ControllerBase
{
    public List<InstructorModel> GetInstructors()
    {
        try
        {
            return dbContext.Instructors
                .Include(d => d.TheoryLessons)
                .Where(x => !x.IsDeleted)
                .ToList();
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error getting instructors");
        }

        return [];
    }
    
    private InstructorModel? GetInstructor(int id) 
        => dbContext.Instructors
            .Include(d => d.TheoryLessons)
            .FirstOrDefault(x => x.Id == id && !x.IsDeleted);
    
    public async Task<InstructorModel?> CreateInstructorAsync(InstructorModel instructor)
    {
        try
        {
            var exist = GetInstructor(instructor.Id);
            if (exist is not null) return exist;

            var newInstructor = new InstructorModel
            {
                FirstName = instructor.FirstName,
                LastName = instructor.LastName,
                Mail = instructor.Mail,
                Phone = instructor.Phone
            };
            
            dbContext.Instructors.Add(newInstructor);
            await dbContext.SaveChangesAsync();
            
            await dbContext.Entry(newInstructor).Collection(x => x.TheoryLessons).LoadAsync();
            
            return instructor;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error creating instructor");
        }

        return null;
    }
    
    public async Task<InstructorModel?> UpdateInstructorAsync(InstructorModel toUpdate)
    {
        try
        {
            var instructor = GetInstructor(toUpdate.Id);
            if (instructor is null) return null;
            
            instructor.FirstName = toUpdate.FirstName;
            instructor.LastName = toUpdate.LastName;
            instructor.Mail = toUpdate.Mail;
            instructor.Phone = toUpdate.Phone;
            
            await dbContext.SaveChangesAsync();
            await dbContext.Entry(instructor).Collection(x => x.TheoryLessons).LoadAsync();
            
            return instructor;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error updating instructor");
        }

        return null;
    }
    
    public async Task<bool> DeleteInstructorAsync(int id)
    {
        try
        {
            var instructor = GetInstructor(id);
            if (instructor is null || instructor.IsDeleted) return false;
            
            instructor.IsDeleted = true;
            
            await dbContext.SaveChangesAsync();
            
            return true;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error deleting instructor");
        }

        return false;
    }
}