using ExampleAspireBlazorApiProject.ServiceDefaults;
using ExampleAspireBlazorApiProject.Web.Apis;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services
    .AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddOutputCache();
builder.Services.AddBlazorBootstrap();

// add https clients
builder.Services.AddHttpClient<DrivingSchoolApiClient>(client =>
{
    client.BaseAddress = new("https+http://apiservice");
});
builder.Services.AddHttpClient<DriverApiClient>(client =>
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