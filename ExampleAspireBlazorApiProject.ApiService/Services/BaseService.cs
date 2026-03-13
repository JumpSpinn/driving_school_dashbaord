namespace ExampleAspireBlazorApiProject.ApiService.Services;

public abstract class BaseService<TService>(ILogger<TService> logger)
{
    [ExcludeFromCodeCoverage]
    protected async Task<T?> ExecuteAsync<T>(Func<Task<T>> action, string errorMessage) where T : class?
    {
        try
        {
            return await action();
        }
        catch (Exception ex)
        {
            return HandleError<T>(ex, errorMessage);
        }
    }

    [ExcludeFromCodeCoverage]
    protected async Task<bool> ExecuteBoolAsync(Func<Task<bool>> action, string errorMessage)
    {
        try
        {
            return await action();
        }
        catch (Exception ex)
        {
            return HandleErrorBool(ex, errorMessage);
        }
    }
    
    [ExcludeFromCodeCoverage]
    private T? HandleError<T>(Exception ex, string message) where T : class
    {
        logger.LogError(ex, message);
        return null;
    }

    [ExcludeFromCodeCoverage]
    private bool HandleErrorBool(Exception ex, string message)
    {
        logger.LogError(ex, message);
        return false;
    }
}