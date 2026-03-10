namespace ExampleAspireBlazorApiProject.Web.Data;

public static class DialogData
{
    public static readonly DialogOptions DialogDefaultOptions = new()
    {
        MaxWidth = MaxWidth.Small,
        FullWidth = true,
        BackdropClick = true,
        CloseOnEscapeKey = true,
        BackgroundClass = "dialog_backdrop_blur"
    };
}