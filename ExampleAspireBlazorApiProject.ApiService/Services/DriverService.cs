using ExampleAspireBlazorApiProject.Shared.Models;

namespace ExampleAspireBlazorApiProject.ApiService.Services;

public sealed class DriverService(ApplicationDbContext dbContext, ILogger<DriverService> logger)
{
    #region GET

    public List<DriverModel> GetAllDrivers() 
        => dbContext.Drivers.Where(x => !x.IsDeleted).ToList();
    
    public DriverModel? GetDriver(int id) 
        => dbContext.Drivers.FirstOrDefault(x => x.Id == id && !x.IsDeleted);

    #endregion

    #region CREATE

    public async Task<DriverModel?> CreateDriverAsync(DriverModel newDriver)
    {
        try
        {
            dbContext.Drivers.Add(newDriver);
            await dbContext.SaveChangesAsync();
            return newDriver;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error creating driver");
        }

        return null;
    }

    #endregion

    #region DELETE

    public async Task<bool> DeleteDriverAsync(int id)
    {
        try
        {
            var driver = GetDriver(id);
            if (driver is null || driver.IsDeleted) return false;

            dbContext.Drivers.Remove(driver);
            await dbContext.SaveChangesAsync();

            return true;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error deleting driver");
        }

        return false;
    }

    #endregion

    #region UPDATE

    public async Task<bool> UpdateDriverAsync(DriverModel editDriver)
    {
        try
        {
            var driver = GetDriver(editDriver.Id);
            if (driver is null) return false;

            driver.FirstName = editDriver.FirstName;
            driver.LastName = editDriver.LastName;
            driver.Mail = editDriver.Mail;
            driver.Phone = editDriver.Phone;
            driver.DrivingSchoolId = editDriver.DrivingSchoolId;
            driver.IsActive = driver.IsActive;
            
            await dbContext.SaveChangesAsync();

            return true;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error updating driver #1");
        }

        return false;
    }
    
    public async Task<bool> UpdateDriverAsync(int id, bool isActive)
    {
        try
        {
            var driver = GetDriver(id);
            if (driver is null) return false;
            
            driver.IsActive = isActive;
            
            await dbContext.SaveChangesAsync();

            return true;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error updating driver #2");
        }

        return false;
    }

    #endregion
}