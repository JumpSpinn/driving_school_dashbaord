namespace ExampleAspireBlazorApiProject.Tests.Components;

[TestFixture]
public sealed class HomePageTests
{
    private TestContext _testContext = null!;
    private Mock<ICourseBookingApiClient> _mockCourseBookingApi = null!;
    private Mock<IInstructorApiClient> _mockInstructorApi = null!;
    private Mock<IStudentApiClient> _mockStudentApi = null!;
    private Mock<IDrivingSchoolApiClient> _mockDrivingSchoolApi = null!;
    private Mock<ITheoryLessonApiClient> _mockTheoryLessonApi = null!;

    [SetUp]
    public void Setup()
    {
        _testContext = new TestContext();

        // Create mock API clients
        _mockCourseBookingApi = new Mock<ICourseBookingApiClient>();
        _mockInstructorApi = new Mock<IInstructorApiClient>();
        _mockStudentApi = new Mock<IStudentApiClient>();
        _mockDrivingSchoolApi = new Mock<IDrivingSchoolApiClient>();
        _mockTheoryLessonApi = new Mock<ITheoryLessonApiClient>();

        // Setup default return values (empty lists)
        _mockCourseBookingApi.Setup(x => x.GetAllCourseBookingsAsync()).ReturnsAsync(new List<CourseBookingModel>());
        _mockInstructorApi.Setup(x => x.GetAllInstructorsAsync()).ReturnsAsync(new List<InstructorModel>());
        _mockStudentApi.Setup(x => x.GetAllStudentsAsync()).ReturnsAsync(new List<StudentModel>());
        _mockDrivingSchoolApi.Setup(x => x.GetAllDrivingSchoolsAsync()).ReturnsAsync(new List<DrivingSchoolModel>());
        _mockTheoryLessonApi.Setup(x => x.GetAllLessonsAsync()).ReturnsAsync(new List<TheoryLessonModel>());

        // Register mocked services BEFORE MudServices
        _testContext.Services.AddSingleton(_mockCourseBookingApi.Object);
        _testContext.Services.AddSingleton(_mockInstructorApi.Object);
        _testContext.Services.AddSingleton(_mockStudentApi.Object);
        _testContext.Services.AddSingleton(_mockDrivingSchoolApi.Object);
        _testContext.Services.AddSingleton(_mockTheoryLessonApi.Object);

        // Register MudBlazor services
        _testContext.Services.AddMudServices();

        // Setup JSInterop AFTER all services registered
        _testContext.JSInterop.Mode = JSRuntimeMode.Loose;
    }

    [TearDown]
    public void TearDown()
    {
        _testContext?.Dispose();
    }

    [Test]
    public void HomePage_RendersPageTitle()
    {
        var cut = _testContext.RenderComponent<Home>();

        var pageTitle = cut.FindComponent<Microsoft.AspNetCore.Components.Web.PageTitle>();
        Assert.That(pageTitle.Instance.ChildContent, Is.Not.Null);
    }

    [Test]
    public void HomePage_CallsAllApiEndpointsOnLoad()
    {
        var cut = _testContext.RenderComponent<Home>();

        // Wait for async initialization
        cut.WaitForState(() => !cut.Instance.GetType()
            .GetField("_dataLoading", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
            .GetValue(cut.Instance)!.Equals(true), TimeSpan.FromSeconds(5));

        _mockCourseBookingApi.Verify(x => x.GetAllCourseBookingsAsync(), Times.Once);
        _mockInstructorApi.Verify(x => x.GetAllInstructorsAsync(), Times.Once);
        _mockStudentApi.Verify(x => x.GetAllStudentsAsync(), Times.Once);
        _mockDrivingSchoolApi.Verify(x => x.GetAllDrivingSchoolsAsync(), Times.Once);
        _mockTheoryLessonApi.Verify(x => x.GetAllLessonsAsync(), Times.Once);
    }

    [Test]
    public void HomePage_ShowsLoadingIndicatorInitially()
    {
        // Setup APIs to delay response
        _mockCourseBookingApi.Setup(x => x.GetAllCourseBookingsAsync())
            .Returns(async () => { await Task.Delay(100); return new List<CourseBookingModel>(); });

        var cut = _testContext.RenderComponent<Home>();

        // Check for progress indicator
        var progressIndicator = cut.FindComponents<MudProgressLinear>();
        Assert.That(progressIndicator, Is.Not.Empty);
    }

    [Test]
    public void HomePage_DisplaysDataAfterLoading()
    {
        // Setup sample data
        var students = new List<StudentModel>
        {
            new() { Id = 1, FirstName = "John", LastName = "Doe", Mail = "john@test.com", HasPassed = true },
            new() { Id = 2, FirstName = "Jane", LastName = "Smith", Mail = "jane@test.com", HasPassed = false }
        };

        _mockStudentApi.Setup(x => x.GetAllStudentsAsync()).ReturnsAsync(students);

        var cut = _testContext.RenderComponent<Home>();

        // Wait for loading to complete
        cut.WaitForState(() => !cut.Instance.GetType()
            .GetField("_dataLoading", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
            .GetValue(cut.Instance)!.Equals(true), TimeSpan.FromSeconds(5));

        // Should display student count
        var markup = cut.Markup;
        Assert.That(markup, Does.Contain("2")); // Should show count of 2 students somewhere
    }
}
