namespace DrivingSchoolDashboard.Tests.Components;

[TestFixture]
public sealed class CourseBookingsTests
{
    private TestContext _testContext = null!;
    private Mock<ICourseBookingApiClient> _mockCourseBookingApi = null!;
    private Mock<IStudentApiClient> _mockStudentApi = null!;
    private Mock<ITheoryLessonApiClient> _mockTheoryLessonApi = null!;

    [SetUp]
    public void Setup()
    {
        _testContext = new TestContext();

        _mockCourseBookingApi = new Mock<ICourseBookingApiClient>();
        _mockStudentApi = new Mock<IStudentApiClient>();
        _mockTheoryLessonApi = new Mock<ITheoryLessonApiClient>();

        _mockCourseBookingApi.Setup(x => x.GetAllCourseBookingsAsync()).ReturnsAsync(new List<CourseBookingModel>());
        _mockStudentApi.Setup(x => x.GetAllStudentsAsync()).ReturnsAsync(new List<StudentModel>());
        _mockTheoryLessonApi.Setup(x => x.GetAllLessonsAsync()).ReturnsAsync(new List<TheoryLessonModel>());

        _testContext.Services.AddMudServices();
        
        _testContext.Services.AddSingleton(_mockCourseBookingApi.Object);
        _testContext.Services.AddSingleton(_mockStudentApi.Object);
        _testContext.Services.AddSingleton(_mockTheoryLessonApi.Object);

        _testContext.JSInterop.Mode = JSRuntimeMode.Loose;
        
        _testContext.ComponentFactories.AddStub<MudPopover>();
    }

    [TearDown]
    public void TearDown()
    {
        _testContext?.Dispose();
    }

    [Test]
    public void CourseBookings_RendersPageTitle()
    {
        var cut = _testContext.RenderComponent<CourseBookings>();

        var pageTitle = cut.FindComponent<Microsoft.AspNetCore.Components.Web.PageTitle>();
        Assert.That(pageTitle.Instance.ChildContent, Is.Not.Null);
    }

    [Test]
    public void CourseBookings_CallsAllApiEndpointsOnLoad()
    {
        var cut = _testContext.RenderComponent<CourseBookings>();

        cut.WaitForState(() => !cut.Instance.GetType()
            .GetField("_dataLoading", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
            .GetValue(cut.Instance)!.Equals(true), TimeSpan.FromSeconds(5));

        _mockCourseBookingApi.Verify(x => x.GetAllCourseBookingsAsync(), Times.Once);
        _mockStudentApi.Verify(x => x.GetAllStudentsAsync(), Times.Once);
        _mockTheoryLessonApi.Verify(x => x.GetAllLessonsAsync(), Times.Once);
    }

    [Test]
    public void CourseBookings_ShowsLoadingIndicatorInitially()
    {
        _mockCourseBookingApi.Setup(x => x.GetAllCourseBookingsAsync())
            .Returns(async () => { await Task.Delay(100); return new List<CourseBookingModel>(); });

        var cut = _testContext.RenderComponent<CourseBookings>();

        var progressIndicator = cut.FindComponents<MudProgressLinear>();
        Assert.That(progressIndicator, Is.Not.Empty);
    }

    [Test]
    public void CourseBookings_DisplaysDataGrid()
    {
        var bookings = new List<CourseBookingModel>
        {
            new() { Id = 1, StudentId = 1, TheoryLessonId = 1, BookingDate = DateTime.Now },
            new() { Id = 2, StudentId = 2, TheoryLessonId = 2, BookingDate = DateTime.Now }
        };

        _mockCourseBookingApi.Setup(x => x.GetAllCourseBookingsAsync()).ReturnsAsync(bookings);

        var cut = _testContext.RenderComponent<CourseBookings>();

        cut.WaitForState(() => !cut.Instance.GetType()
            .GetField("_dataLoading", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
            .GetValue(cut.Instance)!.Equals(true), TimeSpan.FromSeconds(5));

        var dataGrid = cut.FindComponent<MudDataGrid<CourseBookingModel>>();
        Assert.That(dataGrid, Is.Not.Null);
    }

    [Test]
    public void CourseBookings_ShowsEmptyStateWhenNoData()
    {
        var cut = _testContext.RenderComponent<CourseBookings>();

        cut.WaitForState(() => !cut.Instance.GetType()
            .GetField("_dataLoading", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
            .GetValue(cut.Instance)!.Equals(true), TimeSpan.FromSeconds(5));

        var alert = cut.FindComponent<MudAlert>();
        Assert.That(alert, Is.Not.Null);
    }

    [Test]
    public void CourseBookings_HasCreateButton()
    {
        var cut = _testContext.RenderComponent<CourseBookings>();

        var buttons = cut.FindComponents<MudButton>();
        Assert.That(buttons.Any(b => b.Markup.Contains("Neue Buchung")), Is.True);
    }
}
