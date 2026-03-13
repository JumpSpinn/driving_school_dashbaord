namespace ExampleAspireBlazorApiProject.ApiService.Services;

public sealed class DrivingSchoolService(ApplicationDbContext dbContext, ILogger<DrivingSchoolService> l)
    : BaseService<DrivingSchoolService>(l)
{
    #region CREATE

    public async Task<DrivingSchoolModel?> CreateDrivingSchoolAsync(DrivingSchoolModel drivingSchool)
    {
        try
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
        }
        catch (Exception e)
        {
            return HandleError<DrivingSchoolModel>(e, "Error creating driving school");
        }
    }

    #endregion

    #region GET

    private DrivingSchoolModel? GetDrivingSchool(string name)
    {
        try
        {
            var drivingSchool = dbContext.DrivingSchools
                .Include(d => d.Owner)
                .Include(d => d.Students)
                .FirstOrDefault(x => 
                    x.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase) && !x.IsDeleted);
            
            if(drivingSchool is null || drivingSchool.IsDeleted) return null;

            return drivingSchool;
        }
        catch (Exception e)
        {
            return HandleError<DrivingSchoolModel>(e, "Error getting driving school");
        }
    }

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
        try
        {
            var drivingSchool = GetDrivingSchool(toUpdate.Id);
            if (drivingSchool is null) return null;
            
            drivingSchool.Name = toUpdate.Name;
            drivingSchool.OwnerId = toUpdate.OwnerId;
            
            await dbContext.SaveChangesAsync();
            await dbContext.Entry(drivingSchool).Reference(x => x.Owner).LoadAsync();
            
            return drivingSchool;
        }
        catch (Exception e)
        {
            return HandleError<DrivingSchoolModel>(e, "Error updating driving school");
        }
    }

    #endregion

    #region DELETE
    
    public async Task<bool> DeleteDrivingSchoolAsync(int id)
    {
        try
        {
            var drivingSchool = GetDrivingSchool(id);
            if (drivingSchool is null || drivingSchool.IsDeleted) return false;
            
            drivingSchool.IsDeleted = true;
            
            foreach (var student in drivingSchool.Students)
                student.DrivingSchoolId = null;
            
            await dbContext.SaveChangesAsync();
            
            return true;
        }
        catch (Exception e)
        {
            return HandleErrorBool(e, "Error deleting driving school");
        }
    }

    #endregion
}