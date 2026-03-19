namespace DrivingSchoolDashboard.Web.Data;

public static class DialogData
{
    public static readonly DialogOptions DialogDefaultOptions = new()
    {
        MaxWidth = MaxWidth.Small,
        FullWidth = true,
        BackdropClick = false,
        CloseOnEscapeKey = true,
        BackgroundClass = "dialog_backdrop_blur"
    };
    
    public static readonly DialogOptions DialogInfoOptions = new()
    {
        MaxWidth = MaxWidth.Small,
        FullWidth = true,
        BackdropClick = true,
        CloseOnEscapeKey = true,
        BackgroundClass = "dialog_backdrop_blur"
    };
}