namespace DrivingSchoolDashboard.Web.Interfaces;

public interface ITheoryLessonApiClient
{
    public Task<List<TheoryLessonModel>?> GetAllLessonsAsync();
    public Task<TheoryLessonModel?> CreateLessonAsync(TheoryLessonModel newLesson);
    public Task<bool> DeleteLessonAsync(int id);
    public Task<TheoryLessonModel?> UpdateLessonAsync(TheoryLessonModel editLesson);
}