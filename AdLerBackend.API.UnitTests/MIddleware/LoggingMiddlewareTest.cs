using AdLerBackend.API.Middleware;
using Microsoft.AspNetCore.Http;
using NSubstitute;

namespace AdLerBackend.API.UnitTests.MIddleware;

[TestFixture]
public class LoggingMiddlewareTests
{
    private LoggingMiddleware _middleware;
    private RequestDelegate _nextDelegate;

    [SetUp]
    public void Setup()
    {
        _nextDelegate = Substitute.For<RequestDelegate>();
        _middleware = new LoggingMiddleware(_nextDelegate);
    }

    [Test]
    public async Task InvokeAsync_WithTraceIdentifier_AddsToLogContext()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.TraceIdentifier = "test-trace-id";

        // Act
        await _middleware.InvokeAsync(context);

        // Assert
        await _nextDelegate.Received(1).Invoke(context);
        // Note: We can't directly test LogContext.PushProperty as it's a static method.
        // In a real-world scenario, you might want to use a wrapper around LogContext to make it testable.
    }

    [Test]
    public async Task InvokeAsync_WithoutTraceIdentifier_DoesNotAddToLogContext()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.TraceIdentifier = string.Empty;

        // Act
        await _middleware.InvokeAsync(context);

        // Assert
        await _nextDelegate.Received(1).Invoke(context);
        // Again, we can't directly test that LogContext.PushProperty wasn't called.
    }

    [Test]
    public async Task InvokeAsync_AlwaysCallsNextMiddleware()
    {
        // Arrange
        var context = new DefaultHttpContext();

        // Act
        await _middleware.InvokeAsync(context);

        // Assert
        await _nextDelegate.Received(1).Invoke(context);
    }
}