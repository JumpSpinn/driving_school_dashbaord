namespace ExampleAspireBlazorApiProject.ApiService.Services;

public sealed class TheoryLessonService(ApplicationDbContext dbContext, ILogger<TheoryLessonService> l)
    : BaseService<TheoryLessonService>(l)
{
    public List<TheoryLessonModel> GetTheoryLessons()
    {
        try
        {
            return dbContext.TheoryLessons
                .Include(d => d.Instructor)
                .Where(x => !x.IsDeleted)
                .ToList();
        }
        catch (Exception e)
        {
            return HandleError<List<TheoryLessonModel>>(e, "Error getting lessons")!;
        }
    }
    
    private TheoryLessonModel? GetTheoryLesson(int id) 
        => dbContext.TheoryLessons
            .Include(d => d.Instructor)
            .FirstOrDefault(x => x.Id == id && !x.IsDeleted);
    
    public async Task<TheoryLessonModel?> CreateTheoryLessonAsync(TheoryLessonModel lesson)
    {
        try
        {
            var exist = GetTheoryLesson(lesson.Id);
            if (exist is not null) return exist;

            var newLesson = new TheoryLessonModel
            {
                Name = lesson.Name,
                Topic = lesson.Topic,
                DayOfWeek = lesson.DayOfWeek,
                StartTime = lesson.StartTime,
                DurationMinutes = lesson.DurationMinutes,
                MaxStudents = lesson.MaxStudents,
                Price = lesson.Price,
                IsBasic = lesson.IsBasic,
                InstructorId = lesson.InstructorId
            };
            
            dbContext.TheoryLessons.Add(newLesson);
            await dbContext.SaveChangesAsync();
            
            await dbContext.Entry(newLesson).Reference(x => x.Instructor).LoadAsync();
            
            return newLesson;
        }
        catch (Exception e)
        {
            return HandleError<TheoryLessonModel>(e, "Error creating lesson");
        }
    }
    
    public async Task<TheoryLessonModel?> UpdateTheoryLessonAsync(TheoryLessonModel toUpdate)
    {
        try
        {
            var lesson = GetTheoryLesson(toUpdate.Id);
            if (lesson is null) return null;
            
            lesson.Name = toUpdate.Name;
            lesson.Topic = toUpdate.Topic;
            lesson.DayOfWeek = toUpdate.DayOfWeek;
            lesson.StartTime = toUpdate.StartTime;
            lesson.DurationMinutes = toUpdate.DurationMinutes;
            lesson.MaxStudents = toUpdate.MaxStudents;
            lesson.Price = toUpdate.Price;
            lesson.IsBasic = toUpdate.IsBasic;
            lesson.InstructorId = toUpdate.InstructorId;
            
            await dbContext.SaveChangesAsync();
            await dbContext.Entry(lesson).Reference(x => x.Instructor).LoadAsync();
            
            return lesson;
        }
        catch (Exception e)
        {
            return HandleError<TheoryLessonModel>(e, "Error updating lesson");
        }
    }
    
    public async Task<bool> DeleteTheoryLessonAsync(int id)
    {
        try
        {
            var lesson = GetTheoryLesson(id);
            if (lesson is null || lesson.IsDeleted) return false;
            
            if(lesson.Instructor is not null)
                lesson.Instructor.TheoryLessons.Remove(lesson);
            
            lesson.IsDeleted = true;
            
            await dbContext.SaveChangesAsync();
            
            return true;
        }
        catch (Exception e)
        {
            return HandleErrorBool(e, "Error deleting lesson");
        }
    }
}