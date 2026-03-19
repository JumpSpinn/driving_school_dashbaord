namespace DrivingSchoolDashboard.Tests.Components;

[TestFixture]
public sealed class ErrorPageTests : TestContext
{
    [SetUp]
    public void Setup()
    {
        // Register MudBlazor services required for components
        Services.AddMudServices();
    }

    [Test]
    public void ErrorPage_RendersTitle()
    {
        var cut = RenderComponent<Error>();

        var title = cut.Find("h1");
        Assert.That(title.TextContent, Does.Contain("Error"));
    }

    [Test]
    public void ErrorPage_RendersErrorMessage()
    {
        var cut = RenderComponent<Error>();

        var message = cut.Find("h2");
        Assert.That(message.TextContent, Does.Contain("An error occurred while processing your request"));
    }

    [Test]
    public void ErrorPage_DoesNotShowRequestIdByDefault()
    {
        var cut = RenderComponent<Error>();

        // Should not contain Request ID text when no activity/context is set
        var hasRequestId = cut.FindAll("strong").Any(e => e.TextContent.Contains("Request ID"));
        Assert.That(hasRequestId, Is.False);
    }

    [Test]
    public void ErrorPage_ShowsDevelopmentModeWarning()
    {
        var cut = RenderComponent<Error>();

        var developmentHeading = cut.FindAll("h3").FirstOrDefault(h => h.TextContent.Contains("Development Mode"));
        Assert.That(developmentHeading, Is.Not.Null);
    }

    [Test]
    public void ErrorPage_ContainsSecurityWarning()
    {
        var cut = RenderComponent<Error>();

        var warningText = cut.Markup;
        Assert.That(warningText, Does.Contain("shouldn't be enabled for deployed applications"));
    }
}
