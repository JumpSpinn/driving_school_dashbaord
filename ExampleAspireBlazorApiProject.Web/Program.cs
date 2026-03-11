var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services
    .AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddOutputCache();
builder.Services.AddMudServices();

// add https clients
builder.Services.AddHttpClient<DrivingSchoolApiClient>(client =>
{
    client.BaseAddress = new("https+http://apiservice");
});

builder.Services.AddHttpClient<StudentApiClient>(client =>
{
    client.BaseAddress = new("https+http://apiservice");
});

builder.Services.AddHttpClient<InstructorApiClient>(client =>
{
    client.BaseAddress = new("https+http://apiservice");
});

builder.Services.AddHttpClient<TheoryLessonApiClient>(client =>
{
    client.BaseAddress = new("https+http://apiservice");
});

builder.Services.AddHttpClient<CourseBookingApiClient>(client =>
{
    client.BaseAddress = new("https+http://apiservice");
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAntiforgery();
app.UseOutputCache();
app.MapStaticAssets();
app
    .MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();
app.MapDefaultEndpoints();
app.Run();