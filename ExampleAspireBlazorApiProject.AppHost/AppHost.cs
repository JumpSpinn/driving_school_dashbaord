var builder = DistributedApplication.CreateBuilder(args);
var vue = false;

var apiService = builder.AddProject<Projects.ExampleAspireBlazorApiProject_ApiService>("apiservice")
    .WithHttpHealthCheck("/health");

if (!vue)
{
    builder.AddProject<Projects.ExampleAspireBlazorApiProject_Web>("webfrontend")
        .WithExternalHttpEndpoints()
        .WithHttpHealthCheck("/health")
        .WithReference(apiService)
        .WaitFor(apiService);
}
else
{
    builder.AddNpmApp("vue-frontend", "../ExampleAspireBlazorApiProject.Vue", "dev")
        .WithReference(apiService)
        .WaitFor(apiService)
        .WithHttpEndpoint(env: "PORT");
}

builder.Build().Run();