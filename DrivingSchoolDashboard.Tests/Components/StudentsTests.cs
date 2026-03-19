namespace DrivingSchoolDashboard.Tests.Components;

[TestFixture]
public sealed class StudentsTests
{
    private TestContext _testContext = null!;
    private Mock<IStudentApiClient> _mockStudentApi = null!;
    private Mock<IDrivingSchoolApiClient> _mockDrivingSchoolApi = null!;

    [SetUp]
    public void Setup()
    {
        _testContext = new TestContext();

        _mockStudentApi = new Mock<IStudentApiClient>();
        _mockDrivingSchoolApi = new Mock<IDrivingSchoolApiClient>();

        _mockStudentApi.Setup(x => x.GetAllStudentsAsync()).ReturnsAsync(new List<StudentModel>());
        _mockDrivingSchoolApi.Setup(x => x.GetAllDrivingSchoolsAsync()).ReturnsAsync(new List<DrivingSchoolModel>());

        _testContext.Services.AddMudServices();
        
        _testContext.Services.AddSingleton(_mockStudentApi.Object);
        _testContext.Services.AddSingleton(_mockDrivingSchoolApi.Object);

        _testContext.JSInterop.Mode = JSRuntimeMode.Loose;
        
        _testContext.ComponentFactories.AddStub<MudPopover>();
    }

    [TearDown]
    public void TearDown()
    {
        _testContext?.Dispose();
    }

    [Test]
    public void Students_RendersPageTitle()
    {
        var cut = _testContext.RenderComponent<Students>();

        var pageTitle = cut.FindComponent<Microsoft.AspNetCore.Components.Web.PageTitle>();
        Assert.That(pageTitle.Instance.ChildContent, Is.Not.Null);
    }

    [Test]
    public void Students_CallsAllApiEndpointsOnLoad()
    {
        var cut = _testContext.RenderComponent<Students>();

        cut.WaitForState(() => !cut.Instance.GetType()
            .GetField("_dataLoading", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
            .GetValue(cut.Instance)!.Equals(true), TimeSpan.FromSeconds(5));

        _mockStudentApi.Verify(x => x.GetAllStudentsAsync(), Times.Once);
        _mockDrivingSchoolApi.Verify(x => x.GetAllDrivingSchoolsAsync(), Times.Once);
    }

    [Test]
    public void Students_ShowsLoadingIndicatorInitially()
    {
        _mockStudentApi.Setup(x => x.GetAllStudentsAsync())
            .Returns(async () => { await Task.Delay(100); return new List<StudentModel>(); });

        var cut = _testContext.RenderComponent<Students>();

        var progressIndicator = cut.FindComponents<MudProgressLinear>();
        Assert.That(progressIndicator, Is.Not.Empty);
    }

    [Test]
    public void Students_DisplaysDataGrid()
    {
        var students = new List<StudentModel>
        {
            new() { Id = 1, FirstName = "John", LastName = "Doe", Mail = "john@test.com", HasPassed = true, CourseBookings = new List<CourseBookingModel>() },
            new() { Id = 2, FirstName = "Jane", LastName = "Smith", Mail = "jane@test.com", HasPassed = false, CourseBookings = new List<CourseBookingModel>() }
        };

        _mockStudentApi.Setup(x => x.GetAllStudentsAsync()).ReturnsAsync(students);

        var cut = _testContext.RenderComponent<Students>();

        cut.WaitForState(() => !cut.Instance.GetType()
            .GetField("_dataLoading", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
            .GetValue(cut.Instance)!.Equals(true), TimeSpan.FromSeconds(5));

        var dataGrid = cut.FindComponent<MudDataGrid<StudentModel>>();
        Assert.That(dataGrid, Is.Not.Null);
    }

    [Test]
    public void Students_ShowsEmptyStateWhenNoData()
    {
        var cut = _testContext.RenderComponent<Students>();

        cut.WaitForState(() => !cut.Instance.GetType()
            .GetField("_dataLoading", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
            .GetValue(cut.Instance)!.Equals(true), TimeSpan.FromSeconds(5));

        var alert = cut.FindComponent<MudAlert>();
        Assert.That(alert, Is.Not.Null);
    }

    [Test]
    public void Students_HasCreateButton()
    {
        var cut = _testContext.RenderComponent<Students>();

        var buttons = cut.FindComponents<MudButton>();
        Assert.That(buttons.Any(b => b.Markup.Contains("Eintragen")), Is.True);
    }

    [Test]
    public void Students_DisplaysPassedStatus()
    {
        var students = new List<StudentModel>
        {
            new() 
            { 
                Id = 1, 
                FirstName = "John", 
                LastName = "Doe", 
                Mail = "john@test.com",
                HasPassed = true,
                CourseBookings = new List<CourseBookingModel>()
            },
            new() 
            { 
                Id = 2, 
                FirstName = "Jane", 
                LastName = "Smith", 
                Mail = "jane@test.com",
                HasPassed = false,
                CourseBookings = new List<CourseBookingModel>()
            }
        };

        _mockStudentApi.Setup(x => x.GetAllStudentsAsync()).ReturnsAsync(students);

        var cut = _testContext.RenderComponent<Students>();

        cut.WaitForState(() => !cut.Instance.GetType()
            .GetField("_dataLoading", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
            .GetValue(cut.Instance)!.Equals(true), TimeSpan.FromSeconds(5));

        var chips = cut.FindComponents<MudChip<string>>();
        Assert.That(chips.Any(c => c.Markup.Contains("Ja")), Is.True);
        Assert.That(chips.Any(c => c.Markup.Contains("Nein")), Is.True);
    }

    [Test]
    public void Students_DisplaysDrivingSchoolInfo()
    {
        var students = new List<StudentModel>
        {
            new() 
            { 
                Id = 1, 
                FirstName = "John", 
                LastName = "Doe", 
                Mail = "john@test.com",
                DrivingSchool = new DrivingSchoolModel { Id = 1, Name = "Test School" },
                CourseBookings = new List<CourseBookingModel>()
            }
        };

        _mockStudentApi.Setup(x => x.GetAllStudentsAsync()).ReturnsAsync(students);

        var cut = _testContext.RenderComponent<Students>();

        cut.WaitForState(() => !cut.Instance.GetType()
            .GetField("_dataLoading", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
            .GetValue(cut.Instance)!.Equals(true), TimeSpan.FromSeconds(5));

        var chips = cut.FindComponents<MudChip<string>>();
        Assert.That(chips.Any(c => c.Markup.Contains("Test School")), Is.True);
    }
}
