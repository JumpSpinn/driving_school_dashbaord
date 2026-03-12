# Blazor Component Tests

## Overview
This folder contains bUnit tests for Blazor components in the Web project.

## Testing Blazor Components

### Basic Setup
```csharp
[TestFixture]
public class MyComponentTests : TestContext
{
    [SetUp]
    public void Setup()
    {
        // Register MudBlazor services
        Services.AddMudServices();
        
        // Register any required services/mocks
        Services.AddSingleton<IMyService>(mockService);
    }
    
    [Test]
    public void MyComponent_RendersCorrectly()
    {
        var cut = RenderComponent<MyComponent>();
        
        // Assert on rendered output
        Assert.That(cut.Find("h1").TextContent, Is.EqualTo("Expected"));
    }
}
```

### Mocking Dependencies
For components that depend on API clients, you need to mock them:

```csharp
var mockApiClient = new Mock<StudentApiClient>(httpClient, logger);
mockApiClient.Setup(x => x.GetAllStudentsAsync())
    .ReturnsAsync(new List<StudentModel>());
    
Services.AddSingleton(mockApiClient.Object);
```

### Important Notes

1. **Interactive Components**: Components with `@rendermode InteractiveServer` require special handling
2. **Async Loading**: Use `WaitForState()` or `WaitForAssertion()` for async operations
3. **MudBlazor**: Always register MudBlazor services with `Services.AddMudServices()`
4. **Complex Components**: The Students, Instructors, DrivingSchools, etc. pages are very complex and difficult to test due to:
   - Multiple nested dialogs (MudDialog)
   - Forms with validation
   - Data grids with server-side operations
   - Heavy reliance on MudBlazor components

## Recommended Testing Strategy

For complex pages like Students, Instructors, etc., it's better to:

1. **Test the logic separately**: Extract business logic into services/helpers and test those
2. **Integration tests**: Use end-to-end tests with tools like Playwright or Selenium
3. **Unit test components**: Only unit test simple, isolated components
4. **Test the APIs**: Thoroughly test the API clients (already done)

## Current Test Coverage

- ✅ Error page (simple component)
- ✅ Home page (basic rendering and data loading)
- ✅ DialogData (configuration class)
- ❌ Students page (too complex for unit tests - recommend E2E testing)
- ❌ Instructors page (too complex for unit tests - recommend E2E testing)
- ❌ Other CRUD pages (too complex for unit tests - recommend E2E testing)
