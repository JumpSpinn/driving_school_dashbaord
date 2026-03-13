namespace ExampleAspireBlazorApiProject.Tests.Components;

[TestFixture]
public sealed class TheoryLessonsTests
{
    private TestContext _testContext = null!;
    private Mock<ITheoryLessonApiClient> _mockTheoryLessonApi = null!;
    private Mock<IInstructorApiClient> _mockInstructorApi = null!;

    [SetUp]
    public void Setup()
    {
        _testContext = new TestContext();

        _mockTheoryLessonApi = new Mock<ITheoryLessonApiClient>();
        _mockInstructorApi = new Mock<IInstructorApiClient>();

        _mockTheoryLessonApi.Setup(x => x.GetAllLessonsAsync()).ReturnsAsync(new List<TheoryLessonModel>());
        _mockInstructorApi.Setup(x => x.GetAllInstructorsAsync()).ReturnsAsync(new List<InstructorModel>());

        _testContext.Services.AddMudServices();
        
        _testContext.Services.AddSingleton(_mockTheoryLessonApi.Object);
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
    public void TheoryLessons_RendersPageTitle()
    {
        var cut = _testContext.RenderComponent<TheoryLessons>();

        var pageTitle = cut.FindComponent<Microsoft.AspNetCore.Components.Web.PageTitle>();
        Assert.That(pageTitle.Instance.ChildContent, Is.Not.Null);
    }

    [Test]
    public void TheoryLessons_CallsAllApiEndpointsOnLoad()
    {
        var cut = _testContext.RenderComponent<TheoryLessons>();

        cut.WaitForState(() => !cut.Instance.GetType()
            .GetField("_dataLoading", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
            .GetValue(cut.Instance)!.Equals(true), TimeSpan.FromSeconds(5));

        _mockTheoryLessonApi.Verify(x => x.GetAllLessonsAsync(), Times.Once);
        _mockInstructorApi.Verify(x => x.GetAllInstructorsAsync(), Times.Once);
    }

    [Test]
    public void TheoryLessons_ShowsLoadingIndicatorInitially()
    {
        _mockTheoryLessonApi.Setup(x => x.GetAllLessonsAsync())
            .Returns(async () => { await Task.Delay(100); return new List<TheoryLessonModel>(); });

        var cut = _testContext.RenderComponent<TheoryLessons>();

        var progressIndicator = cut.FindComponents<MudProgressLinear>();
        Assert.That(progressIndicator, Is.Not.Empty);
    }

    [Test]
    public void TheoryLessons_DisplaysDataGrid()
    {
        var lessons = new List<TheoryLessonModel>
        {
            new() 
            { 
                Id = 1, 
                Name = "Lesson 1", 
                Topic = "Topic 1", 
                DayOfWeek = DayOfWeek.Monday,
                StartTime = new TimeOnly(18, 0),
                DurationMinutes = 90,
                MaxStudents = 20,
                Price = 25.00m,
                IsBasic = true
            },
            new() 
            { 
                Id = 2, 
                Name = "Lesson 2", 
                Topic = "Topic 2", 
                DayOfWeek = DayOfWeek.Wednesday,
                StartTime = new TimeOnly(19, 0),
                DurationMinutes = 90,
                MaxStudents = 15,
                Price = 30.00m,
                IsBasic = false
            }
        };

        _mockTheoryLessonApi.Setup(x => x.GetAllLessonsAsync()).ReturnsAsync(lessons);

        var cut = _testContext.RenderComponent<TheoryLessons>();

        cut.WaitForState(() => !cut.Instance.GetType()
            .GetField("_dataLoading", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
            .GetValue(cut.Instance)!.Equals(true), TimeSpan.FromSeconds(5));

        var dataGrid = cut.FindComponent<MudDataGrid<TheoryLessonModel>>();
        Assert.That(dataGrid, Is.Not.Null);
    }

    [Test]
    public void TheoryLessons_ShowsEmptyStateWhenNoData()
    {
        var cut = _testContext.RenderComponent<TheoryLessons>();

        cut.WaitForState(() => !cut.Instance.GetType()
            .GetField("_dataLoading", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
            .GetValue(cut.Instance)!.Equals(true), TimeSpan.FromSeconds(5));

        var alert = cut.FindComponent<MudAlert>();
        Assert.That(alert, Is.Not.Null);
    }

    [Test]
    public void TheoryLessons_HasCreateButton()
    {
        var cut = _testContext.RenderComponent<TheoryLessons>();

        var buttons = cut.FindComponents<MudButton>();
        Assert.That(buttons.Any(b => b.Markup.Contains("Erstellen")), Is.True);
    }

    [Test]
    public void TheoryLessons_DisplaysLessonType()
    {
        var lessons = new List<TheoryLessonModel>
        {
            new() 
            { 
                Id = 1, 
                Name = "Basic Lesson", 
                Topic = "Topic", 
                DayOfWeek = DayOfWeek.Monday,
                IsBasic = true
            },
            new() 
            { 
                Id = 2, 
                Name = "Special Lesson", 
                Topic = "Topic", 
                DayOfWeek = DayOfWeek.Tuesday,
                IsBasic = false
            }
        };

        _mockTheoryLessonApi.Setup(x => x.GetAllLessonsAsync()).ReturnsAsync(lessons);

        var cut = _testContext.RenderComponent<TheoryLessons>();

        cut.WaitForState(() => !cut.Instance.GetType()
            .GetField("_dataLoading", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
            .GetValue(cut.Instance)!.Equals(true), TimeSpan.FromSeconds(5));

        var chips = cut.FindComponents<MudChip<string>>();
        Assert.That(chips.Any(c => c.Markup.Contains("Basisstoff")), Is.True);
        Assert.That(chips.Any(c => c.Markup.Contains("Spezialstoff")), Is.True);
    }

    [Test]
    public void TheoryLessons_DisplaysInstructorInfo()
    {
        var lessons = new List<TheoryLessonModel>
        {
            new() 
            { 
                Id = 1, 
                Name = "Lesson 1", 
                Topic = "Topic 1", 
                DayOfWeek = DayOfWeek.Monday,
                Instructor = new InstructorModel { Id = 1, FirstName = "John", LastName = "Doe" }
            }
        };

        _mockTheoryLessonApi.Setup(x => x.GetAllLessonsAsync()).ReturnsAsync(lessons);

        var cut = _testContext.RenderComponent<TheoryLessons>();

        cut.WaitForState(() => !cut.Instance.GetType()
            .GetField("_dataLoading", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
            .GetValue(cut.Instance)!.Equals(true), TimeSpan.FromSeconds(5));

        var markup = cut.Markup;
        Assert.That(markup, Does.Contain("John Doe"));
    }
}
