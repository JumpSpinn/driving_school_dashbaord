namespace ExampleAspireBlazorApiProject.ApiService.Services;

public sealed class InstructorService(ApplicationDbContext dbContext, ILogger<InstructorService> l)
    : BaseService<InstructorService>(l)
{
    public List<InstructorModel> GetInstructors() 
        => dbContext.Instructors
            .Include(d => d.TheoryLessons)
            .Where(x => !x.IsDeleted)
            .ToList();
    
    private InstructorModel? GetInstructor(int id) 
        => dbContext.Instructors
            .Include(d => d.TheoryLessons)
            .FirstOrDefault(x => x.Id == id && !x.IsDeleted);
    
    public async Task<InstructorModel?> CreateInstructorAsync(InstructorModel instructor)
    {
        return await ExecuteAsync(async () =>
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
            
            return newInstructor;
        }, "Error creating instructor");
    }
    
    public async Task<InstructorModel?> UpdateInstructorAsync(InstructorModel toUpdate)
    {
        return await ExecuteAsync(async () =>
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
        }, "Error updating instructor");
    }
    
    public async Task<bool> DeleteInstructorAsync(int id)
    {
        return await ExecuteBoolAsync(async () =>
        {
            var instructor = GetInstructor(id);
            if (instructor is null || instructor.IsDeleted) return false;
            
            instructor.IsDeleted = true;
            
            await dbContext.SaveChangesAsync();
            
            return true;
        }, "Error deleting instructor");
    }
}