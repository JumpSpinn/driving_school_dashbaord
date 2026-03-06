namespace ExampleAspireBlazorApiProject.ApiService.Services;

public sealed class DrivingSchoolService(ApplicationDbContext dbContext, ILogger<DrivingSchoolService> logger)
{
    #region CREATE

    public async Task<DrivingSchoolModel?> CreateDrivingSchoolAsync(string name)
    {
        try
        {
            var exist = GetDrivingSchool(name);
            if (exist is not null) return exist;

            var drivingSchool = new DrivingSchoolModel
            {
                Name = name
            };
            
            dbContext.DrivingSchools.Add(drivingSchool);
            await dbContext.SaveChangesAsync();
            
            return drivingSchool;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return null;
    }

    #endregion

    #region GET
    
    public DrivingSchoolModel? GetDrivingSchool(string name)
    {
        try
        {
            var drivingSchool = dbContext.DrivingSchools
                .Include(d => d.Drivers)
                .FirstOrDefault(x => 
                    x.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase) && !x.IsDeleted);
            
            if(drivingSchool is null || drivingSchool.IsDeleted) return null;

            return drivingSchool;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error getting driving school");
        }

        return null;
    }
    
    public DrivingSchoolModel? GetDrivingSchool(int id) 
        => dbContext.DrivingSchools
            .Include(d => d.Drivers)
            .FirstOrDefault(x => x.Id == id && !x.IsDeleted);

    public List<DrivingSchoolModel> GetAllDrivingSchools()
    {
        var drivingSchools = dbContext.DrivingSchools
            .Include(d => d.Drivers)
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
            if (Equals(drivingSchool.Name, toUpdate.Name)) return drivingSchool;
            
            drivingSchool.Name = toUpdate.Name;
            await dbContext.SaveChangesAsync();
            
            return drivingSchool;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error updating driving school");
        }

        return null;
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
            await dbContext.SaveChangesAsync();
            
            return true;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error deleting driving school");
        }

        return false;
    }

    public async Task<bool> DeleteDrivingSchoolAsync(string name)
    {
        try
        {
            var drivingSchool = GetDrivingSchool(name);
            if (drivingSchool is null || drivingSchool.IsDeleted) return false;

            drivingSchool.IsDeleted = true;
            await dbContext.SaveChangesAsync();
            
            return true;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error deleting driving school");
        }

        return false;
    }

    #endregion
}