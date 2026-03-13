namespace ExampleAspireBlazorApiProject.ApiService.Services;

public abstract class BaseService<TService>(ILogger<TService> logger)
{
    [ExcludeFromCodeCoverage]
    protected T? HandleError<T>(Exception ex, string message) where T : class
    {
        logger.LogError(ex, message);
        return null;
    }

    [ExcludeFromCodeCoverage]
    protected bool HandleErrorBool(Exception ex, string message)
    {
        logger.LogError(ex, message);
        return false;
    }
}