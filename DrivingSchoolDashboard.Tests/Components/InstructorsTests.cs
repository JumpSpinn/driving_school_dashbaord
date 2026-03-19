namespace DrivingSchoolDashboard.Tests.Components;

[TestFixture]
public sealed class InstructorsTests
{
    private TestContext _testContext = null!;
    private Mock<IInstructorApiClient> _mockInstructorApi = null!;

    [SetUp]
    public void Setup()
    {
        _testContext = new TestContext();

        _mockInstructorApi = new Mock<IInstructorApiClient>();

        _mockInstructorApi.Setup(x => x.GetAllInstructorsAsync()).ReturnsAsync(new List<InstructorModel>());

        _testContext.Services.AddMudServices();
        
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
    public void Instructors_RendersPageTitle()
    {
        var cut = _testContext.RenderComponent<Instructors>();

        var pageTitle = cut.FindComponent<Microsoft.AspNetCore.Components.Web.PageTitle>();
        Assert.That(pageTitle.Instance.ChildContent, Is.Not.Null);
    }

    [Test]
    public void Instructors_CallsApiEndpointOnLoad()
    {
        var cut = _testContext.RenderComponent<Instructors>();

        cut.WaitForState(() => !cut.Instance.GetType()
            .GetField("_dataLoading", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
            .GetValue(cut.Instance)!.Equals(true), TimeSpan.FromSeconds(5));

        _mockInstructorApi.Verify(x => x.GetAllInstructorsAsync(), Times.Once);
    }

    [Test]
    public void Instructors_ShowsLoadingIndicatorInitially()
    {
        _mockInstructorApi.Setup(x => x.GetAllInstructorsAsync())
            .Returns(async () => { await Task.Delay(100); return new List<InstructorModel>(); });

        var cut = _testContext.RenderComponent<Instructors>();

        var progressIndicator = cut.FindComponents<MudProgressLinear>();
        Assert.That(progressIndicator, Is.Not.Empty);
    }

    [Test]
    public void Instructors_DisplaysDataGrid()
    {
        var instructors = new List<InstructorModel>
        {
            new() { Id = 1, FirstName = "John", LastName = "Doe", Mail = "john@test.com", TheoryLessons = new List<TheoryLessonModel>() },
            new() { Id = 2, FirstName = "Jane", LastName = "Smith", Mail = "jane@test.com", TheoryLessons = new List<TheoryLessonModel>() }
        };

        _mockInstructorApi.Setup(x => x.GetAllInstructorsAsync()).ReturnsAsync(instructors);

        var cut = _testContext.RenderComponent<Instructors>();

        cut.WaitForState(() => !cut.Instance.GetType()
            .GetField("_dataLoading", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
            .GetValue(cut.Instance)!.Equals(true), TimeSpan.FromSeconds(5));

        var dataGrid = cut.FindComponent<MudDataGrid<InstructorModel>>();
        Assert.That(dataGrid, Is.Not.Null);
    }

    [Test]
    public void Instructors_ShowsEmptyStateWhenNoData()
    {
        var cut = _testContext.RenderComponent<Instructors>();

        cut.WaitForState(() => !cut.Instance.GetType()
            .GetField("_dataLoading", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
            .GetValue(cut.Instance)!.Equals(true), TimeSpan.FromSeconds(5));

        var alert = cut.FindComponent<MudAlert>();
        Assert.That(alert, Is.Not.Null);
    }

    [Test]
    public void Instructors_HasCreateButton()
    {
        var cut = _testContext.RenderComponent<Instructors>();

        var buttons = cut.FindComponents<MudButton>();
        Assert.That(buttons.Any(b => b.Markup.Contains("Eintragen")), Is.True);
    }

    [Test]
    public void Instructors_DisplaysTheoryLessonCount()
    {
        var instructors = new List<InstructorModel>
        {
            new() 
            { 
                Id = 1, 
                FirstName = "John", 
                LastName = "Doe",
                Mail = "john@test.com",
                TheoryLessons = new List<TheoryLessonModel>
                {
                    new() { Id = 1, Name = "Lesson 1", Topic = "Topic 1" },
                    new() { Id = 2, Name = "Lesson 2", Topic = "Topic 2" }
                }
            }
        };

        _mockInstructorApi.Setup(x => x.GetAllInstructorsAsync()).ReturnsAsync(instructors);

        var cut = _testContext.RenderComponent<Instructors>();

        cut.WaitForState(() => !cut.Instance.GetType()
            .GetField("_dataLoading", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
            .GetValue(cut.Instance)!.Equals(true), TimeSpan.FromSeconds(5));

        var chips = cut.FindComponents<MudChip<string>>();
        Assert.That(chips.Any(c => c.Markup.Contains("2")), Is.True);
    }
}
