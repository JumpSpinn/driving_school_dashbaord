var builder = DistributedApplication.CreateBuilder(args);
var vue = true;

var apiService = builder.AddProject<Projects.DrivingSchoolDashboard_ApiService>("apiservice")
    .WithHttpHealthCheck("/health");

if (!vue)
{
    builder.AddProject<Projects.DrivingSchoolDashboard_Web>("webfrontend")
        .WithExternalHttpEndpoints()
        .WithHttpHealthCheck("/health")
        .WithReference(apiService)
        .WaitFor(apiService);
}
else
{
    builder.AddNpmApp("vue-frontend", "../DrivingSchoolDashboard.Vue", "dev")
        .WithReference(apiService)
        .WaitFor(apiService)
        .WithHttpEndpoint(env: "PORT");
}

builder.Build().Run();