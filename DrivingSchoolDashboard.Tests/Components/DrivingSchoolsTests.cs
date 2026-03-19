namespace DrivingSchoolDashboard.Tests.Components;

[TestFixture]
public sealed class DrivingSchoolsTests
{
    private TestContext _testContext = null!;
    private Mock<IDrivingSchoolApiClient> _mockDrivingSchoolApi = null!;
    private Mock<IInstructorApiClient> _mockInstructorApi = null!;

    [SetUp]
    public void Setup()
    {
        _testContext = new TestContext();

        _mockDrivingSchoolApi = new Mock<IDrivingSchoolApiClient>();
        _mockInstructorApi = new Mock<IInstructorApiClient>();

        _mockDrivingSchoolApi.Setup(x => x.GetAllDrivingSchoolsAsync()).ReturnsAsync(new List<DrivingSchoolModel>());
        _mockInstructorApi.Setup(x => x.GetAllInstructorsAsync()).ReturnsAsync(new List<InstructorModel>());

        _testContext.Services.AddMudServices();
        
        _testContext.Services.AddSingleton(_mockDrivingSchoolApi.Object);
        _testContext.Services.AddSingleton(_mockInstructorApi.Object);

        _testContext.JSInterop.Mode = JSRuntimeMode.Loose;
        
        _testContext.ComponentFactories.AddStub<MudPopover>();
    }

    [TearDown]
    public void TearDown()
    {
        _testContext?.Dispose();
    }

    [Test]
    public void DrivingSchools_RendersPageTitle()
    {
        var cut = _testContext.RenderComponent<DrivingSchools>();

        var pageTitle = cut.FindComponent<Microsoft.AspNetCore.Components.Web.PageTitle>();
        Assert.That(pageTitle.Instance.ChildContent, Is.Not.Null);
    }

    [Test]
    public void DrivingSchools_CallsAllApiEndpointsOnLoad()
    {
        var cut = _testContext.RenderComponent<DrivingSchools>();

        cut.WaitForState(() => !cut.Instance.GetType()
            .GetField("_dataLoading", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
            .GetValue(cut.Instance)!.Equals(true), TimeSpan.FromSeconds(5));

        _mockDrivingSchoolApi.Verify(x => x.GetAllDrivingSchoolsAsync(), Times.Once);
        _mockInstructorApi.Verify(x => x.GetAllInstructorsAsync(), Times.Once);
    }

    [Test]
    public void DrivingSchools_ShowsLoadingIndicatorInitially()
    {
        _mockDrivingSchoolApi.Setup(x => x.GetAllDrivingSchoolsAsync())
            .Returns(async () => { await Task.Delay(100); return new List<DrivingSchoolModel>(); });

        var cut = _testContext.RenderComponent<DrivingSchools>();

        var progressIndicator = cut.FindComponents<MudProgressLinear>();
        Assert.That(progressIndicator, Is.Not.Empty);
    }

    [Test]
    public void DrivingSchools_DisplaysDataGrid()
    {
        var schools = new List<DrivingSchoolModel>
        {
            new() { Id = 1, Name = "Test School 1", OwnerId = 1, Students = new List<StudentModel>() },
            new() { Id = 2, Name = "Test School 2", OwnerId = 2, Students = new List<StudentModel>() }
        };

        _mockDrivingSchoolApi.Setup(x => x.GetAllDrivingSchoolsAsync()).ReturnsAsync(schools);

        var cut = _testContext.RenderComponent<DrivingSchools>();

        cut.WaitForState(() => !cut.Instance.GetType()
            .GetField("_dataLoading", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
            .GetValue(cut.Instance)!.Equals(true), TimeSpan.FromSeconds(5));

        var dataGrid = cut.FindComponent<MudDataGrid<DrivingSchoolModel>>();
        Assert.That(dataGrid, Is.Not.Null);
    }

    [Test]
    public void DrivingSchools_ShowsEmptyStateWhenNoData()
    {
        var cut = _testContext.RenderComponent<DrivingSchools>();

        cut.WaitForState(() => !cut.Instance.GetType()
            .GetField("_dataLoading", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
            .GetValue(cut.Instance)!.Equals(true), TimeSpan.FromSeconds(5));

        var alert = cut.FindComponent<MudAlert>();
        Assert.That(alert, Is.Not.Null);
    }

    [Test]
    public void DrivingSchools_HasCreateButton()
    {
        var cut = _testContext.RenderComponent<DrivingSchools>();

        var buttons = cut.FindComponents<MudButton>();
        Assert.That(buttons.Any(b => b.Markup.Contains("Registrieren")), Is.True);
    }

    [Test]
    public void DrivingSchools_DisplaysStudentCount()
    {
        var schools = new List<DrivingSchoolModel>
        {
            new() 
            { 
                Id = 1, 
                Name = "Test School", 
                Students = new List<StudentModel>
                {
                    new() { Id = 1, FirstName = "John", LastName = "Doe" },
                    new() { Id = 2, FirstName = "Jane", LastName = "Smith" }
                }
            }
        };

        _mockDrivingSchoolApi.Setup(x => x.GetAllDrivingSchoolsAsync()).ReturnsAsync(schools);

        var cut = _testContext.RenderComponent<DrivingSchools>();

        cut.WaitForState(() => !cut.Instance.GetType()
            .GetField("_dataLoading", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
            .GetValue(cut.Instance)!.Equals(true), TimeSpan.FromSeconds(5));

        var chips = cut.FindComponents<MudChip<string>>();
        Assert.That(chips.Any(c => c.Markup.Contains("2")), Is.True);
    }
}
