namespace ExampleAspireBlazorApiProject.Tests.Components;

[TestFixture]
public sealed class MainLayoutTests
{
    private TestContext _testContext = null!;

    [SetUp]
    public void Setup()
    {
        _testContext = new TestContext();

        _testContext.Services.AddMudServices();
        _testContext.JSInterop.Mode = JSRuntimeMode.Loose;
        
        _testContext.ComponentFactories.AddStub<MudPopover>();
    }

    [TearDown]
    public void TearDown()
    {
        _testContext?.Dispose();
    }

    [Test]
    public void MainLayout_Renders()
    {
        var cut = _testContext.RenderComponent<MainLayout>();

        Assert.That(cut, Is.Not.Null);
    }

    [Test]
    public void MainLayout_ContainsMudLayout()
    {
        var cut = _testContext.RenderComponent<MainLayout>();

        var mudLayout = cut.FindComponent<MudLayout>();
        Assert.That(mudLayout, Is.Not.Null);
    }

    [Test]
    public void MainLayout_ContainsAppBar()
    {
        var cut = _testContext.RenderComponent<MainLayout>();

        var appBar = cut.FindComponent<MudAppBar>();
        Assert.That(appBar, Is.Not.Null);
    }

    [Test]
    public void MainLayout_AppBarContainsTitle()
    {
        var cut = _testContext.RenderComponent<MainLayout>();

        var markup = cut.Markup;
        Assert.That(markup, Does.Contain("Fahrschul Manager"));
    }

    [Test]
    public void MainLayout_ContainsDrawer()
    {
        var cut = _testContext.RenderComponent<MainLayout>();

        var drawer = cut.FindComponent<MudDrawer>();
        Assert.That(drawer, Is.Not.Null);
    }

    [Test]
    public void MainLayout_HasDrawerToggleButton()
    {
        var cut = _testContext.RenderComponent<MainLayout>();

        // Verify the menu button exists that toggles the drawer
        var iconButtons = cut.FindComponents<MudIconButton>();
        var menuButton = iconButtons.FirstOrDefault(b => b.Instance.Icon == Icons.Material.Filled.Menu);
        Assert.That(menuButton, Is.Not.Null);
    }

    [Test]
    public void MainLayout_ContainsMenuButton()
    {
        var cut = _testContext.RenderComponent<MainLayout>();

        var iconButtons = cut.FindComponents<MudIconButton>();
        Assert.That(iconButtons.Any(b => b.Instance.Icon == Icons.Material.Filled.Menu), Is.True);
    }

    [Test]
    public void MainLayout_ContainsGitHubLink()
    {
        var cut = _testContext.RenderComponent<MainLayout>();

        var iconButtons = cut.FindComponents<MudIconButton>();
        Assert.That(iconButtons.Any(b => b.Markup.Contains("github.com")), Is.True);
    }

    [Test]
    public void MainLayout_ContainsMainContent()
    {
        var cut = _testContext.RenderComponent<MainLayout>();

        var mainContent = cut.FindComponent<MudMainContent>();
        Assert.That(mainContent, Is.Not.Null);
    }

    [Test]
    public void MainLayout_HasCustomTheme()
    {
        var cut = _testContext.RenderComponent<MainLayout>();

        var themeField = cut.Instance.GetType()
            .GetField("_myCustomTheme", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        
        var theme = themeField!.GetValue(cut.Instance) as MudTheme;
        Assert.That(theme, Is.Not.Null);
        Assert.That(theme!.PaletteDark.Primary.Value, Does.Contain("#fd49a8").Or.Contain("rgba(253,73,168"));
        Assert.That(theme.PaletteDark.Secondary.Value, Does.Contain("#4ebcef").Or.Contain("rgba(78,188,239"));
    }

    [Test]
    public void MainLayout_ContainsThemeProvider()
    {
        var cut = _testContext.RenderComponent<MainLayout>();

        var themeProvider = cut.FindComponent<MudThemeProvider>();
        Assert.That(themeProvider, Is.Not.Null);
    }

    [Test]
    public void MainLayout_ContainsDialogProvider()
    {
        var cut = _testContext.RenderComponent<MainLayout>();

        var dialogProvider = cut.FindComponent<MudDialogProvider>();
        Assert.That(dialogProvider, Is.Not.Null);
    }

    [Test]
    public void MainLayout_ContainsSnackbarProvider()
    {
        var cut = _testContext.RenderComponent<MainLayout>();

        var snackbarProvider = cut.FindComponent<MudSnackbarProvider>();
        Assert.That(snackbarProvider, Is.Not.Null);
    }

    [Test]
    public void MainLayout_ContainsPopoverProvider()
    {
        var cut = _testContext.RenderComponent<MainLayout>();

        var popoverProvider = cut.FindComponent<MudPopoverProvider>();
        Assert.That(popoverProvider, Is.Not.Null);
    }
}
