namespace DrivingSchoolDashboard.ApiService.Services;

public sealed class DrivingSchoolService(ApplicationDbContext dbContext, ILogger<DrivingSchoolService> l)
    : BaseService<DrivingSchoolService>(l)
{
    #region CREATE

    public async Task<DrivingSchoolModel?> CreateDrivingSchoolAsync(DrivingSchoolModel drivingSchool)
    {
        return await ExecuteAsync(async () =>
        {
            var exist = GetDrivingSchool(drivingSchool.Name);
            if (exist is not null) return exist;

            var newDrivingSchool = new DrivingSchoolModel
            {
                Name = drivingSchool.Name,
                OwnerId = drivingSchool.OwnerId,
            };
            
            dbContext.DrivingSchools.Add(newDrivingSchool);
            await dbContext.SaveChangesAsync();
            
            await dbContext.Entry(newDrivingSchool).Reference(x => x.Owner).LoadAsync();
            
            return newDrivingSchool;
        }, "Error creating driving school");
    }

    #endregion

    #region GET

    private DrivingSchoolModel? GetDrivingSchool(string name) 
        => dbContext.DrivingSchools
            .Include(d => d.Owner)
            .Include(d => d.Students)
            .FirstOrDefault(x => 
                x.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase) && !x.IsDeleted);
    
    private DrivingSchoolModel? GetDrivingSchool(int id) 
        => dbContext.DrivingSchools
            .Include(d => d.Owner)
            .Include(d => d.Students)
            .FirstOrDefault(x => x.Id == id && !x.IsDeleted);

    public List<DrivingSchoolModel> GetAllDrivingSchools()
    {
        var drivingSchools = dbContext.DrivingSchools
            .Include(d => d.Owner)
            .Include(d => d.Students)
            .Where(x => !x.IsDeleted)
            .ToList();
        
        return drivingSchools;
    }

    #endregion

    #region UPDATE

    public async Task<DrivingSchoolModel?> UpdateDrivingSchoolAsync(DrivingSchoolModel toUpdate)
    {
        return await ExecuteAsync(async () =>
        {
            var drivingSchool = GetDrivingSchool(toUpdate.Id);
            if (drivingSchool is null) return null;
            
            drivingSchool.Name = toUpdate.Name;
            drivingSchool.OwnerId = toUpdate.OwnerId;
            
            await dbContext.SaveChangesAsync();
            await dbContext.Entry(drivingSchool).Reference(x => x.Owner).LoadAsync();
            
            return drivingSchool;
        }, "Error updating driving school");
    }

    #endregion

    #region DELETE
    
    public async Task<bool> DeleteDrivingSchoolAsync(int id)
    {
        return await ExecuteBoolAsync(async () =>
        {
            var drivingSchool = GetDrivingSchool(id);
            if (drivingSchool is null || drivingSchool.IsDeleted) return false;
            
            drivingSchool.IsDeleted = true;
            
            foreach (var student in drivingSchool.Students)
                student.DrivingSchoolId = null;
            
            await dbContext.SaveChangesAsync();
            
            return true;
        }, "Error deleting driving school");
    }

    #endregion
}